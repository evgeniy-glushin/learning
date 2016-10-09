using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using static System.Diagnostics.Debug;

namespace WebScanner
{

    class Scanner
    {

        public void Stop()
        {
            _cancellationSrc?.Cancel();
        }

        CancellationTokenSource _cancellationSrc;
        public async Task Scann(Uri rootUrl, string searchTxt, int maxThreads, int maxUrls)
        {
            
            _cancellationSrc = new CancellationTokenSource();

            int urlCounter = 0, level = 0;
            var _nodesQueue = new ConcurrentQueue<Node>();
            _nodesQueue.Enqueue(new Node(rootUrl, 0));

            WriteLine($"begin scanning threadId: {Thread.CurrentThread.ManagedThreadId}");

            try
            {
                while (_nodesQueue.Any())
                {
                    _cancellationSrc.Token.ThrowIfCancellationRequested();

                    //так как мы используем алгоритм поиска в ширину то возможна паралельная загрузка 
                    //и сканирование страниц только одного уровня графа

                    //находим и записываем в список все страницы текущего уровня
                    List<Node> nodesOfLevel = GetAllPagesOfTheLevel(_nodesQueue, level);

                    var callThreadContext = SynchronizationContext.Current;
                    if (callThreadContext == null) throw new NullReferenceException("Current synchronization context is null!");

                    //var scheduler = TaskScheduler.FromCurrentSynchronizationContext();

                    var scannOperations = nodesOfLevel.Select(node =>
                        new Action(() => //download and scann the page
                    {
                        HttpClient http = null;
                        try
                        {
                            node.State = NodeState.Downloading;
                            /*OnNodeStateChanged?.Invoke(node)*/
                            callThreadContext.Send(delegate (object s) { OnNodeStateChanged?.Invoke(node); }, null);

                            //download the page
                            http = new HttpClient();
                            var pageStr = http.GetStringAsync(node.Url).Result;
                            //Thread.Sleep(1000);
                            //detect the search text on the page
                            node.State = IsSearchTxtFound(searchTxt, pageStr) ? NodeState.Found : NodeState.NotFound;
                            callThreadContext.Send(delegate (object s) { OnNodeStateChanged?.Invoke(node); }, null);

                            WriteLine($"url: {node.Url}; state: {node.State}; level: {level}; threadId: {Thread.CurrentThread.ManagedThreadId}");

                            var urlsOnThePage = GetPageUrls(pageStr);
                            foreach (var urlStr in urlsOnThePage)
                            {
                                if (urlCounter <= maxUrls)
                                {
                                    var newNode = new Node(new Uri(urlStr), level + 1);
                                    _nodesQueue.Enqueue(newNode);
                                    Interlocked.Increment(ref urlCounter); //thread safe increment
                                    //Thread.Sleep(1000);
                                    callThreadContext.Send(delegate (object s) { OnUrlFound?.Invoke(newNode); }, null);
                                }
                                else
                                    break;
                            }
                        }

                        catch (Exception e)
                        {
                            node.ErrorMsg = e.InnerException?.Message;//TODO: refactor
                            node.State = NodeState.Error;
                            callThreadContext.Send(delegate (object s) { OnNodeStateChanged?.Invoke(node); }, null);
                            WriteLine($"url: {node.Url}; state: {node.State}; msg: {node.ErrorMsg} level: {level}; threadId: {Thread.CurrentThread.ManagedThreadId}");
                        }
                        finally
                        {
                            http?.Dispose();
                        }
                    }));

                    //запускаем паралельную загрузку и сканирование всех страниц текущего уровня вложености графа
                    await RunAsynchronously(scannOperations, maxThreads);
                 
                    ++level;
                }
            }
            catch (OperationCanceledException ex)
            {
                OnCanceled?.Invoke();
            }
            catch (Exception ex)
            {
                throw;
            }

            _cancellationSrc = null;
        }

        private async Task RunAsynchronously(IEnumerable<Action> operations, int maxThreads)
        {
            int finished = 0,
                total = operations.Count();

            while (finished < total)
            {
                var tasks = operations
                    .Skip(finished)
                    .Take(maxThreads)
                    .Select(f => Task.Run(f))
                    .ToList();

                await Task.WhenAll(tasks);
                finished += tasks.Count;
            }
        }


        private static List<Node> GetAllPagesOfTheLevel(ConcurrentQueue<Node> nodesQueue, int level)
        {
            List<Node> pagesWithSameLavel = new List<Node>();
            while (nodesQueue.Any() && nodesQueue.First().Level == level)
            {
                Node levelPage;
                nodesQueue.TryDequeue(out levelPage);
                pagesWithSameLavel.Add(levelPage);
            }

            return pagesWithSameLavel;
        }

        /// <summary>
        /// Gets all found links on the page.
        /// </summary>
        /// <param name="pageStr"></param>
        /// <returns></returns>
        IEnumerable<string> GetPageUrls(string pageStr)
        {
            var urlRegex = new Regex("href=\"(?<link>\\S+)\"");
            var urlsOnThePage = from Match m in urlRegex.Matches(pageStr)
                                let link = m.Groups["link"].Value
                                where link.StartsWith("https://") || link.StartsWith("http://")
                                select link;

            return urlsOnThePage;
        }

        bool IsSearchTxtFound(string txt, string pageStr)
        {
            return pageStr.ToLower()
                .Contains(txt.ToLower());
        }

        public event Action<Node> OnUrlFound;
        public event Action<Node> OnNodeStateChanged;
        public event Action OnCanceled;
    }
}

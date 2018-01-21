using System;
using System.Threading.Tasks;
using static System.Linq.Enumerable;
using static System.Console;
using System.Collections.Generic;

namespace AsyncExt
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Func<int, IEnumerable<Task<Message>>> nextBatch = n => GetMessages2()
                                        .Take(n)
                                        .Select(MessageHandler)
                                        .ToList();
            await GetMessages()
                .Select(MessageHandler)
                .ToList() // run all
                .Batch(batchTimeout: 3000,
                       onSuccess: AcknowledgeAll,
                       onFail: RejectAll,
                       nextBatch: nextBatch);

            ReadKey();
        }

        static Task RejectAll(IEnumerable<Message> messages)
        {
            WriteLine($"Reject: {string.Join(",", messages.Select(x => x.Id))}");
            return Task.CompletedTask;
        }

        static Task AcknowledgeAll(IEnumerable<Message> messages)
        {
            WriteLine($"Acknowledge: {string.Join(",", messages.Select(x => x.Id))}");
            return Task.CompletedTask;
        }

        static Task<Message> MessageHandler(Message msg)
        {
            WriteLine("Running handling...");
            return Task.Run(async () =>
            {
                WriteLine($"{msg.Id} - it is being handled ...");
                await Task.Delay(msg.Delay);
                return msg;
            });
        }

        static IEnumerable<Message> GetMessages()
        {
            yield return new Message { Id = "1", Delay = 1000 };
            yield return new Message { Id = "2", Delay = 2000 };
            yield return new Message { Id = "3", Delay = 1000 };
            yield return new Message { Id = "4", Delay = 8000 };
            yield return new Message { Id = "5", Delay = 3500 };
            yield return new Message { Id = "6", Delay = 1000 };
            yield return new Message { Id = "7", Delay = 4000 };
            yield return new Message { Id = "8", Delay = 8000 };
            yield return new Message { Id = "9", Delay = 1000 };
            yield return new Message { Id = "10", Delay = 3200 };
            yield return new Message { Id = "11", Delay = 1000 };
        }

        static IEnumerable<Message> GetMessages2()
        {
            yield return new Message { Id = "21", Delay = 100 };
            yield return new Message { Id = "22", Delay = 200 };
            yield return new Message { Id = "23", Delay = 100 };
            yield return new Message { Id = "24", Delay = 800 };
            yield return new Message { Id = "25", Delay = 3500 };
            yield return new Message { Id = "26", Delay = 100 };
            yield return new Message { Id = "27", Delay = 4000 };
            yield return new Message { Id = "28", Delay = 800 };
            yield return new Message { Id = "29", Delay = 1000 };
            yield return new Message { Id = "210", Delay = 3200 };
            yield return new Message { Id = "211", Delay = 100 };
        }
    }

    public class Message
    {
        public string Id { get; set; }
        public int Delay { get; set; }
    }

    public static class EnumerableExt
    {
        public static async Task Batch(this IEnumerable<Task<Message>> self,
            int batchTimeout,
            Func<IEnumerable<Message>, Task> onSuccess,
            Func<IEnumerable<Message>, Task> onFail,
            Func<int, IEnumerable<Task<Message>>> nextBatch)
        {
            var handledTasks = new List<Task>();
            await Execute(self);
            await Task.WhenAll(handledTasks);

            async Task Execute(IEnumerable<Task<Message>> items)
            {
                var batchTimeoutTask = Task.Delay(batchTimeout);
                var batchTasks = Task.WhenAll(items);

                var completedTask = await Task.WhenAny(batchTasks, batchTimeoutTask);

                var completedSuccessfully = items.Where(x => x.IsCompletedSuccessfully).ToList();
                if (completedSuccessfully.Any())
                    handledTasks.Add(onSuccess(completedSuccessfully.Select(x => x.Result)));

                var faulted = items.Where(x => x.IsFaulted).ToList();
                if (faulted.Any())
                    handledTasks.Add(onFail(faulted.Select(x => x.Result)));

                if (completedTask == batchTimeoutTask)
                {
                    var notFinished = items
                        .Except(completedSuccessfully.Concat(faulted))
                        .ToList();

                    var batchSize = items.Count() - notFinished.Count;

                    await Execute(notFinished.Concat(nextBatch(batchSize)));
                }
            }
        }
    }
}

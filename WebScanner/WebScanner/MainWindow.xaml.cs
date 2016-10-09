using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using static System.Diagnostics.Debug;


namespace WebScanner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //DataContext = this;
            lbLinks.ItemsSource = LinkItems;
        }

        ObservableCollection<LinkItem> _linkItems;
        internal ObservableCollection<LinkItem> LinkItems
        {
            get
            {
                if (_linkItems == null)
                    _linkItems = new ObservableCollection<LinkItem>();
                return _linkItems;
            }
        }

        Scanner _scanner;
        private async void btnScann_Click(object sender, RoutedEventArgs e)
        {
            LinkItems.Clear();

            _scanner = new Scanner();

            _scanner.OnUrlFound += node =>
                LinkItems.Add(new LinkItem { Url = node.Url.AbsoluteUri, State = node.State });

            _scanner.OnNodeStateChanged += node =>
            {
                var changedItemIndex = -1;
                LinkItems.Where((n, i) => {
                    var found = n.Url == node.Url.AbsoluteUri;
                    if (found) changedItemIndex = i;
                    return found;
                }).FirstOrDefault();
                
                if (changedItemIndex >= 0)
                {
                    LinkItems.RemoveAt(changedItemIndex);
                    LinkItems.Insert(changedItemIndex, new LinkItem { Url = node.Url.AbsoluteUri, State = node.State });
                }
            };
            
            _scanner.OnCanceled += () => 
                MessageBox.Show("Operation was canceled by user!");

            WriteLine($"mainwindow threadId: {Thread.CurrentThread.ManagedThreadId}");

            Uri uri;
            if (Uri.TryCreate(tbUrl.Text, UriKind.Absolute, out uri))
                await _scanner.Scann(uri,
                   "fack",
                   2,
                   100);
            else
                MessageBox.Show("Input Url isn't correct.");

            //this.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
            //    (ThreadStart)delegate ()
            //    {
            //        LinkItems.Add(new LinkItem { Url = node.Url, State = node.State });
            //    });
        }

        private void btnStopScann_Click(object sender, RoutedEventArgs e)
        {
            _scanner?.Stop();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {            
            //LinkItems.First().Url = "xxx";
        }
    }

    class LinkItem
    {
        public LinkItem() { }

        public LinkItem(string url, NodeState state)
        {
            Url = url;
            State = state;
        }

        public string Url { get; set; }
        public NodeState State { get; set; }
    }
}

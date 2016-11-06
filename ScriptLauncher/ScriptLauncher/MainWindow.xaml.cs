using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows;

namespace ScriptLauncher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        //const string connectionString = @"Data Source=ASU-SERVER;Initial Catalog=upmtest;Persist Security Info=True;User ID=sa;Password=Abudfv1";
        const string connectionString = @"Data Source=./;Initial Catalog=SalesLogix;Integrated Security=True";
        //const string connectionString = @"Data Source=./;Initial Catalog=testDb;Integrated Security=True";
        private async void btnLaunch_Click(object sender, RoutedEventArgs e)
        {
            tbConsole.Text = string.Empty;

            var directoryNames = Directory.GetDirectories(tbPath.Text)
                                          .Where(d => !d.Contains("exclude"))
                                          .Select(d => new DirectoryInfo(d))
                                          .ToArray();

            var sortedDirectoryNames = Sort(directoryNames);
            
            btnLaunch.IsEnabled = false;
            await RunPatch(sortedDirectoryNames);
            btnLaunch.IsEnabled = true;
        }

        private Task RunPatch(string[] sortedDirectoryNames)
        {
            return Task.Run(() =>
            {
                TransactionScope transaction = null;
                try
                {
                    transaction = new TransactionScope();

                    foreach (string name in sortedDirectoryNames)
                    {
                        var files = Directory.GetFiles(name)
                                             .Where(f => !f.Contains("exclude"))
                                             .Select(f => new FileInfo(f));

                        var sortedFileNames = Sort(files);

                        foreach (var scriptFile in sortedFileNames)
                        {
                            RunScript(scriptFile);
                            Dispatcher.Invoke(() => tbConsole.Text += string.Format("{0} has complited\n", scriptFile));
                        }
                    }

                    transaction.Complete();
                    Dispatcher.Invoke(() => tbConsole.Text += "Success!");
                }
                catch (Exception ex)
                {
                    Dispatcher.Invoke(() => tbConsole.Text += ex.Message);
                }
                finally
                {
                    if (transaction != null) transaction.Dispose();
                }
            });
        }
        private void RunScript(string scriptFile)
        {
            SqlConnection connection = null;
            try
            {
                string script = File.ReadAllText(scriptFile, Encoding.Default);

                connection = new SqlConnection(connectionString);
                SqlCommand cmd = new SqlCommand(script, connection);

                connection.Open();

                cmd.ExecuteNonQuery();
            }
            catch (Exception innerEx)
            {
                Dispatcher.Invoke(() => tbConsole.Text = string.Format("File name: {0}{1}", scriptFile, Environment.NewLine));
                throw;
            }
            finally
            {
                if (connection != null) connection.Dispose();
            }
        }
        
        private string[] Sort(IEnumerable<FileInfo> files)
        {
            var toSort = files.Select(fi =>
            {
                int n = 0;

                if (int.TryParse(fi.Name.Split('.')[0], out n))
                {
                    return new
                    {
                        Number = n,
                        FileName = fi.FullName
                    };
                }
                else
                {
                    throw new Exception(string.Format("File name '{0}' has incorrect format", fi.FullName));
                }
            });

            return toSort.OrderBy(f => f.Number)
                         .Select(f => f.FileName)
                         .ToArray();
        }
        private string[] Sort(IEnumerable<DirectoryInfo> directories)
        {
            var toSort = directories.Select(di =>
            {
                int n = 0;

                if (int.TryParse(di.Name.Split('.')[0], out n))
                {
                    return new
                    {
                        Number = n,
                        DirectoryName = di.FullName
                    };
                }
                else
                {
                    throw new Exception(string.Format("Directory name '{0}' has incorrect format", di.FullName));
                }
            });

            return toSort.OrderBy(f => f.Number)
                         .Select(f => f.DirectoryName)
                         .ToArray();
        }
    }
}

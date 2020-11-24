using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace Word_count
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

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            txtEditor.Text = string.Empty;
            Progress<double> progress = new Progress<double>();
            progress.ProgressChanged += Progress_ProgressChanged;
            Progress_ProgressChanged(this, 0);
            OpenFileDialog openFileDialog = new OpenFileDialog();
            string subtitle = "";
            if (openFileDialog.ShowDialog() == true)
                subtitle = File.ReadAllText(openFileDialog.FileName);

            Regex reg = new Regex(@"(\b[a-zA-Z]{2,}\b)");

            string[] result = ConvertToList(reg.Matches(subtitle));

            HashSet<string> vs = new HashSet<string>(result, StringComparer.OrdinalIgnoreCase);

            await PrintMostFrequentlyWordUsedInThisMovie(result, progress);

            txtEditor.Text += $"in this season there is {result.Count()} word (include repeated), and {vs.Count} words not repeated";
        }

        private async void Progress_ProgressChanged(object sender, double e)
        {
            progressBar.Value = e;
        }

        private string[] ConvertToList(MatchCollection matchCollection)
        {
            List<string> vs = new List<string>();
            foreach (var item in matchCollection)
            {
                vs.Add(item.ToString());
            }
            return vs.ToArray();
        }

        private async Task PrintMostFrequentlyWordUsedInThisMovie(IEnumerable<string> list, IProgress<double> progress)
        {
            txtEditor.Text = "most frequent word used in this movie sorted by used count:" + Environment.NewLine;
            var result = from x in list
                         group x by x.ToString().ToLower();

            Dictionary<string, string> d = new Dictionary<string, string>();

            result = result.OrderByDescending(x => x.Count());

            int count = 0;
            foreach (var item in result)
            {
                txtEditor.Text += $"{item.Key,-20}is used {item.Count(),-3} time(s){Environment.NewLine}";
                txtEditor.Text += $"_____________________________________________________________{Environment.NewLine}";
                progress.Report(++count * 100 / result.Count());
            }
        }
    }
}

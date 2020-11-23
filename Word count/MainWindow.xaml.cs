using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            string subtitle = "";
            if (openFileDialog.ShowDialog() == true)
                subtitle = File.ReadAllText(openFileDialog.FileName);

            Regex reg = new Regex(@"(\b[a-zA-Z]{2,}\b)");

            string[] result = ConvertToList(reg.Matches(subtitle));

            HashSet<string> vs = new HashSet<string>(result, StringComparer.OrdinalIgnoreCase);

            PrintMostFrequentlyWordUsedInThisMovie(result);

            txtEditor.Text += $"in this season there is {result.Count()} word (include repeated), and {vs.Count} words not repeated";
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

        private void PrintMostFrequentlyWordUsedInThisMovie(IEnumerable<string> list)
        {
            txtEditor.Text = "most frequent word used in this movie sorted by used count:" + Environment.NewLine;
            var result = from x in list
                         group x by x.ToString().ToLower();

            result = result.OrderByDescending(x => x.Count());

            foreach (var item in result)
            {
                txtEditor.Text += $"{item.Key,-20}is used {item.Count(),3} time(s){Environment.NewLine}";
                txtEditor.Text += $"_____________________________________________________________{Environment.NewLine}";
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;

namespace TemperatureMonitoringWPF
{
    /// <summary>
    /// Interaction logic for ReportWindow.xaml
    /// </summary>
    public partial class ReportWindow : Window
    {
        int maxTemp;
        int? minTemp;

        public ReportWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var str = Buffer.Text.Split('\n');

            maxTemp = int.Parse(str[1]);

            if (str[0].Equals("2"))
            {
                minTemp = int.Parse(str[2]);
            }

            for (int i = int.Parse(str[0]) + 1; i < str.Length - 1; i++)
            {
                string[] str2 = str[i].Split('\t');

                Report.Text += str2[0] + "\t" + str2[1] + "\t" + str2[2] + "\t" + str2[3] + "\n";
            }

            string[] excess = BufferInt.Text.Split("\n");
            if(!excess[0].Equals("0"))
            {
                int time = int.Parse(excess[0]);
                ReportMessage.Text += $"Порог максимально допустимой температуры превышен на {time / 60} часа " +
                    $"{time - (time / 60 * 60)} минут";
            }
            else if(!excess[1].Equals("0"))
            {
                int time = int.Parse(excess[1]);
                ReportMessage.Text += $"Порог минимально допустимой температуры превышен на {time / 60} " +
                    $"часа {time - (time / 60 * 60)} минут";
            }
        }

        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    var saveFile = new Microsoft.Win32.SaveFileDialog();
            
        //    if(saveFile.ShowDialog() == true)
        //    {
        //        StreamWriter sw = new StreamWriter(saveFile.FileName);
        //        sw.Write("Hello World!!");
        //        sw.Close();
        //    }
        //}
    }
}

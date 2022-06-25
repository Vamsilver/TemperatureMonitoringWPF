
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace TemperatureMonitoringWPF
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

        private string ParseDateWithMin()
        {
            int maxTempInt = int.Parse(maxTemp.Text.ToString());
            int maxTimeInt = int.Parse(maxTime.Text.ToString());
            
            int minTempInt = int.Parse(minTemp.Text.ToString());
            int minTimeInt = int.Parse(minTime.Text.ToString());

            DateTime date = DateTime.Parse(Date.Text.ToString());

            string[] vs = Temperature.Text.ToString().Split(' ');
            int[] temp = new int[vs.Length];

            string str = "";

            for (int i = 0; i < vs.Length; i++)
            {
                temp[i] = int.Parse(vs[i].ToString());
            }

            str += 2 + "\n";
            str += maxTempInt + "\n";
            str += minTempInt + "\n";

            foreach (var t in temp)
            {
                date = date.AddMinutes(10);
                if(t > maxTempInt || t < minTempInt)
                {
                    int standartTemp = (t >= maxTimeInt) ? maxTempInt : minTempInt;
                    str += date.ToString() + "\t" + t + "\t" + standartTemp + "\t" + (standartTemp - t) + "\n";
                }
            }

            return str;
        }

        private string ParseDateWithoutMin()
        {
            int maxTempInt = int.Parse(maxTemp.Text.ToString());
            int maxTimeInt = int.Parse(maxTime.Text.ToString());

            var date = DateTime.Parse(Date.Text.ToString());

            string[] vs = Temperature.Text.ToString().Split(' ');
            int[] temp = new int[vs.Length];

            string str = "";

            for (int i = 0; i < vs.Length; i++)
            {
                temp[i] = int.Parse(vs[i].ToString());
            }

            str += 1 + "\n" + maxTempInt + "\n";

            foreach (var t in temp)
            {
                date = date.AddMinutes(10);
                if (t > maxTempInt)
                {
                    str += date.ToString() + "\t" + t + "\t" + maxTempInt + "\t" + (maxTempInt - t) + "\n";
                }
            }

            return str;
        }

        private int[] IdentifyExcess(string[] str)
        {
            int[] answer = new int[2] { 0, 0 };

            for (int i = int.Parse(str[0]) + 1; i < str.Length - 1; i++)
            {
                string[] str2 = str[i].Split('\t');

                if ((str2[2] + "\r").Equals(maxTemp.Text))
                    answer[0] += 10;
                else if((str2[2] + "\r").Equals(minTemp.Text))
                    answer[1] += 10;
            }

            return answer;
        }

        private void CreateReport_Click(object sender, RoutedEventArgs e)
        {
            if (Date.Text.Equals("") || Temperature.Text.Equals("") ||
                maxTemp.Text.Equals("" )|| maxTime.Text.Equals(""))
            {
                MessageBox.Show("Введите все необходимые данные", "Отсутствие данных");
            }
            else
            {
                ReportWindow report = new ReportWindow();
                report.Owner = this;
                if (!minTemp.Text.Equals("") && !minTime.Text.Equals(""))
                {
                    string str = ParseDateWithMin();
                    report.Buffer.Text = str;
                    int[] vs = IdentifyExcess(str.Split('\n'));
                    report.BufferInt.Text += vs[0].ToString() + "\n";
                    report.BufferInt.Text += vs[1].ToString();
                }
                else
                {
                    string str = ParseDateWithoutMin();
                    report.Buffer.Text = str;
                    int[] vs = IdentifyExcess(str.Split('\n'));
                    report.BufferInt.Text += vs[0].ToString() + "\n";
                    report.BufferInt.Text += vs[1].ToString();
                }
                    
                report.Show();
            }
        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            if (fileDialog.ShowDialog() == true)
            {
                ParseFileText(File.ReadAllText(fileDialog.FileName));
            }
        }

        private void ParseFileText(string str)
        {
            string[] text = str.Split('\n');
            if(text.Length > 3)
            {
                foreach(var s in text)
                { 
                    FishName.Text = text[0];
                    maxTemp.Text = text[1];
                    maxTime.Text = text[2];

                    if (text[3].Equals(".\r"))
                    {
                        minTemp.Text = "";
                        minTime.Text = "";
                    }
                    else
                    {
                        minTemp.Text = text[3];
                        minTime.Text = text[4];
                    }
                    Date.Text = text[5];
                    Temperature.Text = text[6];
                }
            }
            else
            {
                Date.Text = text[0];
                Temperature.Text = text[1];
            }

        }
    }
}

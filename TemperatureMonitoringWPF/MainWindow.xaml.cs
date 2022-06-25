
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

        private string ParseDate()
        {
            int maxTempInt = int.Parse(maxTemp.Text.ToString());
            int minTempInt = int.Parse(minTemp.Text.ToString());
            int maxTimeInt = int.Parse(maxTime.Text.ToString());
            int minTimeInt = int.Parse(minTime.Text.ToString());
            var date = DateTime.Parse(Date.Text.ToString());

            string[] vs = Temperature.Text.ToString().Split(' ');
            int[] temp = new int[vs.Length];

            string str = "";

            for (int i = 0; i < vs.Length; i++)
            {
                temp[i] = int.Parse(vs[i].ToString());
            }

            foreach(var t in temp)
            {
                date.AddMinutes(10);
                if(t > maxTempInt || t < minTempInt)
                {
                    int standartTemp = ((t >= maxTimeInt) ? maxTempInt : minTimeInt);
                    str += date.ToString() + "\t" + t + "\t" + standartTemp + "\t" + (standartTemp - t) + "\n";
                }
            }

            MessageBox.Show(str);
            return str;
        }

        private void CreateReport_Click(object sender, RoutedEventArgs e)
        {
            if (Date.Text.Equals("") || Temperature.Text.Equals("") || maxTemp.Text.Equals("")
                    || maxTime.Text.Equals("") || minTemp.Text.Equals("") || minTime.Text.Equals(""))
            {
                MessageBox.Show("Введите все необходимые данные", "Отсутствие данных");
            }
            else
            {
                ReportWindow report = new ReportWindow();
                report.Owner = this;
                report.Buffer.Text = ParseDate();
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
                    minTemp.Text = text[3];
                    minTime.Text = text[4];
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

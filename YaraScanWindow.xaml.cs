using System;
using System.ComponentModel;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace VolatilityGUI
{
    /// <summary>
    /// Interaction logic for YaraScanWindow.xaml
    /// </summary>
    public partial class YaraScanWindow : Window
    {
        private static string imageRam;
        private static string imageRamProfile;
        private string output;
        private string outputYara;
        private string rules;
        private string pid;
        public YaraScanWindow()
        {
            InitializeComponent();
            CentralWindow.historyListCons.Add(DateTime.UtcNow.ToString("HH:mm") + " | YaraScan Window Opened");
            imageRam = CentralWindow.imageRamCons;
            imageRamProfile = CentralWindow.imageRamProfileCons;
            pbStatus.Visibility = Visibility.Hidden;
            RunCommand();
        }

        private void btnRun_Click(object sender, RoutedEventArgs e)
        {
            CentralWindow.historyListCons.Add(DateTime.UtcNow.ToString("HH:mm") + " | YaraScan Runed");
            pid = null;
            rules = lbl_About.Text;
            if (ComboBoxY.SelectedItem.ToString().Contains("Entire System"))
            {
                pid = null;
                RunCommandYara();
            }
            else
            {
                //pid = ComboBoxY.SelectedItem.ToString();
                pid = Regex.Match(ComboBoxY.SelectedItem.ToString(), @"\d+").Value;
                RunCommandYara();
            }
        }

        private void lbl_About_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (lbl_About.Text == null || lbl_About.Text == "")
            {
                btnRun.IsEnabled = false;
            }
            else
            {
                btnRun.IsEnabled = true;
            }
        }

        void RunCommand()
        {
            BackgroundWorker bg = new BackgroundWorker();
            pbStatus.Visibility = Visibility.Visible;
            btnRun.IsEnabled = false;
            lbl_About.IsReadOnly = true;
            bg.DoWork += new DoWorkEventHandler(MethodToGetInfo);
            bg.RunWorkerCompleted += new RunWorkerCompletedEventHandler(Bg_RunWorkerCompleted);
            //show marquee here
            bg.RunWorkerAsync();
        }

        void MethodToGetInfo(Object sender, DoWorkEventArgs args)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.FileName = "CMD.exe";
            startInfo.WorkingDirectory = Directory.GetCurrentDirectory();
            startInfo.Arguments = "/c volatility.exe -f " + "\"" + imageRam + "\"" + " --profile=" + imageRamProfile + " pslist";
            startInfo.CreateNoWindow = true;
            process.StartInfo = startInfo;
            process.Start();
            output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
        }

        void Bg_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs args)
        {
            //this method will be called once background worker has completed it's task
            //hide the marquee
            pbStatus.Visibility = Visibility.Hidden;
            //btnRun.IsEnabled = true;
            lbl_About.IsReadOnly = false;
            Regex exp = new Regex("^0x(.*)", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            MatchCollection matchCollection = exp.Matches(output);
            foreach (Match m in matchCollection)
            {
                RegexOptions options = RegexOptions.None;
                Regex regex = new Regex("[ ]{2,}", options);
                string aux2 = regex.Replace(m.ToString(), " ");
                string[] auxSplit = aux2.Split(' ');
                ComboBoxY.Items.Add(auxSplit[1] + " - " + auxSplit[2]);
            }
        }

        void RunCommandYara()
        {
            BackgroundWorker bg = new BackgroundWorker();
            pbStatus.Visibility = Visibility.Visible;
            btnRun.IsEnabled = false;
            lbl_About.IsReadOnly = true;
            bg.DoWork += new DoWorkEventHandler(MethodToGetInfoYara);
            bg.RunWorkerCompleted += new RunWorkerCompletedEventHandler(Bg_RunWorkerCompletedYara);
            //show marquee here
            bg.RunWorkerAsync();
        }

        void MethodToGetInfoYara(Object sender, DoWorkEventArgs args)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.FileName = "CMD.exe";
            startInfo.WorkingDirectory = Directory.GetCurrentDirectory();
            if (pid == null)
            {
                startInfo.Arguments = "/c volatility.exe -f " + "\"" + imageRam + "\"" + " --profile=" + imageRamProfile + " yarascan --yara-rules=\"" + rules + "\"";
            }
            else
            {
                startInfo.Arguments = "/c volatility.exe -f " + "\"" + imageRam + "\"" + " --profile=" + imageRamProfile + " yarascan --yara-rules=\"" + rules + "\" --pid=" + pid;
            }
            startInfo.CreateNoWindow = true;
            process.StartInfo = startInfo;
            process.Start();
            outputYara = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
        }

        void Bg_RunWorkerCompletedYara(object sender, RunWorkerCompletedEventArgs args)
        {
            //this method will be called once background worker has completed it's task
            //hide the marquee
            pbStatus.Visibility = Visibility.Hidden;
            btnRun.IsEnabled = true;
            lbl_About.IsReadOnly = false;
            lbl_output.Text = outputYara;
        }
    }
}

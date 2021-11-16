using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.ComponentModel;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;

namespace VolatilityGUI
{
    /// <summary>
    /// Interaction logic for MEMWindow.xaml
    /// </summary>
    public partial class MEMWindow : Window
    {
        private static string imageRam;
        private static string imageRamProfile;
        private string output;
        private string outputError;
        private string folder;
        private string pid;
        public MEMWindow()
        {
            InitializeComponent();
            CentralWindow.historyListCons.Add(DateTime.UtcNow.ToString("HH:mm") + " | About Memory Window Opened");
            imageRam = CentralWindow.imageRamCons;
            imageRamProfile = CentralWindow.imageRamProfileCons;
            pbStatus.Visibility = Visibility.Hidden;

            if (imageRam == null)
            {
                MessageBox.Show("No RAM Image Selected");
            }
            else if (CentralWindow.pidListCons.Count == 0)
            {

                RunCommand();
            }
            else
            {
                ComboBoxPIDMEM.ItemsSource = CentralWindow.pidListCons;
            }
        }
        void RunCommand()
        {
            BackgroundWorker bg = new BackgroundWorker();
            pbStatus.Visibility = Visibility.Visible;
            RunSelectMEM.IsEnabled = false;
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
            RunSelectMEM.IsEnabled = true;
            Regex exp = new Regex("^0x(.*)", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            MatchCollection matchCollection = exp.Matches(output);
            foreach (Match m in matchCollection)
            {
                RegexOptions options = RegexOptions.None;
                Regex regex = new Regex("[ ]{2,}", options);
                string aux2 = regex.Replace(m.ToString(), " ");
                string[] auxSplit = aux2.Split(' ');
                CentralWindow.pidListCons.Add(auxSplit[1] + " - " + auxSplit[2]);
                //ComboBoxPIDMEM.Items.Add(auxSplit[1] + " - " + auxSplit[2]);
            }
            ComboBoxPIDMEM.ItemsSource = CentralWindow.pidListCons;
        }

        private void RunSelectMEM_Click(object sender, RoutedEventArgs e)
        {
            if (ComboBoxPIDMEM.SelectedItem != null)
            {
                pid = Regex.Match(ComboBoxPIDMEM.SelectedItem.ToString(), @"\d+").Value;
                var dlg = new CommonOpenFileDialog();
                dlg.Title = "Choose Download Folder";
                dlg.IsFolderPicker = true;
                dlg.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
                dlg.AllowNonFileSystemItems = false;
                dlg.DefaultDirectory = AppDomain.CurrentDomain.BaseDirectory;
                dlg.EnsureFileExists = true;
                dlg.EnsurePathExists = true;
                dlg.EnsureReadOnly = false;
                dlg.EnsureValidNames = true;
                dlg.Multiselect = false;
                dlg.ShowPlacesList = true;

                if (dlg.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    folder = dlg.FileName;
                    MessageBox.Show(folder);
                    CentralWindow.historyListCons.Add(DateTime.UtcNow.ToString("HH:mm") + " | Memory from pid " + pid + " downloaded to folder " + folder);
                    App.Current.Windows[1].Activate();
                    App.Current.Windows[2].Activate();
                    runCommandPID();
                }
                else
                {
                    MessageBox.Show("No valid path select");
                }
            }
            else
            {
                MessageBox.Show("No PID Selected");
            }
        }

        void runCommandPID()
        {

            BackgroundWorker bg = new BackgroundWorker();
            pbStatus.Visibility = Visibility.Visible;
            RunSelectMEM.IsEnabled = false;
            bg.DoWork += new DoWorkEventHandler(MethodToGetInfo3);
            bg.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bg_RunWorkerCompleted3);
            //show marquee here
            bg.RunWorkerAsync();

        }

        private void MethodToGetInfo3(Object sender, DoWorkEventArgs args)
        {
            System.Diagnostics.Process process2 = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo3 = new System.Diagnostics.ProcessStartInfo();
            startInfo3.UseShellExecute = false;
            startInfo3.RedirectStandardOutput = true;
            startInfo3.FileName = "CMD.exe";
            startInfo3.WorkingDirectory = Directory.GetCurrentDirectory();
            startInfo3.Arguments = "/c volatility.exe -f " + "\"" + imageRam + "\"" + " --profile=" + imageRamProfile + " memdump --dump-dir=" + "\"" + folder + "\"" + " -p " + pid;
            startInfo3.CreateNoWindow = true;
            process2.StartInfo = startInfo3;
            process2.Start();
            outputError = process2.StandardOutput.ReadToEnd();
            process2.WaitForExit();
        }

        void bg_RunWorkerCompleted3(object sender, RunWorkerCompletedEventArgs args)
        {
            //this method will be called once background worker has completed it's task
            if (outputError.Contains("Error"))
                MessageBox.Show(outputError);

            //hide the marquee

            pbStatus.Visibility = Visibility.Hidden;
            RunSelectMEM.IsEnabled = true;
            MessageBox.Show("Selected Memory Pages from PID: " + pid + " downloaded to " + folder);
        }
    }
}

using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace VolatilityGUI
{
    /// <summary>
    /// Interaction logic for DLLWindow.xaml
    /// </summary>
    public partial class DLLWindow : Window
    {
        private static string imageRam;
        private static string imageRamProfile;
        private string output;
        private string outputError;
        private string folder;
        private string offset;
        private string pid;
        private string pid2;
        private float numberOfPages;
        private int countAUX;
        List<string> outputAUX = new List<string>();
        List<DLL> dllList = new List<DLL>();
        List<DLL> SelectedList = new List<DLL>();

        public DLLWindow()
        {
            InitializeComponent();
            CentralWindow.historyListCons.Add(DateTime.UtcNow.ToString("HH:mm") + " | About DLL Download Window Opened");
            imageRam = CentralWindow.imageRamCons;
            imageRamProfile = CentralWindow.imageRamProfileCons;

            if (imageRam == null)
            {
                MessageBox.Show("No RAM Image Selected");
            }
            else
            {
                RunCommand();
            }
        }

        void RunCommand()
        {
            BackgroundWorker bg = new BackgroundWorker();
            pbStatus.Visibility = Visibility.Visible;
            RunSelectDLL.IsEnabled = false;
            FindPIDDLL.IsEnabled = false;
            DonwloadPIDDLL.IsEnabled = false;
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
            startInfo.Arguments = "/c volatility.exe -f " + "\"" + imageRam + "\"" + " --profile=" + imageRamProfile + " dlllist";
            startInfo.CreateNoWindow = true;
            process.StartInfo = startInfo;
            process.Start();
            output = process.StandardOutput.ReadToEnd();
            output += Environment.NewLine;
            output += "************************************************************************";

            process.WaitForExit();

        }

        void Bg_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs args)
        {
            //this method will be called once background worker has completed it's task
            //hide the marquee

            pbStatus.Visibility = Visibility.Hidden;
            RunSelectDLL.IsEnabled = true;
            FindPIDDLL.IsEnabled = true;
            DonwloadPIDDLL.IsEnabled = true;

            //agarrar PIDs
            //Regex exp2 = new Regex("pid:(.*)", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            Regex exp2 = new Regex(".*pid:.*", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            MatchCollection matchCollection2 = exp2.Matches(output);
            foreach (Match y in matchCollection2)
            {
                var x = Regex.Replace(y.Value, @"\s+", " ");
                ComboBoxPID.Items.Add(x.Split(' ')[0] + "  " + x.Split(' ')[1] + " " + x.Split(' ')[2]);
                //ComboBoxPID.Items.Add(y);
            }
        }


        private void RunSelectDLL_Click(object sender, RoutedEventArgs e)
        {
            CentralWindow.historyListCons.Add(DateTime.UtcNow.ToString("HH:mm") + " | Downloaded DLLs");
            //funciona tem haver com o tamanho da janela            
            if (dgDLL.Items.Count != 0)
            {

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
                    for (int i = 0; i < dgDLL.Items.Count; i++)
                    {
                        var item = dgDLL.Items[i];
                        var mycheckbox = dgDLL.Columns[5].GetCellContent(item) as CheckBox;
                        if ((bool)mycheckbox.IsChecked)
                        {
                            SelectedList.Add(dllList[i]);
                        }
                    }
                    if (SelectedList.Count == 0)
                    {
                        MessageBox.Show("No DLLs were selected");
                    }
                    else
                    {
                        countAUX = 0;
                        offset = SelectedList[0].Base;
                        App.Current.Windows[1].Activate();
                        App.Current.Windows[2].Activate();
                        runCommandSellected();

                    }
                }
                else
                {
                    MessageBox.Show("No valid path select");
                }

            }
            else
            {
                MessageBox.Show("No values on DLL List");
            }


        }

        private void FindPIDDLL_Click(object sender, RoutedEventArgs e)
        {
            //limpar a lista?            
            if (ComboBoxPID.SelectedItem != null)
            {
                string auxII = (string)ComboBoxPID.SelectedItem.ToString();
                pid = Regex.Match(auxII, @"\d+").Value;
                dllList.Clear();
                outputAUX.Clear();
                string auxIII = BetweenStringsWithInputEspecial(auxII, "************************************************************************", output);

                Regex exp = new Regex("^0x(.*)", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                MatchCollection matchCollection = exp.Matches(auxIII);

                foreach (Match m in matchCollection)
                {
                    outputAUX.Add(m.Value);
                }
                if (outputAUX.Count == 0)
                {
                    MessageBox.Show("No DLLs associated to this PID");
                }
                else
                {
                    if (outputAUX.Count > 10)
                    {
                        Right.Visibility = Visibility.Visible;
                        Pages.Visibility = Visibility.Visible;
                        Left.Visibility = Visibility.Visible;

                        numberOfPages = (float)decimal.Divide(outputAUX.Count, 10);
                        numberOfPages = (float)Math.Ceiling(numberOfPages);
                        Pages.Content = "1 of " + numberOfPages;


                        int count = 1;
                        //foreach string da lista separar aquilo por cenas
                        foreach (string aux in outputAUX)
                        {
                            RegexOptions options = RegexOptions.None;
                            Regex regex = new Regex("[ ]{2,}", options);
                            string aux2 = regex.Replace(aux, " ");
                            string[] auxSplit = aux2.Split(' ');
                            if (count < 11)
                            {
                                dllList.Add(new DLL() { IsChecked = false, Count = count, Base = auxSplit[0], Size = auxSplit[1], LoadCount = auxSplit[2], Path = auxSplit[3] });
                            }
                            count++;
                        }

                        dgDLL.ItemsSource = dllList;
                        dgDLL.Items.Refresh();

                    }
                    else
                    {
                        int count = 1;
                        //foreach string da lista separar aquilo por cenas
                        foreach (string aux in outputAUX)
                        {
                            RegexOptions options = RegexOptions.None;
                            Regex regex = new Regex("[ ]{2,}", options);
                            string aux2 = regex.Replace(aux, " ");
                            string[] auxSplit = aux2.Split(' ');
                            count++;
                        }

                        dgDLL.ItemsSource = dllList;
                        dgDLL.Items.Refresh();
                    }
                }
            }
            else
            {
                MessageBox.Show("No value selected from list");
            }

        }

        void runCommandSellected()
        {
            BackgroundWorker bg = new BackgroundWorker();
            pbStatus.Visibility = Visibility.Visible;
            RunSelectDLL.IsEnabled = false;
            DonwloadPIDDLL.IsEnabled = false;
            FindPIDDLL.IsEnabled = false;
            bg.DoWork += new DoWorkEventHandler(MethodToGetInfo2);
            bg.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bg_RunWorkerCompleted2);
            //show marquee here
            bg.RunWorkerAsync();
        }

        private void MethodToGetInfo2(Object sender, DoWorkEventArgs args)
        {
            System.Diagnostics.Process process2 = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo2 = new System.Diagnostics.ProcessStartInfo();
            startInfo2.UseShellExecute = false;
            startInfo2.RedirectStandardOutput = true;
            startInfo2.FileName = "CMD.exe";
            startInfo2.WorkingDirectory = Directory.GetCurrentDirectory();
            startInfo2.Arguments = "/c volatility.exe -f " + "\"" + imageRam + "\"" + " --profile=" + imageRamProfile + " dlldump --dump-dir " + "\"" + folder + "\"" + " -p " + pid + " -b " + offset;
            startInfo2.CreateNoWindow = true;
            process2.StartInfo = startInfo2;
            process2.Start();
            outputError = " " + process2.StandardOutput.ReadToEnd();
            process2.WaitForExit();
        }

        void bg_RunWorkerCompleted2(object sender, RunWorkerCompletedEventArgs args)
        {
            //this method will be called once background worker has completed it's task
            if (outputError.Contains("Error"))
            {
                MessageBox.Show("Unable to download PID: " + pid + " Offset " + offset + " : DllBase is paged");
                pbStatus.Visibility = Visibility.Hidden;
                RunSelectDLL.IsEnabled = true;
                FindPIDDLL.IsEnabled = true;
                DonwloadPIDDLL.IsEnabled = true;
            }
            else
            {
                //hide the marquee
                countAUX++;
                if (SelectedList.Count > countAUX)
                {
                    offset = SelectedList[countAUX].Base;
                    runCommandSellected();
                }
                else
                {
                    pbStatus.Visibility = Visibility.Hidden;
                    RunSelectDLL.IsEnabled = true;
                    FindPIDDLL.IsEnabled = true;
                    DonwloadPIDDLL.IsEnabled = true;



                    MessageBox.Show("All Selected DLLs downloaded to " + folder);
                }
            }
        }

        private string BetweenStringsWithInputEspecial(string before, string after, string stringAux)
        {
            int pFrom = output.IndexOf(before) + before.Length;
            string outputMinus = output.Substring(pFrom - before.Length);
            int pFrom2 = outputMinus.IndexOf(before) + before.Length;
            int pTo = outputMinus.IndexOf(after);
            string outputAUX = outputMinus.Substring(pFrom2, pTo - pFrom2);
            return outputAUX;
        }

        private void Right_Click(object sender, RoutedEventArgs e)
        {
            string pagesAUX = Pages.Content.ToString();
            string numberofPageTemp = pagesAUX.Substring(0, 1);
            int numberofPageNow = Int32.Parse(numberofPageTemp);

            if (pagesAUX.Contains(numberOfPages + " of " + numberOfPages))
            {
                // pode ser chato estar a mostrar esta Message Box
                //MessageBox.Show("Already on the last page");
            }
            else
            {
                Pages.Content = (numberofPageNow + 1) + " of " + numberOfPages;
                dllList.Clear();
                int count = 1;
                foreach (string aux in outputAUX)
                {
                    RegexOptions options = RegexOptions.None;
                    Regex regex = new Regex("[ ]{2,}", options);
                    string aux2 = regex.Replace(aux, " ");
                    string[] auxSplit = aux2.Split(' ');
                    if (count > numberofPageNow * 10 && count < numberofPageNow * 10 + 11)
                    {
                        dllList.Add(new DLL() { IsChecked = false, Count = count, Base = auxSplit[0], Size = auxSplit[1], LoadCount = auxSplit[2], Path = auxSplit[3] });
                    }
                    count++;
                }

                dgDLL.ItemsSource = dllList;
                dgDLL.Items.Refresh();
            }

        }

        private void Left_Click(object sender, RoutedEventArgs e)
        {
            string pagesAUX = Pages.Content.ToString();
            string numberofPageTemp = pagesAUX.Substring(0, 1);
            int numberofPageNow = Int32.Parse(numberofPageTemp);

            if (pagesAUX.Contains("1 of"))
            {
                // pode ser chato estar a mostrar esta Message Box
                //MessageBox.Show("Already on the first page");
            }
            else
            {
                Pages.Content = (numberofPageNow - 1) + " of " + numberOfPages;
                dllList.Clear();
                int count = 1;
                foreach (string aux in outputAUX)
                {
                    RegexOptions options = RegexOptions.None;
                    Regex regex = new Regex("[ ]{2,}", options);
                    string aux2 = regex.Replace(aux, " ");
                    string[] auxSplit = aux2.Split(' ');
                    if (numberofPageNow == 2)
                    {
                        if (count < numberofPageNow * 10 - 9)
                        {
                            dllList.Add(new DLL() { IsChecked = false, Count = count, Base = auxSplit[0], Size = auxSplit[1], LoadCount = auxSplit[2], Path = auxSplit[3] });
                        }
                    }
                    else
                    {
                        if (count > numberofPageNow * 10 - 20 && count < numberofPageNow * 10 - 9)
                        {
                            dllList.Add(new DLL() { IsChecked = false, Count = count, Base = auxSplit[0], Size = auxSplit[1], LoadCount = auxSplit[2], Path = auxSplit[3] });
                        }
                    }

                    count++;
                }

                dgDLL.ItemsSource = dllList;
                dgDLL.Items.Refresh();
            }
        }

        private void DonwloadPIDDLL_Click(object sender, RoutedEventArgs e)
        {

            if (ComboBoxPID.SelectedItem != null)
            {
                string pidAux = (string)ComboBoxPID.SelectedItem.ToString();
                pid2 = Regex.Match(pidAux, @"\d+").Value;
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
                    CentralWindow.historyListCons.Add(DateTime.UtcNow.ToString("HH:mm") + " | Downloaded all DLLs from " + pid2 + " to the folder " + folder);
                    runCommandPID();
                }
                else
                {
                    MessageBox.Show("No valid path select");
                }
            }
            else
            {
                MessageBox.Show("No PID selected");
            }
        }

        void runCommandPID()
        {
            BackgroundWorker bg = new BackgroundWorker();
            pbStatus.Visibility = Visibility.Visible;
            RunSelectDLL.IsEnabled = false;
            DonwloadPIDDLL.IsEnabled = false;
            FindPIDDLL.IsEnabled = false;
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

            startInfo3.Arguments = "/c volatility.exe -f " + "\"" + imageRam + "\"" + " --profile=" + imageRamProfile + " dlldump --dump-dir " + folder + " -p " + pid2;

            startInfo3.CreateNoWindow = true;
            process2.StartInfo = startInfo3;
            process2.Start();
            outputError = process2.StandardOutput.ReadToEnd();
            process2.WaitForExit();
        }

        void bg_RunWorkerCompleted3(object sender, RunWorkerCompletedEventArgs args)
        {
            //this method will be called once background worker has completed it's task
            Regex exp = new Regex(".*Error.*", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            MatchCollection matchCollection = exp.Matches(outputError);
            foreach (Match m in matchCollection)
            {
                string auxShow = m.Value;
                RegexOptions options = RegexOptions.None;
                Regex regex = new Regex("[ ]{2,}", options);
                auxShow = regex.Replace(auxShow, " ");
                auxShow.Split(' ');
                MessageBox.Show("Unable to download DLL offset " + auxShow.Split(' ')[2] + " - Error: DllBase is paged");
            }
            //hide the marquee            
            pbStatus.Visibility = Visibility.Hidden;
            RunSelectDLL.IsEnabled = true;
            FindPIDDLL.IsEnabled = true;
            DonwloadPIDDLL.IsEnabled = true;
            MessageBox.Show("All PID: " + pid2 + " DLLs downloaded to " + folder);
        }
    }
}

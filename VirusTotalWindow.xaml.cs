using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Threading;

namespace VolatilityGUI
{
    /// <summary>
    /// Interaction logic for VirusTotalWindow.xaml
    /// </summary>
    public partial class VirusTotalWindow : Window
    {
        private static string imageRam;
        private static string imageRamProfile;
        private string output;
        private string outputdll;
        private bool toogleEXE;
        private bool toogleDLL;
        private float numberOfPages;
        private string folderAux;
        List<VirusT> fileList = new List<VirusT>();
        List<VirusT> fileListAuxEXE = new List<VirusT>();
        List<VirusT> fileListAuxDLL = new List<VirusT>();
        List<VirusT> SelectedList = new List<VirusT>();
        List<EXE> fileListEXE = new List<EXE>();
        List<EXE> fileListDLL = new List<EXE>();
        private string file;
        private int countPrivAPI;
        private int countPublicAPI;
        private int countPublicAPITotal;
        private int countPublicAPIInterval;
        System.Windows.Forms.Timer t = new System.Windows.Forms.Timer();
        DispatcherTimer timer = new DispatcherTimer();

        public VirusTotalWindow()
        {
            InitializeComponent();
            Right.Visibility = Visibility.Hidden;
            Pages.Visibility = Visibility.Hidden;
            Left.Visibility = Visibility.Hidden;
            KeyInfo.Visibility = Visibility.Hidden;
            FindFile.IsEnabled = false;
            CentralWindow.historyListCons.Add(DateTime.UtcNow.ToString("HH:mm") + " | About Virus Total API Window Opened");
            imageRam = CentralWindow.imageRamCons;
            imageRamProfile = CentralWindow.imageRamProfileCons;
            toogleEXE = false;
            toogleDLL = false;
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
            FindFile.IsEnabled = true;
            Regex exp2 = new Regex("pid:(.*)", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            MatchCollection matchCollection2 = exp2.Matches(output);
            foreach (Match y in matchCollection2)
            {
                ComboBoxY.Items.Add(y);
            }
        }

        private void DG_Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            Hyperlink link = (Hyperlink)e.OriginalSource;
            Process.Start(link.NavigateUri.AbsoluteUri);
        }

        private void FindFile_Click(object sender, RoutedEventArgs e)
        {
            if (ComboBoxP.SelectedItem.ToString().Contains("EXEs"))
            {
                folderAux = AppDomain.CurrentDomain.BaseDirectory + @"Saves\EXEs";
                if (toogleEXE == false)
                {
                    RunCommandEXE();
                    toogleEXE = true;
                }
                else
                {
                    fillListEXE();
                }
            }
            else if (ComboBoxP.SelectedItem.ToString().Contains("DLLs"))
            {
                folderAux = AppDomain.CurrentDomain.BaseDirectory + @"Saves\DLL";
                if (toogleDLL == false)
                {
                    RunCommandDLL();
                    toogleDLL = true;
                }
                else
                {
                    fillListDLL();
                }
            }
            else
            {
                MessageBox.Show("No value selected");
            }
        }

        void RunCommandEXE()
        {
            BackgroundWorker bg = new BackgroundWorker();
            System.IO.Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + @"Saves\EXEs\");
            pbStatus.Visibility = Visibility.Visible;
            FindFile.IsEnabled = false;
            Right.IsEnabled = false;
            Left.IsEnabled = false;
            bg.DoWork += new DoWorkEventHandler(MethodToGetInfoEXE);
            bg.RunWorkerCompleted += new RunWorkerCompletedEventHandler(Bg_RunWorkerCompletedEXE);
            //show marquee here
            bg.RunWorkerAsync();
        }

        void MethodToGetInfoEXE(Object sender, DoWorkEventArgs args)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.FileName = "CMD.exe";
            startInfo.WorkingDirectory = Directory.GetCurrentDirectory();
            startInfo.Arguments = "/c volatility.exe -f " + "\"" + imageRam + "\"" + " --profile=" + imageRamProfile + " procdump --dump-dir=" + "\"" + AppDomain.CurrentDomain.BaseDirectory + "Saves\\EXEs" + "\"";   //"'" + folderAux + "'";
            //MessageBox.Show(startInfo.Arguments);
            startInfo.CreateNoWindow = true;
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();
        }

        void Bg_RunWorkerCompletedEXE(object sender, RunWorkerCompletedEventArgs args)
        {
            //this method will be called once background worker has completed it's task
            //hide the marquee
            pbStatus.Visibility = Visibility.Hidden;
            FindFile.IsEnabled = true;
            Right.IsEnabled = true;
            Left.IsEnabled = true;
            fillListEXE();
        }

        private void fillListEXE()
        {
            fileList.Clear();
            fileListAuxEXE.Clear();

            // Create a DirectoryInfo object representing the specified directory.
            var dir = new DirectoryInfo(folderAux);
            // Get the FileInfo objects for every file in the directory.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                fileListEXE.Add(new EXE() { Name = file.Name, Hash = BytesToString(GetHashSha256(file.FullName)) });
            }
            Regex exp2 = new Regex("pid:(.*)", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            MatchCollection matchCollection2 = exp2.Matches(output);
            foreach (Match y in matchCollection2)
            {
                string yTemp = y.ToString().Replace("pid:", "");
                string yAUX = yTemp.Trim();
                for (int i = 0; i < fileListEXE.Count; i++)
                {
                    if (fileListEXE[i].Name == "executable." + yAUX + ".exe")
                    {
                        fileList.Add(new VirusT() { IsChecked = false, Name = fileListEXE[i].Name, Path = new Uri("https://www.virustotal.com/gui/file/" + fileListEXE[i].Hash + "/detection") });
                    }
                }
            }
            if (fileList.Count > 10)
            {
                Right.Visibility = Visibility.Visible;
                Pages.Visibility = Visibility.Visible;
                Left.Visibility = Visibility.Visible;

                numberOfPages = (float)decimal.Divide(fileList.Count, 10);
                numberOfPages = (float)Math.Ceiling(numberOfPages);
                Pages.Content = "1 of " + numberOfPages;

                for (int i = 0; i < 10; i++)
                {
                    fileListAuxEXE.Add(new VirusT() { IsChecked = false, Name = fileListEXE[i].Name, Path = new Uri("https://www.virustotal.com/gui/file/" + fileListEXE[i].Hash + "/detection") });
                }
                dgDLL.ItemsSource = fileListAuxEXE;
                dgDLL.Items.Refresh();
            }
            else
            {
                dgDLL.ItemsSource = fileList;
                dgDLL.Items.Refresh();
            }
        }

        void RunCommandDLL()
        {
            BackgroundWorker bg = new BackgroundWorker();
            System.IO.Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + @"Saves\DLL");
            pbStatus.Visibility = Visibility.Visible;
            FindFile.IsEnabled = false;
            Right.IsEnabled = false;
            Left.IsEnabled = false;
            bg.DoWork += new DoWorkEventHandler(MethodToGetInfoDLL);
            bg.RunWorkerCompleted += new RunWorkerCompletedEventHandler(Bg_RunWorkerCompletedDLL);
            //show marquee here
            bg.RunWorkerAsync();
        }

        void MethodToGetInfoDLL(Object sender, DoWorkEventArgs args)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.FileName = "CMD.exe";
            startInfo.WorkingDirectory = Directory.GetCurrentDirectory();
            startInfo.Arguments = "/c volatility.exe -f " + "\"" + imageRam + "\"" + " --profile=" + imageRamProfile + " dlldump --dump-dir=" + "\"" + AppDomain.CurrentDomain.BaseDirectory + "Saves\\DLL" + "\"";  //"'" + folderAux + "'";
            startInfo.CreateNoWindow = true;
            process.StartInfo = startInfo;
            process.Start();
            string output1 = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
        }

        void Bg_RunWorkerCompletedDLL(object sender, RunWorkerCompletedEventArgs args)
        {
            //this method will be called once background worker has completed it's task
            //hide the marquee
            pbStatus.Visibility = Visibility.Hidden;
            FindFile.IsEnabled = true;
            Right.IsEnabled = true;
            Left.IsEnabled = true;
            fillListDLL();
        }


        private void fillListDLL()
        {
            //aqui
            List<string> outputAUX = new List<string>();
            fileList.Clear();
            fileListAuxDLL.Clear();
            fileListDLL.Clear();
            outputAUX.Clear();
            outputdll = "";
            // Create a DirectoryInfo object representing the specified directory.
            var dir = new DirectoryInfo(folderAux);
            // Get the FileInfo objects for every file in the directory.
            FileInfo[] files = dir.GetFiles();
            if (ComboBoxY.SelectedItem.ToString().Contains("Entire System"))
            {
                foreach (FileInfo file in files)
                {
                    fileListDLL.Add(new EXE() { Name = file.Name, Hash = BytesToString(GetHashSha256(file.FullName)) });
                }
            }
            else
            {
                string aux = ComboBoxY.SelectedItem.ToString();
                aux = aux.Substring(7).Trim();
                foreach (FileInfo file in files)
                {
                    if (file.Name.StartsWith("module." + aux + "."))
                    {
                        fileListDLL.Add(new EXE() { Name = file.Name, Hash = BytesToString(GetHashSha256(file.FullName)) });
                    }
                }
            }

            Regex exp = new Regex("pid:(.*)", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            MatchCollection matchCollection = exp.Matches(output);
            foreach (Match y in matchCollection)
            {
                outputAUX.Add(y.ToString());
            }

            foreach (string aux2 in outputAUX)
            {
                string aux3 = BetweenStringsWithInputEspecial(aux2, "************************************************************************", output);
                string[] auxArr = aux3.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                foreach (string aux4 in auxArr)
                {
                    if (aux4.StartsWith("0x"))
                    {
                        outputdll += aux2 + " " + aux4;
                    }
                }
            }

            Regex exp2 = new Regex("pid:(.*)", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            MatchCollection matchCollection2 = exp2.Matches(outputdll);
            foreach (Match y in matchCollection2)
            {
                string yPid = y.ToString().Split(' ')[1];
                string yOff = y.ToString().Split(' ')[2];
                yOff = yOff.Replace("0x", "");
                yOff = yOff.Replace("0", "");
                for (int i = 0; i < fileListDLL.Count; i++)
                {
                    if (fileListDLL[i].Name.Contains("module." + yPid) && fileListDLL[i].Name.Contains(yOff))
                    {
                        fileList.Add(new VirusT() { IsChecked = false, Name = fileListDLL[i].Name, Path = new Uri("https://www.virustotal.com/gui/file/" + fileListDLL[i].Hash + "/detection") });
                    }
                }
            }
            if (fileList.Count > 10)
            {
                Right.Visibility = Visibility.Visible;
                Pages.Visibility = Visibility.Visible;
                Left.Visibility = Visibility.Visible;

                numberOfPages = (float)decimal.Divide(fileList.Count, 10);
                numberOfPages = (float)Math.Ceiling(numberOfPages);
                Pages.Content = "1 of " + numberOfPages;

                for (int i = 0; i < 10; i++)
                {
                    fileListAuxDLL.Add(new VirusT() { IsChecked = false, Name = fileListDLL[i].Name, Path = new Uri("https://www.virustotal.com/gui/file/" + fileListDLL[i].Hash + "/detection") });
                }
                dgDLL.ItemsSource = fileListAuxDLL;
                dgDLL.Items.Refresh();
            }
            else
            {
                dgDLL.ItemsSource = fileList;
                dgDLL.Items.Refresh();
            }
        }

        // The cryptographic service provider.
        private SHA256 Sha256 = SHA256.Create();

        // Compute the file's hash.
        private byte[] GetHashSha256(string filename)
        {
            using (FileStream stream = File.OpenRead(filename))
            {
                return Sha256.ComputeHash(stream);
            }
        }

        // Return a byte array as a sequence of hex values.
        public static string BytesToString(byte[] bytes)
        {
            string result = "";
            foreach (byte b in bytes) result += b.ToString("x2");
            return result;
        }


        private async void RunAPI(object sender, RoutedEventArgs e)
        {
            if (lbl_Status.Text == "" || lbl_Status.Text == null || lbl_Status.Text.Length < 64)
            {
                MessageBox.Show("Please input a valid Virus Total API Key");
            }
            else
            {
                CentralWindow.historyListCons.Add(DateTime.UtcNow.ToString("HH:mm") + " | Run Virus Total API");
                //KeyInfo.Content = "500 out of 500 requests left";
                //KeyInfo.Visibility = Visibility.Visible;                
                for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
                {
                    if (vis is DataGridRow)
                    {
                        var row = (DataGridRow)vis;
                        if (dgDLL.ItemsSource == fileListAuxEXE)
                        {
                            file = AppDomain.CurrentDomain.BaseDirectory + @"Saves\EXEs\" + fileListAuxEXE[row.GetIndex()].Name;
                        }
                        else if (dgDLL.ItemsSource == fileListAuxDLL)
                        {
                            file = AppDomain.CurrentDomain.BaseDirectory + @"Saves\DLL\" + fileListAuxDLL[row.GetIndex()].Name;
                        }
                        //verificar tamanh0 do ficheiro
                        FileInfo fi = new FileInfo(file);
                        if (fi.Length >= 20971520)
                        {
                            MessageBox.Show("Sorry this file cannot be scanned by the API because it's bigger than 20MB, please scan it manually");
                        }
                        else
                        {
                            if (PrivateCheck.IsChecked == true)
                            {
                                countPrivAPI++;
                                pbStatus.Visibility = Visibility.Visible;
                                FindFile.IsEnabled = false;
                                row.IsEnabled = false;
                                Right.IsEnabled = false;
                                Left.IsEnabled = false;
                                PrivateCheck.IsEnabled = false;
                                lbl_Status.IsEnabled = false;
                                KeyInfo.Visibility = Visibility.Visible;
                                KeyInfo.Content = "Running " + countPrivAPI + " Processes. Using Private API Key.";
                                using (var httpClient = new HttpClient())
                                {
                                    using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://www.virustotal.com/vtapi/v2/file/scan"))
                                    {
                                        var multipartContent = new MultipartFormDataContent();
                                        multipartContent.Add(new StringContent(lbl_Status.Text), "apikey");
                                        multipartContent.Add(new ByteArrayContent(File.ReadAllBytes(file)), "file", Path.GetFileName(file));
                                        request.Content = multipartContent;

                                        var response = await httpClient.SendAsync(request);
                                        if (!response.StatusCode.Equals(200))
                                        {
                                            MessageBox.Show("Something went wrong, the link will not work. The problem is probably the API key or the connection");
                                        }
                                    }
                                }
                                await Task.Delay(30000);
                                row.IsEnabled = true;
                                countPrivAPI--;
                                KeyInfo.Content = "Running " + countPrivAPI + " Processes. Using Private API Key.";
                                if (countPrivAPI == 0)
                                {
                                    FindFile.IsEnabled = true;
                                    Right.IsEnabled = true;
                                    Left.IsEnabled = true;
                                    PrivateCheck.IsEnabled = true;
                                    lbl_Status.IsEnabled = true;
                                    KeyInfo.Visibility = Visibility.Hidden;
                                    pbStatus.Visibility = Visibility.Hidden;
                                }
                            }
                            else
                            {
                                if (countPublicAPITotal > 500)
                                {
                                    MessageBox.Show("You problaby reached the maximum number of requests in a day. Check the API offical page.");
                                }
                                else
                                {
                                    if (timer.IsEnabled == false)
                                    {
                                        timer.Interval = TimeSpan.FromSeconds(60);
                                        timer.Tick += TimerTick;
                                        timer.Start();
                                    }
                                    if (timer.IsEnabled == true && countPublicAPI == 4 || countPublicAPIInterval == 4)
                                    {
                                        MessageBox.Show("You can only make 4 requests a minute please wait");
                                    }
                                    else
                                    {
                                        countPublicAPIInterval++;
                                        countPublicAPITotal++;
                                        countPublicAPI++;
                                        pbStatus.Visibility = Visibility.Visible;
                                        FindFile.IsEnabled = false;
                                        row.IsEnabled = false;
                                        Right.IsEnabled = false;
                                        Left.IsEnabled = false;
                                        PrivateCheck.IsEnabled = false;
                                        lbl_Status.IsEnabled = false;
                                        KeyInfo.Visibility = Visibility.Visible;
                                        KeyInfo.Content = "Running " + countPublicAPI + " Processes. Using Public API Key.";
                                        using (var httpClient = new HttpClient())
                                        {
                                            using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://www.virustotal.com/vtapi/v2/file/scan"))
                                            {
                                                var multipartContent = new MultipartFormDataContent();
                                                multipartContent.Add(new StringContent(lbl_Status.Text), "apikey");
                                                multipartContent.Add(new ByteArrayContent(File.ReadAllBytes(file)), "file", Path.GetFileName(file));
                                                request.Content = multipartContent;

                                                var response = await httpClient.SendAsync(request);
                                                if (response.StatusCode.Equals(403))
                                                {
                                                    MessageBox.Show("Something went wrong, the link will not work. The problem is probably the API key or the connection");
                                                }
                                                if (response.StatusCode.Equals(204))
                                                {
                                                    MessageBox.Show("Something went wrong, too many requests");
                                                }
                                            }
                                        }
                                        await Task.Delay(30000);
                                        row.IsEnabled = true;

                                        //Ver
                                        //row.FontWeight = FontWeights.Bold;

                                        countPublicAPI--;
                                        KeyInfo.Content = "Running " + countPublicAPI + " Processes. Using Public API Key.";
                                        if (countPublicAPI == 0)
                                        {
                                            FindFile.IsEnabled = true;
                                            Right.IsEnabled = true;
                                            Left.IsEnabled = true;
                                            PrivateCheck.IsEnabled = true;
                                            lbl_Status.IsEnabled = true;
                                            KeyInfo.Visibility = Visibility.Hidden;
                                            pbStatus.Visibility = Visibility.Hidden;
                                        }
                                    }
                                }

                            }

                        }
                    }
                }
            }
        }

        private void TimerTick(object sender, EventArgs e)
        {
            countPublicAPIInterval = 0;
            ((DispatcherTimer)sender).Stop();
        }

        private void Right_Click(object sender, RoutedEventArgs e)
        {
            string pagesAUX = Pages.Content.ToString();
            string numberofPageTemp = pagesAUX.Substring(0, 1);
            int numberofPageNow = Int32.Parse(numberofPageTemp);

            if (pagesAUX.Contains(numberOfPages + " of " + numberOfPages))
            {
                //
            }
            else
            {
                Pages.Content = (numberofPageNow + 1) + " of " + numberOfPages;
                if (dgDLL.ItemsSource == fileListAuxEXE)
                {
                    fileListAuxEXE.Clear();
                    for (int i = numberofPageNow * 10; i < numberofPageNow * 10 + 10; i++)
                    {
                        if (i < fileListEXE.Count)
                        {
                            fileListAuxEXE.Add(new VirusT() { IsChecked = false, Name = fileListEXE[i].Name, Path = new Uri("https://www.virustotal.com/gui/file/" + fileListEXE[i].Hash + "/detection") });
                        }
                    }
                    dgDLL.ItemsSource = fileListAuxEXE;
                    dgDLL.Items.Refresh();
                }
                else if (dgDLL.ItemsSource == fileListAuxDLL)
                {
                    fileListAuxDLL.Clear();
                    for (int i = numberofPageNow * 10; i < numberofPageNow * 10 + 10; i++)
                    {
                        if (i < fileListDLL.Count)
                        {
                            fileListAuxDLL.Add(new VirusT() { IsChecked = false, Name = fileListDLL[i].Name, Path = new Uri("https://www.virustotal.com/gui/file/" + fileListDLL[i].Hash + "/detection") });
                        }
                    }
                    dgDLL.ItemsSource = fileListAuxDLL;
                    dgDLL.Items.Refresh();
                }
            }
        }

        private void Left_Click(object sender, RoutedEventArgs e)
        {
            string pagesAUX = Pages.Content.ToString();
            string numberofPageTemp = pagesAUX.Substring(0, 1);
            int numberofPageNow = Int32.Parse(numberofPageTemp);

            if (pagesAUX.Contains("1 of"))
            {
                //
            }
            else
            {
                Pages.Content = (numberofPageNow - 1) + " of " + numberOfPages;
                if (numberofPageNow == 2)
                {
                    if (dgDLL.ItemsSource == fileListAuxEXE)
                    {
                        fileListAuxEXE.Clear();
                        for (int i = 0; i < 10; i++)
                        {
                            fileListAuxEXE.Add(new VirusT() { IsChecked = false, Name = fileListEXE[i].Name, Path = new Uri("https://www.virustotal.com/gui/file/" + fileListEXE[i].Hash + "/detection") });
                        }
                        dgDLL.ItemsSource = fileListAuxEXE;
                        dgDLL.Items.Refresh();
                    }
                    else if (dgDLL.ItemsSource == fileListAuxDLL)
                    {
                        fileListAuxDLL.Clear();
                        for (int i = 0; i < 10; i++)
                        {
                            fileListAuxDLL.Add(new VirusT() { IsChecked = false, Name = fileListDLL[i].Name, Path = new Uri("https://www.virustotal.com/gui/file/" + fileListDLL[i].Hash + "/detection") });
                        }
                        dgDLL.ItemsSource = fileListAuxDLL;
                        dgDLL.Items.Refresh();
                    }
                }
                else
                {
                    if (dgDLL.ItemsSource == fileListAuxEXE)
                    {
                        fileListAuxEXE.Clear();
                        for (int i = numberofPageNow * 10 - 20; i < numberofPageNow * 10 - 10; i++)
                        {
                            if (i < fileListEXE.Count)
                            {
                                fileListAuxEXE.Add(new VirusT() { IsChecked = false, Name = fileListEXE[i].Name, Path = new Uri("https://www.virustotal.com/gui/file/" + fileListEXE[i].Hash + "/detection") });
                            }
                        }
                        dgDLL.ItemsSource = fileListAuxEXE;
                        dgDLL.Items.Refresh();
                    }
                    else if (dgDLL.ItemsSource == fileListAuxDLL)
                    {
                        fileListAuxDLL.Clear();
                        for (int i = numberofPageNow * 10 - 20; i < numberofPageNow * 10 - 10; i++)
                        {
                            if (i < fileListDLL.Count)
                            {
                                fileListAuxDLL.Add(new VirusT() { IsChecked = false, Name = fileListDLL[i].Name, Path = new Uri("https://www.virustotal.com/gui/file/" + fileListDLL[i].Hash + "/detection") });
                            }
                        }
                        dgDLL.ItemsSource = fileListAuxDLL;
                        dgDLL.Items.Refresh();
                    }
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

        private void ComboBoxP_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBoxP.SelectedItem.ToString().Contains("DLLs"))
            {
                ComboBoxY.Visibility = Visibility.Visible;
            }
            else
            {
                ComboBoxY.Visibility = Visibility.Hidden;
            }
        }
    }
}

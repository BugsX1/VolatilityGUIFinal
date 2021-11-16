using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Windows;

namespace VolatilityGUI
{
    /// <summary>
    /// Interaction logic for NewRAMWindow.xaml
    /// </summary>
    public partial class NewRAMWindow : Window
    {
        private string output;
        private string ramImageName;
        private string selectedAUX;
        private string selectedProfile;

        public NewRAMWindow()
        {
            InitializeComponent();
            CentralWindow.historyListCons.Add(DateTime.UtcNow.ToString("HH:mm") + " | New RAM Window Opened");
            //spinnerLoading.Visibility = Visibility.Collapsed;
            CentralWindow.closingAUXCons = "something";
        }

        public string ramImageNameCons
        {
            get { return ramImageName; }
            set { ramImageName = value; }

        }

        private void btnOpenRAM_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog selectItem = new OpenFileDialog
            {
                Filter = "RAW files (*.RAW)|*.RAW|All files (*.*)|*.*"
            };
            if (selectItem.ShowDialog() == true)
            {
                txtSelected.Text = selectItem.FileName;
                ramImageName = selectItem.SafeFileName;
                selectedAUX = selectItem.FileName;

                bool aux1 = selectedAUX.Contains(".raw");
                if (selectedAUX.Contains(".sys") || selectedAUX.Contains(".dd") || selectedAUX.Contains(".dmp") || selectedAUX.Contains(".elf") || selectedAUX.Contains(".vmdk") || selectedAUX.Contains(".vmsd") || selectedAUX.Contains(".vmsn") || selectedAUX.Contains(".e01") || selectedAUX.Contains(".ko") || selectedAUX.Contains(".o") || selectedAUX.Contains(".img") || selectedAUX.Contains(".fdpro") || selectedAUX.Contains(".dv") || selectedAUX.Contains(".raw"))
                {
                    CentralWindow.historyListCons.Add(DateTime.UtcNow.ToString("HH:mm") + " | RAM " + ramImageName + " loaded");
                    runCommand();
                }
                else
                {
                    txtImageInfoDate.Text = "No RAM Image";
                }

            }
        }

        void runCommand()
        {
            BackgroundWorker bg = new BackgroundWorker();
            pbStatus.Visibility = Visibility.Visible;
            SelectButton.IsEnabled = false;
            SelectButtonImage.IsEnabled = false;
            bg.DoWork += new DoWorkEventHandler(MethodToGetInfo);
            bg.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bg_RunWorkerCompleted);
            //show marquee here
            bg.RunWorkerAsync();
        }

        void MethodToGetInfo(Object sender, DoWorkEventArgs args)
        {
            // find system info here
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.FileName = "CMD.exe";
            startInfo.WorkingDirectory = Directory.GetCurrentDirectory();
            startInfo.Arguments = "/c volatility.exe -f " + "\"" + selectedAUX + "\"" + " imageinfo";
            startInfo.CreateNoWindow = true;
            process.StartInfo = startInfo;
            process.Start();
            output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

        }

        void bg_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs args)
        {
            //this method will be called once background worker has completed it's task
            //hide the marquee

            pbStatus.Visibility = Visibility.Hidden;
            SelectButton.IsEnabled = true;
            SelectButtonImage.IsEnabled = true;

            //update the textbox
            string outputDate = output.Substring(output.IndexOf("Image local date and time : ") + "Image local date and time : ".Length);

            string outputProcessor = BetweenStrings("Number of Processors : ", "     Image Type (Service Pack)");
            string outputSP = BetweenStrings("Image Type (Service Pack) : ", "                KPCR for");
            string outputPAE = BetweenStrings("PAE type : ", "                           DTB");
            string outputDTB = BetweenStrings("DTB : ", "                          KDBG");
            string outputKDBG = BetweenStrings("KDBG : ", "          Number of Processors");
            string outputKPCR = BetweenStrings("KPCR for CPU 0 : ", "             KUSER_SHARED_DATA");
            string outputKUSER = BetweenStrings("             KUSER_SHARED_DATA : ", "           Image date and time");
            string outputProfile = BetweenStrings("Suggested Profile(s) : ", "                     AS Layer1 :");

            txtImageInfoProfile.Text = outputProfile;
            txtImageInfoDate.Text = outputDate;
            txtImageInfoProcessor.Text = outputProcessor;
            txtImageInfoSP.Text = outputSP;
            txtImageInfoPAE.Text = outputPAE;
            txtImageInfoDTB.Text = outputDTB;
            txtImageInfoKDBG.Text = outputKDBG;
            txtImageInfoKPCR.Text = outputKPCR;
            txtImageInfoKUSER.Text = outputKUSER;

            txtsha256.Text = BytesToString(GetHashSha256(selectedAUX));
            txtsha1.Text = BytesToString(GetHashSha1(selectedAUX));
            txtmd5.Text = BytesToString(GetHashMd5(selectedAUX));

            //select profile
            string selectedProfileAUX = BetweenStrings("Suggested Profile(s) : ", ",");
            List<string> profiles = selectedProfileAUX.Split(',').ToList<string>();
            selectedProfile = profiles[0];
        }

        private void btnSelectImage_Click(object sender, RoutedEventArgs e)
        {
            if (selectedAUX == null)
            {
                MessageBox.Show("No File selected");
            }
            else
            {
                bool aux1 = selectedAUX.Contains(".raw");
                if (aux1 == false || selectedAUX.Length == 0)
                {
                    MessageBox.Show("No RAW file detected");
                }
                else
                {
                    if (CentralWindow.imageRamCons != null)
                    {
                        MessageBoxResult result = MessageBox.Show("Any unsave changes to the previous project will not be saved. Continue?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (result == MessageBoxResult.Yes)
                        {
                            CentralWindow.closingAUXCons = null;
                            App.Current.Windows[0].Close();
                            CentralWindow.imageRamCons = selectedAUX;
                            CentralWindow.imageRamProfileCons = selectedProfile;
                            CentralWindow.loadedFileCons = null;
                            CentralWindow objWindow = new CentralWindow();
                            objWindow.Show();
                            this.Close();
                            CentralWindow.closingAUXCons = "something";
                        }
                    }
                    else
                    {
                        CentralWindow.closingAUXCons = null;
                        App.Current.Windows[0].Close();
                        CentralWindow.imageRamCons = selectedAUX;
                        CentralWindow.imageRamProfileCons = selectedProfile;
                        CentralWindow objWindow = new CentralWindow();
                        objWindow.Show();
                        this.Close();
                        CentralWindow.closingAUXCons = "something";
                    }

                }
            }
        }

        private string BetweenStrings(string before, string after)
        {
            int pFrom = output.IndexOf(before) + before.Length;
            int pTo = output.LastIndexOf(after);
            string outputAUX = output.Substring(pFrom, pTo - pFrom);
            return outputAUX;
        }

        private SHA256 Sha256 = SHA256.Create();

        private byte[] GetHashSha256(string filename)
        {
            using (FileStream stream = File.OpenRead(filename))
            {
                return Sha256.ComputeHash(stream);
            }
        }

        private SHA1 Sha1 = SHA1.Create();

        private byte[] GetHashSha1(string filename)
        {
            using (FileStream stream = File.OpenRead(filename))
            {
                return Sha1.ComputeHash(stream);
            }
        }

        private MD5 Md5 = MD5.Create();

        private byte[] GetHashMd5(string filename)
        {
            using (FileStream stream = File.OpenRead(filename))
            {
                return Md5.ComputeHash(stream);
            }
        }

        // Return a byte array as a sequence of hex values.
        public static string BytesToString(byte[] bytes)
        {
            string result = "";
            foreach (byte b in bytes) result += b.ToString("x2");
            return result;
        }
    }
}

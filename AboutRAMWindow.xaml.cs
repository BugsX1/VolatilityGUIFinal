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
    /// Interaction logic for AboutRAMWindow.xaml
    /// </summary>
    public partial class AboutRAMWindow : Window
    {

        private string imageRAM;
        private string output;
        public AboutRAMWindow()
        {
            InitializeComponent();
            CentralWindow.historyListCons.Add(DateTime.UtcNow.ToString("HH:mm") + " | About RAM Window Opened");
            imageRAM = CentralWindow.imageRamCons;
            txtSelected.Text = imageRAM;
            runCommand();
        }

        void runCommand()
        {
            BackgroundWorker bg = new BackgroundWorker();
            pbStatus.Visibility = Visibility.Visible;
            ChangeButtonProfile.IsEnabled = false;
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
            startInfo.Arguments = "/c volatility.exe -f " + "\"" + imageRAM + "\"" + " imageinfo";
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
            ChangeButtonProfile.IsEnabled = true;

            //update the textbox
            string outputDate = output.Substring(output.IndexOf("Image local date and time : ") + "Image local date and time : ".Length);

            string outputProcessor = BetweenStrings("Number of Processors : ", "     Image Type (Service Pack)");
            string outputSP = BetweenStrings("Image Type (Service Pack) : ", "                KPCR for");
            string outputPAE = BetweenStrings("PAE type : ", "                           DTB");
            string outputDTB = BetweenStrings("DTB : ", "                          KDBG");
            string outputKDBG = BetweenStrings("KDBG : ", "          Number of Processors");
            string outputKPCR = BetweenStrings("KPCR for CPU 0 : ", "             KUSER_SHARED_DATA");
            string outputKUSER = BetweenStrings("             KUSER_SHARED_DATA : ", "           Image date and time");
            //string outputProfile = BetweenStrings("Suggested Profile(s) : ", "                     AS Layer1 :");

            txtImageInfoDate.Text = outputDate;
            txtImageInfoProcessor.Text = outputProcessor;
            txtImageInfoSP.Text = outputSP;
            txtImageInfoPAE.Text = outputPAE;
            txtImageInfoDTB.Text = outputDTB;
            txtImageInfoKDBG.Text = outputKDBG;
            txtImageInfoKPCR.Text = outputKPCR;
            txtImageInfoKUSER.Text = outputKUSER;

            txtsha256.Text = BytesToString(GetHashSha256(imageRAM));
            txtsha1.Text = BytesToString(GetHashSha1(imageRAM));
            txtmd5.Text = BytesToString(GetHashMd5(imageRAM));

            //select profile
            string selectedProfileAUX = BetweenStrings("Suggested Profile(s) : ", ",");
            List<string> profiles = selectedProfileAUX.Split(',').ToList<string>();

            foreach (var i in profiles)
            {
                ComboBoxProfile.Items.Add(i.Trim());
            }
        }

        private string BetweenStrings(string before, string after)
        {
            int pFrom = output.IndexOf(before) + before.Length;
            int pTo = output.LastIndexOf(after);
            string outputAUX = output.Substring(pFrom, pTo - pFrom);
            return outputAUX;
        }

        private void btnChangeProfile_Click(object sender, RoutedEventArgs e)
        {
            if (ComboBoxProfile.SelectedItem != null)
            {
                string selectedProfile = ComboBoxProfile.SelectedItem.ToString();
                CentralWindow.historyListCons.Add(DateTime.UtcNow.ToString("HH:mm") + " | Profile Changed to " + selectedProfile);
                CentralWindow.imageRamProfileCons = selectedProfile;
                this.Close();
            }
            else
            {
                MessageBox.Show("No value Selected in Profile");
            }

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

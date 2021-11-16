using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace VolatilityGUI
{
    /// <summary>
    /// Interaction logic for ForensicWindow.xaml
    /// </summary>
    public partial class ForensicWindow : Window
    {
        private static string imageRam;
        private static string imageRamProfile;
        private string hiveSYSTEM;
        private string hiveSAM;
        private string output;
        List<string> users = new List<string>();
        List<string> processList = new List<string>();
        List<string> netList = new List<string>();

        public ForensicWindow()
        {
            InitializeComponent();
            CentralWindow.historyListCons.Add(DateTime.UtcNow.ToString("HH:mm") + " | About Forensic Investigation Window Opened");
            imageRam = CentralWindow.imageRamCons;
            imageRamProfile = CentralWindow.imageRamProfileCons;
            if (imageRam == null)
            {
                MessageBox.Show("No RAM Image Selected");
            }
            else
            {
                MessageBox.Show("Warning! This is just ideia. Any real investagion should take place before checking this page.");
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
            startInfo.Arguments = "/c volatility.exe -f " + "\"" + imageRam + "\"" + " --profile=" + imageRamProfile + " hivelist";
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
            Regex exp = new Regex(".*SAM.*", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            MatchCollection matchCollection = exp.Matches(output);
            foreach (Match y in matchCollection)
            {
                hiveSAM = y.Value.Split(' ')[0];
            }
            Regex exp1 = new Regex(".*SYSTEM.*", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            MatchCollection matchCollection1 = exp1.Matches(output);
            foreach (Match x in matchCollection1)
            {
                hiveSYSTEM = x.Value.Split(' ')[0];
            }
            runCommandCycle();
        }

        void runCommandCycle()
        {
            BackgroundWorker bg = new BackgroundWorker();
            bg.DoWork += new DoWorkEventHandler(MethodToGetInfoCycle);
            bg.RunWorkerCompleted += new RunWorkerCompletedEventHandler(Bg_RunWorkerCompletedCycle);
            //show marquee here
            bg.RunWorkerAsync();
        }

        void MethodToGetInfoCycle(Object sender, DoWorkEventArgs args)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.FileName = "CMD.exe";
            startInfo.WorkingDirectory = Directory.GetCurrentDirectory();
            startInfo.Arguments = "/c volatility.exe -f " + "\"" + imageRam + "\"" + " --profile=" + imageRamProfile + " hashdump - y " + hiveSYSTEM + " -s " + hiveSAM;
            startInfo.CreateNoWindow = true;
            process.StartInfo = startInfo;
            process.Start();
            output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
        }

        void Bg_RunWorkerCompletedCycle(object sender, RunWorkerCompletedEventArgs args)
        {
            //this method will be called once background worker has completed it's task
            //hide the marquee
            string[] outputArrNew = output.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            foreach (string aux in outputArrNew)
            {
                string[] outputArrDuo = aux.Split(':');
                users.Add(outputArrDuo[0]);
            }
            runCommandMal();
        }

        void runCommandMal()
        {
            BackgroundWorker bg = new BackgroundWorker();
            pbStatus.Visibility = Visibility.Visible;
            bg.DoWork += new DoWorkEventHandler(MethodToGetInfoMal);
            bg.RunWorkerCompleted += new RunWorkerCompletedEventHandler(Bg_RunWorkerCompletedMal);
            //show marquee here
            bg.RunWorkerAsync();
        }

        void MethodToGetInfoMal(Object sender, DoWorkEventArgs args)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.FileName = "CMD.exe";
            startInfo.WorkingDirectory = Directory.GetCurrentDirectory();
            startInfo.Arguments = "/c volatility.exe -f " + "\"" + imageRam + "\"" + " --profile=" + imageRamProfile + " malfind";
            startInfo.CreateNoWindow = true;
            process.StartInfo = startInfo;
            process.Start();
            output = process.StandardOutput.ReadToEnd();
            output += Environment.NewLine;
            output += "------------------------------------------------------------";
            process.WaitForExit();
        }

        void Bg_RunWorkerCompletedMal(object sender, RunWorkerCompletedEventArgs args)
        {
            //this method will be called once background worker has completed it's task
            //hide the marquee
            Regex exp = new Regex("Process: (.*)", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            MatchCollection matchCollection = exp.Matches(output);
            foreach (Match y in matchCollection)
            {
                string aux = y.Value.Split(' ')[1] + " (" + y.Value.Split(' ')[3] + ")";
                if (!processList.Contains(aux))
                {
                    processList.Add(aux);
                }
            }
            runCommandNet();
        }

        void runCommandNet()
        {
            BackgroundWorker bg = new BackgroundWorker();
            pbStatus.Visibility = Visibility.Visible;
            bg.DoWork += new DoWorkEventHandler(MethodToGetInfoNet);
            bg.RunWorkerCompleted += new RunWorkerCompletedEventHandler(Bg_RunWorkerCompletedNet);
            //show marquee here
            bg.RunWorkerAsync();
        }

        void MethodToGetInfoNet(Object sender, DoWorkEventArgs args)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.FileName = "CMD.exe";
            startInfo.WorkingDirectory = Directory.GetCurrentDirectory();
            startInfo.Arguments = "/c volatility.exe -f " + "\"" + imageRam + "\"" + " --profile=" + imageRamProfile + " netscan";
            startInfo.CreateNoWindow = true;
            process.StartInfo = startInfo;
            process.Start();
            output = process.StandardOutput.ReadToEnd();
            output += Environment.NewLine;
            output += "------------------------------------------------------------";
            process.WaitForExit();
        }

        void Bg_RunWorkerCompletedNet(object sender, RunWorkerCompletedEventArgs args)
        {
            //this method will be called once background worker has completed it's task
            //hide the marquee
            string[] outputArrNet = output.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            foreach (string aux in outputArrNet)
            {
                netList.Add(aux);
            }
            runCommandInv();
        }

        void runCommandInv()
        {
            BackgroundWorker bg = new BackgroundWorker();
            bg.DoWork += new DoWorkEventHandler(MethodToGetInfoInv);
            bg.RunWorkerCompleted += new RunWorkerCompletedEventHandler(Bg_RunWorkerCompletedInv);
            //show marquee here
            bg.RunWorkerAsync();
        }

        void MethodToGetInfoInv(Object sender, DoWorkEventArgs args)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.FileName = "CMD.exe";
            startInfo.WorkingDirectory = Directory.GetCurrentDirectory();
            startInfo.Arguments = "/c volatility.exe -f " + "\"" + imageRam + "\"" + " --profile=" + imageRamProfile + " getsids";
            startInfo.CreateNoWindow = true;
            process.StartInfo = startInfo;
            process.Start();
            output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
        }

        void Bg_RunWorkerCompletedInv(object sender, RunWorkerCompletedEventArgs args)
        {
            Paragraph paragraph = new Paragraph();
            string outputAux = "";
            //this method will be called once background worker has completed it's task
            //hide the marquee
            pbStatus.Visibility = Visibility.Hidden;
            paragraph.Inlines.Add(new Run("This is a resume of the users and it's launched processes. The red ones could be malware."));
            paragraph.Inlines.Add(Environment.NewLine);
            string[] outputArrNew = output.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            if (users.Contains(""))
            {
                users.Remove("");
            }
            foreach (string auxUser in users)
            {
                paragraph.Inlines.Add(new Bold(new Run("User: " + auxUser)));
                paragraph.Inlines.Add(Environment.NewLine);
                foreach (string aux in outputArrNew)
                {
                    bool contains = Regex.IsMatch(aux, $@"\b{auxUser}\b"); // yields true
                    if (contains == true)
                    {
                        foreach (string procMal in processList)
                        {
                            foreach (string netAux in netList)
                            {

                                if (aux.Contains(procMal) && !outputAux.Contains(aux) && netAux.Contains(procMal.Split(' ')[0]))
                                {
                                    outputAux += aux;
                                    outputAux += Environment.NewLine;
                                    paragraph.Inlines.Add(new Run(aux.Split(':')[0]) { Foreground = Brushes.Red });
                                    paragraph.Inlines.Add(new Run(" - Check NetScan"));
                                    paragraph.Inlines.Add(Environment.NewLine);
                                }
                                else if (aux.Contains(procMal) && !outputAux.Contains(aux))
                                {
                                    outputAux += aux;
                                    outputAux += Environment.NewLine;
                                    paragraph.Inlines.Add(new Run(aux.Split(':')[0]) { Foreground = Brushes.Red });
                                    paragraph.Inlines.Add(Environment.NewLine);
                                }
                                else if (!outputAux.Contains(aux))
                                {
                                    outputAux += aux;
                                    outputAux += Environment.NewLine;
                                    paragraph.Inlines.Add(new Run(aux.Split(':')[0]));
                                    paragraph.Inlines.Add(Environment.NewLine);
                                }
                            }
                        }
                    }
                }
            }
            InvBox.Document = new FlowDocument(paragraph);
            //aqui ver o que se pode fazer mais
        }
    }
}

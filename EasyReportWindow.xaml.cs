using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Windows;

namespace VolatilityGUI
{
    /// <summary>
    /// Interaction logic for EasyReportWindow.xaml
    /// </summary>
    public partial class EasyReportWindow : Window
    {
        private static string imageRam;
        private static string imageRamProfile;
        private string output1;
        private string output2;
        private string output3;
        private string output4;
        private string output5;
        private string output6;
        private string output7;
        private string output8;
        private string output9;
        private string output10;
        private string output11;
        private string output12;
        private string output13;
        private string output14;
        private string output15;
        private string output16;
        private string output17;
        private string output18;
        private string output19;
        private string output20;
        private string output21;
        private string output22;
        private string output23;
        private string output24;
        private string output25;
        private string output26;
        private string output27;
        private string output28;
        private string output29;
        private string output30;
        private string output31;
        private string output32;
        private string output33;
        private string output34;
        private string output35;
        private string output36;
        private string output37;
        private string output38;
        private string output39;
        private string output40;
        private string output41;
        private string output42;
        private string output43;
        private string output44;
        private string output45;
        private string output46;
        private string output47;
        private string output48;
        private string output49;
        private string output50;
        private string output51;
        private string output52;
        private string output53;
        private string output54;
        private string output55;
        private string outputHive;
        private string auxF;
        private string folder;
        private string fileName;
        private string commandToRun;
        List<string> htmlPage = new List<string>();
        List<string> htmlPageMEM = new List<string>();
        List<string> htmlPageMFT = new List<string>();
        List<string> lineToRun = new List<string>();
        private int lineToRunCount;
        //private bool cancelWasClicked = false;
        System.Diagnostics.Process process = new System.Diagnostics.Process();
        private string hiveSYSTEM;
        private string hiveSAM;
        private bool selectedAUX;

        public EasyReportWindow()
        {
            InitializeComponent();
            CentralWindow.historyListCons.Add(DateTime.UtcNow.ToString("HH:mm") + " | Easy Report Window Opened");
            imageRam = CentralWindow.imageRamCons;
            imageRamProfile = CentralWindow.imageRamProfileCons;
            lineToRunCount = 0;
            lineToRun.Clear();
            selectedAUX = false;
            //conclusionOutput();
        }

        private void BuildReport_Click(object sender, RoutedEventArgs e)
        {
            fileName = "VolatilityReport_" + DateTime.UtcNow.ToString("dd-MM-yyyy(HH-mm)");
            cleanOutputs();
            lineToRun.Clear();
            lineToRunCount = 0;
            htmlPage.Clear();
            htmlPageMEM.Clear();
            htmlPageMFT.Clear();
            getCheckBoxes();
            folder = null;
            if (lineToRun.Count != 0)
            {
                lbl_Progress.Visibility = Visibility.Visible;
                lbl_NumberProgress.Visibility = Visibility.Visible;
                pbStatus.Visibility = Visibility.Visible;
                ProgressCancel.Visibility = Visibility.Visible;
                runCommandCycle();
            }
        }



        private void runCommandCycle()
        {
            lbl_NumberProgress.Content = (lineToRunCount + 1) + " out of " + lineToRun.Count + " Processes Running";
            switch (lineToRun[lineToRunCount])
            {
                //verificar comandos com prof
                case "ImageInfo":
                    commandToRun = "imageinfo";
                    runCommand();
                    break;
                case "Processes":
                    commandToRun = "pslist";
                    runCommand();
                    break;
                case "ProcessesScan":
                    commandToRun = "psscan";
                    runCommand();
                    break;
                case "ProcesseHandles":
                    commandToRun = "handles";
                    runCommand();
                    break;
                case "ProcesseDLL":
                    commandToRun = "dlllist";
                    runCommand();
                    break;
                case "ProcesseSID":
                    commandToRun = "getsids";
                    runCommand();
                    break;
                case "ProcesseCMD":
                    //penso que nao faz sentido correr o cmdscan devido a serem parecidos mas este ter mais info
                    commandToRun = "consoles";
                    runCommand();
                    break;
                case "ProcessePrivileges":
                    commandToRun = "privs";
                    runCommand();
                    break;
                case "ProcesseEnvPrivileges":
                    commandToRun = "envars";
                    runCommand();
                    break;
                case "ProcesseVerify":
                    // muito lento avisar e grande
                    commandToRun = "verinfo";
                    runCommand();
                    break;
                case "MemMap":
                    // Se calhar é melhor nem incluir devido a nao ter info rletavente
                    commandToRun = "memmap";
                    runCommand();
                    break;
                case "InternetExplorer":
                    commandToRun = "iehistory";
                    runCommand();
                    break;
                case "KernelDrivers":
                    commandToRun = "modules";
                    runCommand();
                    break;
                case "KernelSSDT":
                    commandToRun = "ssdt";
                    runCommand();
                    break;
                case "Drivers":
                    commandToRun = "driverscan";
                    runCommand();
                    break;
                case "FileScan":
                    commandToRun = "filescan";
                    runCommand();
                    break;
                case "MutantScan":
                    commandToRun = "mutantscan";
                    runCommand();
                    break;
                case "SymbolicLinks":
                    commandToRun = "symlinkscan";
                    runCommand();
                    break;
                case "ETHREADobjects":
                    commandToRun = "thrdscan";
                    runCommand();
                    break;
                case "UnloadedModules":
                    commandToRun = "unloadedmodules";
                    runCommand();
                    break;
                case "Networking":
                    commandToRun = "connections";
                    runCommandNetworking();
                    break;
                case "RegistryHives":
                    commandToRun = "hivelist";
                    runCommand();
                    break;
                case "RegistryKeys":
                    commandToRun = "printkey";
                    runCommand();
                    break;
                case "RegistryDomain":
                    if (output22 != null)
                    {
                        Regex exp = new Regex(".*SAM.*", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        MatchCollection matchCollection = exp.Matches(output22);
                        foreach (Match y in matchCollection)
                        {
                            hiveSAM = y.Value.Split(' ')[0];
                        }
                        Regex exp1 = new Regex(".*SYSTEM.*", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        MatchCollection matchCollection1 = exp1.Matches(output22);
                        foreach (Match x in matchCollection1)
                        {
                            if (hiveSYSTEM == null)
                            {
                                hiveSYSTEM = x.Value.Split(' ')[0];
                            }
                        }
                        commandToRun = "hashdump";
                        runCommand();
                    }
                    else
                    {
                        runCommandHive();
                    }
                    break;
                case "RegistryLSA":
                    commandToRun = "lsadump";
                    runCommand();
                    break;
                case "RegistryUserAssist":
                    commandToRun = "userassist";
                    runCommand();
                    break;
                case "RegistryShell":
                    commandToRun = "shellbags";
                    runCommand();
                    break;
                case "RegistryShim":
                    commandToRun = "shimcache";
                    runCommand();
                    break;
                case "RegistrySID":
                    commandToRun = "getservicesids";
                    runCommand();
                    break;
                case "FileMFT":
                    // verificar lento e muito grande
                    commandToRun = "mftparser";
                    runCommand();
                    break;
                case "BIOS":
                    commandToRun = "bioskbd";
                    runCommand();
                    break;
                case "Timeliner":
                    // leva type
                    // é lento e grande , ver se vale a pena dividir por pedaços
                    commandToRun = "timeliner";
                    runCommand();
                    break;
                case "CrashInfo":
                    // varios comandos
                    commandToRun = "crashinfo";
                    runCommandCrash();
                    break;
                case "Sessions":
                    commandToRun = "sessions";
                    runCommand();
                    break;
                case "WNDObjects":
                    commandToRun = "wndscan";
                    runCommand();
                    break;
                case "DeskScan":
                    commandToRun = "deskscan";
                    runCommand();
                    break;
                case "Atom":
                    commandToRun = "atomscan";
                    runCommand();
                    break;
                case "Clipboard":
                    commandToRun = "clipboard";
                    runCommand();
                    break;
                case "EventHooks":
                    commandToRun = "eventhooks";
                    runCommand();
                    break;
                case "gahti":
                    commandToRun = "gahti";
                    runCommand();
                    break;
                case "MessageHooks":
                    commandToRun = "messagehooks";
                    runCommand();
                    break;
                case "UserHandles":
                    commandToRun = "userhandles";
                    runCommand();
                    break;
                case "gditimers":
                    commandToRun = "gditimers";
                    runCommand();
                    break;
                case "DesktopWindows":
                    // verificar pode levar dois comands mas deve so levar um em pricipio
                    commandToRun = "deskscan";
                    runCommand();
                    break;
                case "malfind":
                    commandToRun = "malfind";
                    runCommand();
                    break;
                case "services":
                    commandToRun = "svcscan";
                    runCommand();
                    break;
                case "ldrmodules":
                    commandToRun = "ldrmodules";
                    runCommand();
                    break;
                case "apihooks":
                    commandToRun = "apihooks";
                    runCommand();
                    break;
                case "idt":
                    commandToRun = "idt";
                    runCommand();
                    break;
                case "gdt":
                    commandToRun = "gdt";
                    runCommand();
                    break;
                case "threads":
                    // pode meter parametros se for mais facil para separar info
                    commandToRun = "threads";
                    runCommand();
                    break;
                case "Callbacks":
                    commandToRun = "callbacks";
                    runCommand();
                    break;
                case "driverirp":
                    commandToRun = "driverirp";
                    runCommand();
                    break;
                case "timers":
                    commandToRun = "timers";
                    runCommand();
                    break;
                case "Conclusion":
                    //aqui
                    runCommandConclusion();
                    break;
            }
        }

        private void getCheckBoxes()
        {
            if (ImageInfo.IsChecked == true)
            {
                lineToRun.Add(ImageInfo.Name);
            }
            if (Processes.IsChecked == true)
            {
                lineToRun.Add(Processes.Name);
            }
            if (ProcessesScan.IsChecked == true)
            {
                lineToRun.Add(ProcessesScan.Name);
            }
            if (ProcesseHandles.IsChecked == true)
            {
                lineToRun.Add(ProcesseHandles.Name);
            }
            if (ProcesseDLL.IsChecked == true)
            {
                lineToRun.Add(ProcesseDLL.Name);
            }
            if (ProcesseSID.IsChecked == true)
            {
                lineToRun.Add(ProcesseSID.Name);
            }
            if (ProcesseCMD.IsChecked == true)
            {
                lineToRun.Add(ProcesseCMD.Name);
            }
            if (ProcessePrivileges.IsChecked == true)
            {
                lineToRun.Add(ProcessePrivileges.Name);
            }
            if (ProcesseEnvPrivileges.IsChecked == true)
            {
                lineToRun.Add(ProcesseEnvPrivileges.Name);
            }
            if (ProcesseVerify.IsChecked == true)
            {
                lineToRun.Add(ProcesseVerify.Name);
            }
            if (MemMap.IsChecked == true)
            {
                lineToRun.Add(MemMap.Name);
            }
            if (InternetExplorer.IsChecked == true)
            {
                lineToRun.Add(InternetExplorer.Name);
            }
            if (KernelDrivers.IsChecked == true)
            {
                lineToRun.Add(KernelDrivers.Name);
            }
            if (KernelSSDT.IsChecked == true)
            {
                lineToRun.Add(KernelSSDT.Name);
            }
            if (Drivers.IsChecked == true)
            {
                lineToRun.Add(Drivers.Name);
            }
            if (FileScan.IsChecked == true)
            {
                lineToRun.Add(FileScan.Name);
            }
            if (MutantScan.IsChecked == true)
            {
                lineToRun.Add(MutantScan.Name);
            }
            if (SymbolicLinks.IsChecked == true)
            {
                lineToRun.Add(SymbolicLinks.Name);
            }
            if (ETHREADobjects.IsChecked == true)
            {
                lineToRun.Add(ETHREADobjects.Name);
            }
            if (UnloadedModules.IsChecked == true)
            {
                lineToRun.Add(UnloadedModules.Name);
            }
            if (Networking.IsChecked == true)
            {
                lineToRun.Add(Networking.Name);
            }
            if (RegistryHives.IsChecked == true)
            {
                lineToRun.Add(RegistryHives.Name);
            }
            if (RegistryKeys.IsChecked == true)
            {
                lineToRun.Add(RegistryKeys.Name);
            }
            if (RegistryDomain.IsChecked == true)
            {
                lineToRun.Add(RegistryDomain.Name);
            }
            if (RegistryLSA.IsChecked == true)
            {
                lineToRun.Add(RegistryLSA.Name);
            }
            if (RegistryUserAssist.IsChecked == true)
            {
                lineToRun.Add(RegistryUserAssist.Name);
            }
            if (RegistryShell.IsChecked == true)
            {
                lineToRun.Add(RegistryShell.Name);
            }
            if (RegistryShim.IsChecked == true)
            {
                lineToRun.Add(RegistryShim.Name);
            }
            if (RegistrySID.IsChecked == true)
            {
                lineToRun.Add(RegistrySID.Name);
            }
            if (FileMFT.IsChecked == true)
            {
                lineToRun.Add(FileMFT.Name);
            }
            if (BIOS.IsChecked == true)
            {
                lineToRun.Add(BIOS.Name);
            }
            if (Timeliner.IsChecked == true)
            {
                lineToRun.Add(Timeliner.Name);
            }
            if (CrashInfo.IsChecked == true)
            {
                lineToRun.Add(CrashInfo.Name);
            }
            if (Sessions.IsChecked == true)
            {
                lineToRun.Add(Sessions.Name);
            }
            if (WNDObjects.IsChecked == true)
            {
                lineToRun.Add(WNDObjects.Name);
            }
            if (DeskScan.IsChecked == true)
            {
                lineToRun.Add(DeskScan.Name);
            }
            if (Atom.IsChecked == true)
            {
                lineToRun.Add(Atom.Name);
            }
            if (Clipboard.IsChecked == true)
            {
                lineToRun.Add(Clipboard.Name);
            }
            if (EventHooks.IsChecked == true)
            {
                lineToRun.Add(EventHooks.Name);
            }
            if (gahti.IsChecked == true)
            {
                lineToRun.Add(gahti.Name);
            }
            if (MessageHooks.IsChecked == true)
            {
                lineToRun.Add(MessageHooks.Name);
            }
            if (UserHandles.IsChecked == true)
            {
                lineToRun.Add(UserHandles.Name);
            }
            if (gditimers.IsChecked == true)
            {
                lineToRun.Add(gditimers.Name);
            }
            if (DesktopWindows.IsChecked == true)
            {
                lineToRun.Add(DesktopWindows.Name);
            }
            if (malfind.IsChecked == true)
            {
                lineToRun.Add(malfind.Name);
            }
            if (services.IsChecked == true)
            {
                lineToRun.Add(services.Name);
            }
            if (ldrmodules.IsChecked == true)
            {
                lineToRun.Add(ldrmodules.Name);
            }
            if (apihooks.IsChecked == true)
            {
                lineToRun.Add(apihooks.Name);
            }
            if (idt.IsChecked == true)
            {
                lineToRun.Add(idt.Name);
            }
            if (gdt.IsChecked == true)
            {
                lineToRun.Add(gdt.Name);
            }
            if (threads.IsChecked == true)
            {
                lineToRun.Add(threads.Name);
            }
            if (Callbacks.IsChecked == true)
            {
                lineToRun.Add(Callbacks.Name);
            }
            if (driverirp.IsChecked == true)
            {
                lineToRun.Add(driverirp.Name);
            }
            if (timers.IsChecked == true)
            {
                lineToRun.Add(timers.Name);
            }
            if (Conclusion.IsChecked == true)
            {
                lineToRun.Add(Conclusion.Name);
            }
        }

        void runCommand()
        {
            BackgroundWorker bg = new BackgroundWorker();
            //pbStatus.Visibility = Visibility.Visible;
            BuildReport.IsEnabled = false;
            bg.DoWork += new DoWorkEventHandler(MethodToGetInfo);
            bg.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bg_RunWorkerCompleted);
            //show marquee here
            bg.RunWorkerAsync();
        }

        void MethodToGetInfo(Object sender, DoWorkEventArgs args)
        {
            // find system info here
            //System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.FileName = "CMD.exe";
            startInfo.WorkingDirectory = Directory.GetCurrentDirectory();
            if (commandToRun == "imageinfo")
            {
                startInfo.Arguments = "/c volatility.exe -f " + "\"" + imageRam + "\"" + " " + commandToRun;
            }
            else if (commandToRun == "hashdump")
            {
                startInfo.Arguments = "/c volatility.exe -f " + "\"" + imageRam + "\"" + " --profile=" + imageRamProfile + " " + commandToRun + " -y " + hiveSYSTEM + " -s " + hiveSAM;
            }
            else if (commandToRun == "hashdump2")
            {
                startInfo.Arguments = "/c volatility.exe -f " + "\"" + imageRam + "\"" + " --profile=" + imageRamProfile + " hashdump";
            }
            else
            {
                startInfo.Arguments = "/c volatility.exe -f " + "\"" + imageRam + "\"" + " --profile=" + imageRamProfile + " " + commandToRun;
            }
            startInfo.CreateNoWindow = true;
            process.StartInfo = startInfo;
            process.Start();
            //tudo comentado até nao dar problema
            //if (cancelWasClicked == true)
            //{
            //ProgressCancel.IsEnabled = true;
            //process.Kill();
            //BuildReport.IsEnabled = true;
            // lbl_Progress.Visibility = Visibility.Hidden;
            //   lbl_NumberProgress.Visibility = Visibility.Hidden;
            //   pbStatus.Visibility = Visibility.Hidden;
            //    ProgressCancel.Visibility = Visibility.Hidden;
            //}
            //else
            //{
            switch (lineToRun[lineToRunCount])
            {
                //determinar o output
                case "ImageInfo":
                    output1 = process.StandardOutput.ReadToEnd();
                    break;
                case "Processes":
                    output2 = process.StandardOutput.ReadToEnd();
                    break;
                case "ProcessesScan":
                    output3 = process.StandardOutput.ReadToEnd();
                    break;
                case "ProcesseHandles":
                    output4 = process.StandardOutput.ReadToEnd();
                    break;
                case "ProcesseDLL":
                    output5 = process.StandardOutput.ReadToEnd();
                    break;
                case "ProcesseSID":
                    output6 = process.StandardOutput.ReadToEnd();
                    break;
                case "ProcesseCMD":
                    output7 = process.StandardOutput.ReadToEnd();
                    break;
                case "ProcessePrivileges":
                    output8 = process.StandardOutput.ReadToEnd();
                    break;
                case "ProcesseEnvPrivileges":
                    output9 = process.StandardOutput.ReadToEnd();
                    break;
                case "ProcesseVerify":
                    output10 = process.StandardOutput.ReadToEnd();
                    break;
                case "MemMap":
                    output11 = process.StandardOutput.ReadToEnd();
                    break;
                case "InternetExplorer":
                    output12 = process.StandardOutput.ReadToEnd();
                    break;
                case "KernelDrivers":
                    output13 = process.StandardOutput.ReadToEnd();
                    break;
                case "KernelSSDT":
                    output14 = process.StandardOutput.ReadToEnd();
                    break;
                case "Drivers":
                    output15 = process.StandardOutput.ReadToEnd();
                    break;
                case "FileScan":
                    output16 = process.StandardOutput.ReadToEnd();
                    break;
                case "MutantScan":
                    output17 = process.StandardOutput.ReadToEnd();
                    break;
                case "SymbolicLinks":
                    output18 = process.StandardOutput.ReadToEnd();
                    break;
                case "ETHREADobjects":
                    output19 = process.StandardOutput.ReadToEnd();
                    break;
                case "UnloadedModules":
                    output20 = process.StandardOutput.ReadToEnd();
                    break;
                case "Networking":
                    output21 = process.StandardOutput.ReadToEnd();
                    break;
                case "RegistryHives":
                    output22 = process.StandardOutput.ReadToEnd();
                    break;
                case "RegistryKeys":
                    output23 = process.StandardOutput.ReadToEnd();
                    break;
                case "RegistryDomain":
                    output24 = process.StandardOutput.ReadToEnd();
                    break;
                case "RegistryLSA":
                    output25 = process.StandardOutput.ReadToEnd();
                    break;
                case "RegistryUserAssist":
                    output26 = process.StandardOutput.ReadToEnd();
                    break;
                case "RegistryShell":
                    output27 = process.StandardOutput.ReadToEnd();
                    break;
                case "RegistryShim":
                    output28 = process.StandardOutput.ReadToEnd();
                    break;
                case "RegistrySID":
                    output29 = process.StandardOutput.ReadToEnd();
                    break;
                case "FileMFT":
                    output30 = process.StandardOutput.ReadToEnd();
                    break;
                case "BIOS":
                    output31 = process.StandardOutput.ReadToEnd();
                    break;
                case "Timeliner":
                    output32 = process.StandardOutput.ReadToEnd();
                    break;
                case "CrashInfo":
                    output33 = process.StandardOutput.ReadToEnd();
                    break;
                case "Sessions":
                    output34 = process.StandardOutput.ReadToEnd();
                    break;
                case "WNDObjects":
                    output35 = process.StandardOutput.ReadToEnd();
                    break;
                case "DeskScan":
                    output36 = process.StandardOutput.ReadToEnd();
                    break;
                case "Atom":
                    output37 = process.StandardOutput.ReadToEnd();
                    break;
                case "Clipboard":
                    output38 = process.StandardOutput.ReadToEnd();
                    break;
                case "EventHooks":
                    output39 = process.StandardOutput.ReadToEnd();
                    break;
                case "gahti":
                    output40 = process.StandardOutput.ReadToEnd();
                    break;
                case "MessageHooks":
                    output41 = process.StandardOutput.ReadToEnd();
                    break;
                case "UserHandles":
                    output42 = process.StandardOutput.ReadToEnd();
                    break;
                case "gditimers":
                    output43 = process.StandardOutput.ReadToEnd();
                    break;
                case "DesktopWindows":
                    output44 = process.StandardOutput.ReadToEnd();
                    break;
                case "malfind":
                    output45 = process.StandardOutput.ReadToEnd();
                    break;
                case "services":
                    output46 = process.StandardOutput.ReadToEnd();
                    break;
                case "ldrmodules":
                    output47 = process.StandardOutput.ReadToEnd();
                    break;
                case "apihooks":
                    output48 = process.StandardOutput.ReadToEnd();
                    break;
                case "idt":
                    output49 = process.StandardOutput.ReadToEnd();
                    break;
                case "gdt":
                    output50 = process.StandardOutput.ReadToEnd();
                    break;
                case "threads":
                    output51 = process.StandardOutput.ReadToEnd();
                    break;
                case "Callbacks":
                    output52 = process.StandardOutput.ReadToEnd();
                    break;
                case "driverirp":
                    output53 = process.StandardOutput.ReadToEnd();
                    break;
                case "timers":
                    output54 = process.StandardOutput.ReadToEnd();
                    break;
                case "Conclusion":
                    output55 = process.StandardOutput.ReadToEnd();
                    break;
            }
            process.WaitForExit();
            // }
        }

        void bg_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs args)
        {
            //this method will be called once background worker has completed it's task
            //hide the marquee
            if (commandToRun == "imageinfo")
            {
                output1 += Environment.NewLine;
                output1 += "SHA256 : " + BytesToString(GetHashSha256(imageRam));
                output1 += Environment.NewLine;
                output1 += "SHA1 : " + BytesToString(GetHashSha1(imageRam));
                output1 += Environment.NewLine;
                output1 += "MD5 : " + BytesToString(GetHashMd5(imageRam));
            }

            if (commandToRun == "hashdump" && !output24.Contains(":"))
            {
                commandToRun = "hashdump2";
                runCommand();
            }
            else
            {
                lineToRunCount++;
                if (lineToRunCount < lineToRun.Count)
                {
                    runCommandCycle();
                }
                else
                {
                    BuildReport.IsEnabled = true;
                    lbl_Progress.Visibility = Visibility.Hidden;
                    lbl_NumberProgress.Visibility = Visibility.Hidden;
                    pbStatus.Visibility = Visibility.Hidden;
                    ProgressCancel.Visibility = Visibility.Hidden;
                    System.IO.Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + @"Saves\HTMLPage");
                    //aqui!!!!!!!!!!!!!! pre prod | falta testar
                    var dlg = new CommonOpenFileDialog();
                    dlg.Title = "Choose Download Folder";
                    dlg.IsFolderPicker = true;
                    dlg.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory + @"Saves\HTMLPage";
                    dlg.AllowNonFileSystemItems = false;
                    dlg.DefaultDirectory = AppDomain.CurrentDomain.BaseDirectory + @"Saves\HTMLPage";
                    dlg.EnsureFileExists = true;
                    dlg.EnsurePathExists = true;
                    dlg.EnsureReadOnly = false;
                    dlg.EnsureValidNames = true;
                    dlg.Multiselect = false;
                    dlg.ShowPlacesList = true;

                    if (dlg.ShowDialog() == CommonFileDialogResult.Ok)
                    {

                        buildWebpage();
                    }
                    else
                    {
                        buildWebpage();
                    }

                }
            }
        }

        void runCommandNetworking()
        {
            BackgroundWorker bg = new BackgroundWorker();
            //pbStatus.Visibility = Visibility.Visible;
            BuildReport.IsEnabled = false;
            bg.DoWork += new DoWorkEventHandler(MethodToGetInfoNetworking);
            bg.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bg_RunWorkerCompletedNetworking);
            //show marquee here
            bg.RunWorkerAsync();
        }

        void MethodToGetInfoNetworking(Object sender, DoWorkEventArgs args)
        {
            // find system info here
            //System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.FileName = "CMD.exe";
            startInfo.WorkingDirectory = Directory.GetCurrentDirectory();
            startInfo.Arguments = "/c volatility.exe -f " + "\"" + imageRam + "\"" + " --profile=" + imageRamProfile + " " + commandToRun;
            startInfo.CreateNoWindow = true;
            process.StartInfo = startInfo;
            process.Start();
            string outputNetworking = process.StandardOutput.ReadToEnd();
            if (outputNetworking.Contains("ERROR   : volatility.debug"))
            {

            }
            else
            {
                output21 += outputNetworking;
                output21 += Environment.NewLine;
            }

            process.WaitForExit();

        }

        void bg_RunWorkerCompletedNetworking(object sender, RunWorkerCompletedEventArgs args)
        {
            //this method will be called once background worker has completed it's task
            //hide the marquee 

            //lineToRunCount++;
            if (commandToRun == "connections")
            {
                commandToRun = "connscan";
                runCommandNetworking();
            }
            else if (commandToRun == "connscan")
            {
                commandToRun = "sockets";
                runCommandNetworking();
            }
            else if (commandToRun == "sockets")
            {
                commandToRun = "sockscan";
                runCommandNetworking();
            }
            else if (commandToRun == "sockscan")
            {
                commandToRun = "netscan";
                runCommandNetworking();
            }
            else if (commandToRun == "netscan")
            {
                lineToRunCount++;
                if (lineToRunCount < lineToRun.Count)
                {
                    runCommandCycle();
                }
                else
                {
                    BuildReport.IsEnabled = true;
                    lbl_Progress.Visibility = Visibility.Hidden;
                    lbl_NumberProgress.Visibility = Visibility.Hidden;
                    pbStatus.Visibility = Visibility.Hidden;
                    ProgressCancel.Visibility = Visibility.Hidden;
                    System.IO.Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + @"Saves\HTMLPage");
                    var dlg = new CommonOpenFileDialog();
                    dlg.Title = "Choose Download Folder";
                    dlg.IsFolderPicker = true;
                    dlg.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory + @"Saves\HTMLPage";
                    dlg.AllowNonFileSystemItems = false;
                    dlg.DefaultDirectory = AppDomain.CurrentDomain.BaseDirectory + @"Saves\HTMLPage";
                    dlg.EnsureFileExists = true;
                    dlg.EnsurePathExists = true;
                    dlg.EnsureReadOnly = false;
                    dlg.EnsureValidNames = true;
                    dlg.Multiselect = false;
                    dlg.ShowPlacesList = true;

                    if (dlg.ShowDialog() == CommonFileDialogResult.Ok)
                    {

                        buildWebpage();
                    }
                    else
                    {
                        buildWebpage();
                    }
                }
            }
        }


        void runCommandConclusion()
        {
            if (output45 == null)
            {
                commandToRun = "malfind";
            }
            else if (output2 == null)
            {
                commandToRun = "pslist";
            }
            else if (output5 == null)
            {
                commandToRun = "dlllist";
            }
            else if (output8 == null)
            {
                commandToRun = "privs";
            }
            else if (output9 == null)
            {
                commandToRun = "envars";
            }
            else if (output21 == null)
            {
                commandToRun = "connections";
            }
            BackgroundWorker bg = new BackgroundWorker();
            //pbStatus.Visibility = Visibility.Visible;
            BuildReport.IsEnabled = false;
            bg.DoWork += new DoWorkEventHandler(MethodToGetInfoConclusion);
            bg.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bg_RunWorkerCompletedConclusion);
            //show marquee here
            bg.RunWorkerAsync();
        }

        void MethodToGetInfoConclusion(Object sender, DoWorkEventArgs args)
        {
            // find system info here
            //System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.FileName = "CMD.exe";
            startInfo.WorkingDirectory = Directory.GetCurrentDirectory();
            startInfo.Arguments = "/c volatility.exe -f " + "\"" + imageRam + "\"" + " --profile=" + imageRamProfile + " " + commandToRun;
            startInfo.CreateNoWindow = true;
            process.StartInfo = startInfo;
            process.Start();
            if (commandToRun == "malfind")
            {
                output45 = process.StandardOutput.ReadToEnd();
            }
            else if (commandToRun == "pslist")
            {
                output2 = process.StandardOutput.ReadToEnd();
            }
            else if (commandToRun == "dlllist")
            {
                output5 = process.StandardOutput.ReadToEnd();
            }
            else if (commandToRun == "privs")
            {
                output8 = process.StandardOutput.ReadToEnd();
            }
            else if (commandToRun == "envars")
            {
                output9 = process.StandardOutput.ReadToEnd();
            }
            else if (commandToRun == "connections")
            {
                output21 = process.StandardOutput.ReadToEnd();
            }

            process.WaitForExit();
        }

        void bg_RunWorkerCompletedConclusion(object sender, RunWorkerCompletedEventArgs args)
        {
            //this method will be called once background worker has completed it's task
            //hide the marquee 
            //lineToRunCount++;
            if (output2 == null)
            {
                runCommandConclusion();
            }
            else if (output5 == null)
            {
                runCommandConclusion();
            }
            else if (output8 == null)
            {
                runCommandConclusion();
            }
            else if (output9 == null)
            {
                runCommandConclusion();
            }
            else if (output21 == null)
            {
                runCommandConclusion();
            }
            else
            {
                //tratar aqui o output55 para corrrer bem manupular os outputs corresponestes
                if (output45 == null)
                {
                    output55 = "Sorry the program hasn't able to build this section";
                }
                else
                {
                    conclusionOutput();
                }

                lineToRunCount++;
                if (lineToRunCount < lineToRun.Count)
                {
                    runCommandCycle();
                }
                else
                {
                    BuildReport.IsEnabled = true;
                    lbl_Progress.Visibility = Visibility.Hidden;
                    lbl_NumberProgress.Visibility = Visibility.Hidden;
                    pbStatus.Visibility = Visibility.Hidden;
                    ProgressCancel.Visibility = Visibility.Hidden;
                    System.IO.Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + @"Saves\HTMLPage");
                    var dlg = new CommonOpenFileDialog();
                    dlg.Title = "Choose Download Folder";
                    dlg.IsFolderPicker = true;
                    dlg.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory + @"Saves\HTMLPage";
                    dlg.AllowNonFileSystemItems = false;
                    dlg.DefaultDirectory = AppDomain.CurrentDomain.BaseDirectory + @"Saves\HTMLPage";
                    dlg.EnsureFileExists = true;
                    dlg.EnsurePathExists = true;
                    dlg.EnsureReadOnly = false;
                    dlg.EnsureValidNames = true;
                    dlg.Multiselect = false;
                    dlg.ShowPlacesList = true;

                    if (dlg.ShowDialog() == CommonFileDialogResult.Ok)
                    {

                        buildWebpage();
                    }
                    else
                    {
                        buildWebpage();
                    }
                }
            }
        }

        void conclusionOutput()
        {
            //aqui
            //////////////////////apagar depois///////////////////////////////
            /*
            output45 += "Process: explorer.exe Pid: 1860 Address: 0x3ee0000:";
            output45 += Environment.NewLine;
            output45 += "0x03ee0000 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 ................";
            output45 += Environment.NewLine;
            output45 += "0x03ee0000 0000 ADD [EAX], AL";
            output45 += Environment.NewLine;
            output45 += "Process: svchost.exe Pid: 1820 Address: 0x24f0000:";
            output45 += Environment.NewLine;
            output45 += "0x024f0000 20 00 00 00 e0 ff 07 00 0c 00 00 00 01 00 05 00 ................";
            output45 += Environment.NewLine;
            output45 += "0x024f0004 e0ff LOOPNZ 0x24f0005";
            output45 += Environment.NewLine;
            output45 += "Process: svchost.exe Pid: 1820 Address: 0x24f0000:";
            output45 += Environment.NewLine;
            output45 += "0x0 tem de aparecer";
            output45 += Environment.NewLine;
            output45 += "0x0 jah";
            output45 += Environment.NewLine;
            //output45 += "Pid:";
            //output45 += Environment.NewLine;
            output2 = "0xfffffa8003172b30 explorer.exe 1860 1756 19 645 1 0 2019-05-02 18:03:09 UTC+0000";
            output2 += Environment.NewLine;
            output2 += "0xfffffa8003162060 svchost.exe 1820 504 11 317 0 0 2019-05-02 18:05:09 UTC+0000";
            output2 += Environment.NewLine;
            output2 += "0xfffffa8003162060 svchost.exe 1820 504 11 317 0 0 2019-05-02 18:05:09 UTC+0000";
            output5 = "explorer.exe pid: 1860";
            output5 += Environment.NewLine;
            output5 += "0x00000000ffa20000 0x2c0000 0xffff C:\\Windows\\Explorer.EXE";
            output5 += Environment.NewLine;
            output5 += "0x0000000077b90000 0x1a9000 0xffff C:\\Windows\\SYSTEM32\ntdll.dll";
            output5 += Environment.NewLine;
            output5 += "svchost.exe pid: 1820";
            output5 += Environment.NewLine;
            output5 += "0x00000000ff300000 0xb000 0xffff C:\\Windows\\System32\\svchost.exe";
            output5 += Environment.NewLine;
            output5 += "0x0000000077b90000 0x1a9000 0xffff C:\\Windows\\SYSTEM32\\ntdll.dll";
            output5 += Environment.NewLine;
            output5 += "0x0000000077b90000 0x1a9000 0xffff C:\\Windows\\SYSTEM32\\ntdll.dll";
            output5 += Environment.NewLine;
            output5 += "teste.exe pid: 1111";
            output5 += Environment.NewLine;
            output5 += "0x00000000ff300000 0xb000 0xffff C:\\yo";
            output5 += Environment.NewLine;
            output5 += "0x0000000077b90000 0x1a9000 0xffff C:\\yo.dll";
            output8 = "1860 explorer.exe 0x0000000002b6cf00 ALLUSERSPROFILE C:\\ProgramData";
            output8 += Environment.NewLine;
            output8 += "1860 explorer.exe 0x0000000002b6cf00 APPDATA C:\\Users\\victim\\AppData\\Roaming";
            output8 += Environment.NewLine;
            output8 += "1860 explorer.exe 0x0000000002b6cf00 CommonProgramFiles C:\\Program Files\\Common Files";
            output8 += Environment.NewLine;
            output8 += "1820 svchost.exe 0x000000000024c850 FP_NO_HOST_CHECK NO";
            output8 += Environment.NewLine;
            output8 += "1820 svchost.exe 0x000000000024c850 LOCALAPPDATA C:\\Windows\\system32\\config\\systemprofile\\AppData\\Local";
            output8 += Environment.NewLine;
            output8 += "2876 WmiPrvSE.exe 0x0000000000221320 windows_tracing_flags 3";
            output8 += Environment.NewLine;
            output9 += "444 winlogon.exe 0x000000000041fed0 PROCESSOR_LEVEL 6";
            output9 += Environment.NewLine;
            output9 += "1860 explorer.exe 0x0000000002b6cf00 CommonProgramFiles C:\\Program Files\\Common Files";
            output9 += Environment.NewLine;
            output9 += "1820 svchost.exe 0x0000000002b6cf00 CommonProgramFiles C:\\Program Files\\Common Files";
            output9 += Environment.NewLine;
            output9 += "1860 explorer.exe 0x0000000002b6cf00 CommonProgramFiles C:\\Program Files\\Common Files";
            output9 += Environment.NewLine;
            output9 += "1860 explorer.exe 0x0000000002b6cf00 CommonProgramFiles C:\\Program Files\\Common Files";
            output9 += Environment.NewLine;
            output21 += "0x5c201ca0 UDPv4 0.0.0.0:5005 *:* 2464 wmpnetwk.exe 2019-05-02 18:05:14 UTC+0000";
            output21 += Environment.NewLine;
            output21 += "0x5c201ca0 UDPv6 :::5005 *:* 2464 wmpnetwk.exe 2019-05-02 18:05:14 UTC+0000";
            output21 += Environment.NewLine;
            output21 += "0x5c44e1b0 TCPv6 :::5357 :::0 LISTENING 4 System";
            output21 += Environment.NewLine;
            output21 += "0x5c44e1b0 TCPv4 0.0.0.0:5357 0.0.0.0:0 LISTENING 4 System";
            output21 += Environment.NewLine;
            output21 += "0x5c201ca0 UDPv4 0.0.0.0:5005 *:* 1860 explorer.exe 2019-05-02 18:05:14 UTC+0000";
            output21 += Environment.NewLine;
            output21 += "0x5c201ca0 UDPv4 0.0.0.0:5005 *:* 1820 svchost.exe 2019-05-02 18:05:14 UTC+0000";
            output21 += Environment.NewLine;
            output21 += "0x5c44e1b0 TCPv6 :::5357 :::0 LISTENING 1860 explorer.exe";
            output21 += Environment.NewLine;
            output21 += "0x5c44e1b0 TCPv4 0.0.0.0:5357 0.0.0.0:0 LISTENING 1820 svchost.exe";
            output21 += Environment.NewLine;
            */
            //////////////////////variaveis de teste///////////////////////////////////////////

            //isto precisa de estar aqui porque pode quebrar/ nºao apagar/////////
            output45 += Environment.NewLine;
            output45 += "------------------------------------------------------------";
            output5 += Environment.NewLine;
            output5 += "------------------------------------------------------------";
            List<string> processList = new List<string>();
            Regex exp = new Regex("Process: (.*)", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            MatchCollection matchCollection = exp.Matches(output45);
            foreach (Match y in matchCollection)
            {
                string aux = y.Value.Split(' ')[1] + " " + y.Value.Split(' ')[3];
                if (!processList.Contains(aux))
                {
                    processList.Add(aux);
                    //MessageBox.Show(aux);
                }
            }
            if (processList.Count != 0)
            {
                output55 += "This is a resume of the most suspicious Processes and there activities";
                output55 += Environment.NewLine;
            }
            else
            {
                output55 += "Volatility was not able to find any suspicious Processes";
                output55 += Environment.NewLine;
            }
            RegexOptions options = RegexOptions.None;
            Regex regex = new Regex("[ ]{2,}", options);
            string output2A = regex.Replace(output2, " ");
            string[] output2Array = output2A.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            foreach (string auxA in processList)
            {
                output55 += "Process Name: " + auxA.Split(' ')[0] + " PID: " + auxA.Split(' ')[1] + " - Info";
                output55 += Environment.NewLine;
                foreach (string aux in output2Array)
                {
                    if (aux.Contains(auxA.Split(' ')[0]) && aux.Contains(auxA.Split(' ')[1]))
                    {
                        output55 += "Offset: " + aux.Split(' ')[0] + " PPID: " + aux.Split(' ')[3] + " Thread: " + aux.Split(' ')[4];
                        output55 += Environment.NewLine;
                        output55 += "Start Time: " + aux.Split(' ')[8] + " " + aux.Split(' ')[9] + " " + aux.Split(' ')[10];
                        output55 += Environment.NewLine;
                    }

                }
            }
            ///////////////////////////////output5
            //RegexOptions options = RegexOptions.None;
            Regex regex2 = new Regex("[ ]{2,}", options);
            string output5A = regex.Replace(output5, " ");
            string[] output5Array = output5A.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            foreach (string auxA in processList)
            {
                output55 += "Process Name: " + auxA.Split(' ')[0] + " PID: " + auxA.Split(' ')[1] + " - DLLs";
                output55 += Environment.NewLine;

                foreach (string aux in output5Array)
                {
                    if (aux.Contains(auxA.ToString().Split(' ')[0] + " pid: " + auxA.ToString().Split(' ')[1]))
                    {
                        string aux2 = BetweenStringsWithInput(auxA.ToString().Split(' ')[0] + " pid: " + auxA.ToString().Split(' ')[1], "pid:", output5A);
                        string[] aux2Array = aux2.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                        foreach (string auxA2 in aux2Array)
                        {
                            if (auxA2.Contains(".dll"))
                            {
                                output55 += auxA2;
                                output55 += Environment.NewLine;
                            }
                        }
                    }

                }
            }
            //////////////output8
            string[] output8Array = output8.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            foreach (string auxA in processList)
            {
                output55 += "Process Name: " + auxA.Split(' ')[0] + " PID: " + auxA.Split(' ')[1] + " - Priviledges";
                output55 += Environment.NewLine;

                foreach (string aux in output8Array)
                {
                    if (aux.Contains(auxA.Split(' ')[1] + " " + auxA.Split(' ')[0]))
                    {
                        //output55 += "Process Name: " + auxA.Split(' ')[0] + " PID: " + auxA.Split(' ')[1] + " - Priviledges";
                        //output55 += Environment.NewLine;                      
                        //output55 += Environment.NewLine;
                        output55 += aux;
                        output55 += Environment.NewLine;
                    }
                    /*else
                    {
                        //output55 += "Process Name: " + processList[i].Split(' ')[0] + " PID: " + processList[i].Split(' ')[1];
                        //output55 += Environment.NewLine;
                        output55 += "No priviledges has able to be obtained from this process";
                        output55 += Environment.NewLine;
                    }*/
                }
            }
            //////////////output9
            string[] output9Array = output9.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            foreach (string auxA in processList)
            {
                output55 += "Process Name: " + auxA.Split(' ')[0] + " PID: " + auxA.Split(' ')[1] + " - Env Priviledges";
                output55 += Environment.NewLine;

                foreach (string aux in output9Array)
                {
                    if (aux.Contains(auxA.Split(' ')[1] + " " + auxA.Split(' ')[0]))
                    {
                        //output55 += "Process Name: " + auxA.Split(' ')[0] + " PID: " + auxA.Split(' ')[1] + " - Priviledges";
                        // output55 += Environment.NewLine;
                        //MessageBox.Show("teste");                        
                        //output55 += Environment.NewLine;
                        output55 += aux;
                        output55 += Environment.NewLine;
                    }
                    /*else
                    {
                        //output55 += "Process Name: " + processList[i].Split(' ')[0] + " PID: " + processList[i].Split(' ')[1];
                        //output55 += Environment.NewLine;
                        output55 += "No priviledges has able to be obtained from this process";
                        output55 += Environment.NewLine;
                    }*/
                }
            }
            //////////////////output21            
            string[] output21Array = output21.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            foreach (string auxA in processList)
            {
                output55 += "Process Name: " + auxA.Split(' ')[0] + " PID: " + auxA.Split(' ')[1] + " - Connections";
                output55 += Environment.NewLine;

                foreach (string aux in output21Array)
                {
                    if (aux.Contains(auxA.ToString().Split(' ')[0]) && aux.Contains(auxA.ToString().Split(' ')[1]))
                    {
                        output55 += "New Connection : ";
                        output55 += Environment.NewLine;
                        output55 += "Protocol : " + aux.Split(' ')[1] + " ";
                        if (aux.Contains("LISTENING"))
                        {
                            output55 += "LISTENING at IP : " + aux.Split(' ')[2] + " " + aux.Split(' ')[3] + " ";
                            output55 += Environment.NewLine;
                        }
                        else
                        {
                            output55 += "Connection made to IP : " + aux.Split(' ')[2] + " at " + aux.Split(' ')[6] + " " + aux.Split(' ')[7] + " " + aux.Split(' ')[8] + " ";
                            output55 += Environment.NewLine;
                        }
                        output55 += Environment.NewLine;
                    }

                }
            }
            string[] output45Array = output45.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            foreach (string auxA in processList)
            {
                output55 += "Process Name: " + auxA.Split(' ')[0] + " PID: " + auxA.Split(' ')[1] + " - Code";
                output55 += Environment.NewLine;

                foreach (string aux in output45Array)
                {
                    if (aux.Contains(auxA.ToString().Split(' ')[0] + " Pid: " + auxA.ToString().Split(' ')[1]))
                    {
                        string beforeA = auxA.ToString().Split(' ')[0] + " Pid: " + auxA.ToString().Split(' ')[1];
                        string aux2 = BetweenStringsWithInput(auxA.ToString().Split(' ')[0] + " Pid: " + auxA.ToString().Split(' ')[1], "Pid:", output45);

                        string[] aux2Array = aux2.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                        foreach (string auxA2 in aux2Array)
                        {
                            if (auxA2.StartsWith("0x"))
                            {
                                if (!output55.Contains(auxA2))
                                {
                                    output55 += auxA2;
                                    output55 += Environment.NewLine;
                                }
                            }
                        }
                    }

                }
            }
            //teste
            //buildWebpage();
        }

        private string BetweenStringsWithInput(string before, string after, string stringAux)
        {
            auxF = "";
            //string stringOutput = stringAux.Substring(stringAux.IndexOf(before));
            string stringOutput = stringAux.Substring(stringAux.IndexOf(before) + before.Length);
            //MessageBox.Show(stringOutput);
            int pFrom = stringAux.IndexOf(before) + before.Length;
            int pFromF = stringAux.LastIndexOf(before) + before.Length;
            int pTo = stringOutput.IndexOf(after); //+ before.Length;
            if (stringAux == output45)
            {
                if (pFrom != pFromF)
                {
                    string stringOutputF = stringAux.Substring(stringAux.LastIndexOf(before) + before.Length);
                    int pToF2 = stringOutputF.IndexOf(after);
                    if (pToF2 == -1)
                    {
                        int pToF3 = stringOutputF.IndexOf("------------------------------------------------------------");
                        auxF = stringAux.Substring(pFromF, pToF3 /*- pFrom*/);
                    }
                    else
                    {
                        auxF = stringAux.Substring(pFromF, pToF2 /*- pFrom*/);
                    }
                }
            }
            if (pTo == -1)
            {
                int pToF = stringOutput.IndexOf("------------------------------------------------------------");
                string outputAUX = stringAux.Substring(pFrom, pToF /*- pFrom*/);
                outputAUX += Environment.NewLine;
                outputAUX += auxF;
                return outputAUX;
            }
            else
            {
                string outputAUX = stringAux.Substring(pFrom, pTo /*- pFrom*/);
                outputAUX += Environment.NewLine;
                outputAUX += auxF;
                return outputAUX;
            }
        }

        void runCommandCrash()
        {
            BackgroundWorker bg = new BackgroundWorker();
            //pbStatus.Visibility = Visibility.Visible;
            BuildReport.IsEnabled = false;
            bg.DoWork += new DoWorkEventHandler(MethodToGetInfoCrash);
            bg.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bg_RunWorkerCompletedCrash);
            //show marquee here
            bg.RunWorkerAsync();
        }

        void MethodToGetInfoCrash(Object sender, DoWorkEventArgs args)
        {
            // find system info here
            //System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.FileName = "CMD.exe";
            startInfo.WorkingDirectory = Directory.GetCurrentDirectory();
            startInfo.Arguments = "/c volatility.exe -f " + "\"" + imageRam + "\"" + " --profile=" + imageRamProfile + " " + commandToRun;
            startInfo.CreateNoWindow = true;
            process.StartInfo = startInfo;
            process.Start();
            string outputCrash = process.StandardOutput.ReadToEnd();
            if (outputCrash.Contains("ERROR   : volatility.debug"))
            {

            }
            else
            {
                output33 += outputCrash;
                output33 += Environment.NewLine;
            }

            process.WaitForExit();

        }

        void bg_RunWorkerCompletedCrash(object sender, RunWorkerCompletedEventArgs args)
        {
            //this method will be called once background worker has completed it's task
            //hide the marquee 

            //lineToRunCount++;
            if (commandToRun == "crashinfo")
            {
                commandToRun = "hibinfo";
                runCommandCrash();
            }
            else if (commandToRun == "hibinfo")
            {
                commandToRun = "vboxinfo";
                runCommandCrash();
            }
            else if (commandToRun == "vboxinfo")
            {
                commandToRun = "vmwareinfo";
                runCommandCrash();
            }
            else if (commandToRun == "vmwareinfo")
            {
                lineToRunCount++;
                if (lineToRunCount < lineToRun.Count)
                {
                    runCommandCycle();
                }
                else
                {
                    BuildReport.IsEnabled = true;
                    lbl_Progress.Visibility = Visibility.Hidden;
                    lbl_NumberProgress.Visibility = Visibility.Hidden;
                    pbStatus.Visibility = Visibility.Hidden;
                    ProgressCancel.Visibility = Visibility.Hidden;
                    System.IO.Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + @"Saves\HTMLPage");
                    var dlg = new CommonOpenFileDialog();
                    dlg.Title = "Choose Download Folder";
                    dlg.IsFolderPicker = true;
                    dlg.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory + @"Saves\HTMLPage";
                    dlg.AllowNonFileSystemItems = false;
                    dlg.DefaultDirectory = AppDomain.CurrentDomain.BaseDirectory + @"Saves\HTMLPage";
                    dlg.EnsureFileExists = true;
                    dlg.EnsurePathExists = true;
                    dlg.EnsureReadOnly = false;
                    dlg.EnsureValidNames = true;
                    dlg.Multiselect = false;
                    dlg.ShowPlacesList = true;

                    if (dlg.ShowDialog() == CommonFileDialogResult.Ok)
                    {

                        buildWebpage();
                    }
                    else
                    {
                        buildWebpage();
                    }
                }
            }
        }

        void runCommandHive()
        {
            BackgroundWorker bg = new BackgroundWorker();
            //pbStatus.Visibility = Visibility.Visible;
            BuildReport.IsEnabled = false;
            bg.DoWork += new DoWorkEventHandler(MethodToGetInfoHive);
            bg.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bg_RunWorkerCompletedHive);
            //show marquee here
            bg.RunWorkerAsync();
        }

        void MethodToGetInfoHive(Object sender, DoWorkEventArgs args)
        {
            // find system info here
            //System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.FileName = "CMD.exe";
            startInfo.WorkingDirectory = Directory.GetCurrentDirectory();
            startInfo.Arguments = "/c volatility.exe -f " + "\"" + imageRam + "\"" + " --profile=" + imageRamProfile + " hivelist";
            startInfo.CreateNoWindow = true;
            process.StartInfo = startInfo;
            process.Start();
            outputHive = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
        }

        void bg_RunWorkerCompletedHive(object sender, RunWorkerCompletedEventArgs args)
        {
            //this method will be called once background worker has completed it's task
            //hide the marquee
            Regex exp = new Regex(".*SAM.*", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            MatchCollection matchCollection = exp.Matches(outputHive);
            foreach (Match y in matchCollection)
            {
                hiveSAM = y.Value.Split(' ')[0];
            }
            Regex exp1 = new Regex(".*SYSTEM.*", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            MatchCollection matchCollection1 = exp1.Matches(outputHive);
            foreach (Match x in matchCollection1)
            {
                hiveSYSTEM = x.Value.Split(' ')[0];
            }
            commandToRun = "hashdump";
            runCommand();
        }

        private void buildWebpage()
        {
            htmlPage.Add("<!doctype html>");
            htmlPage.Add("<html lang=\"en\" class=\"no - js\">");
            htmlPage.Add("<head>");
            htmlPage.Add("<meta charset=\"utf - 8\">");
            htmlPage.Add("<meta http-equiv=\"x - ua - compatible\" content=\"ie = edge\">");
            htmlPage.Add("<meta name=\"viewport\" content=\"width = device - width, initial - scale = 1.0\">");
            htmlPage.Add("<title>Volatility Report</title>");
            htmlPage.Add("<meta name=\"description\" content=\"*****Volatility Report*****\">");
            htmlPage.Add("<link rel=\"stylesheet\" href=\"https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/css/bootstrap.min.css\">");
            htmlPage.Add("<script src=\"https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js\"></script>");
            htmlPage.Add("<script src=\"https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/js/bootstrap.min.js\"></script>");
            htmlPage.Add("</head>");
            htmlPage.Add("<body>");

            htmlPage.Add("<nav class=\"navbar navbar-inverse navbar-fixed-top\">");
            htmlPage.Add("<div class=\"container-fluid\">");
            htmlPage.Add("<div class=\"navbar-header\">");
            htmlPage.Add("<a class=\"navbar-brand\">My Volatility Report - " + DateTime.UtcNow.ToString("dd-MM-yyyy") + "</a>");
            htmlPage.Add("</div>");
            htmlPage.Add("<ul class=\"nav navbar-nav navbar-right\">");
            htmlPage.Add("<li><a href=\"#ImageInfo\">Image Info</a></li>");
            if (Processes.IsChecked == true || ProcessesScan.IsChecked == true || ProcesseHandles.IsChecked == true || ProcesseDLL.IsChecked == true || ProcesseSID.IsChecked == true || ProcesseCMD.IsChecked == true || ProcessePrivileges.IsChecked == true || ProcesseEnvPrivileges.IsChecked == true || ProcesseVerify.IsChecked == true)
            {
                htmlPage.Add("<li class=\"dropdown\">");
                htmlPage.Add("<a class=\"dropdown-toggle\" data-toggle=\"dropdown\" href=\"#\">Processes and DLLs<span class=\"caret\"></span></a>");
                htmlPage.Add("<ul class=\"dropdown-menu\">");

                if (Processes.IsChecked == true)
                {
                    htmlPage.Add("<li><a href=\"#Processes\">Processes</a></li>");
                }
                if (ProcessesScan.IsChecked == true)
                {
                    htmlPage.Add("<li><a href=\"#ProcessesScan\">Processes Scan</a></li>");
                }
                if (ProcesseHandles.IsChecked == true)
                {
                    htmlPage.Add("<li><a href=\"#ProcesseHandles\">Processe Handles</a></li>");
                }
                if (ProcesseDLL.IsChecked == true)
                {
                    htmlPage.Add("<li><a href=\"#ProcesseDLL\">Processe DLL</a></li>");
                }
                if (ProcesseSID.IsChecked == true)
                {
                    htmlPage.Add("<li><a href=\"#ProcesseSID\">Processe SID</a></li>");
                }
                if (ProcesseCMD.IsChecked == true)
                {
                    htmlPage.Add("<li><a href=\"#ProcesseCMD\">Processe CMD</a></li>");
                }
                if (ProcessePrivileges.IsChecked == true)
                {
                    htmlPage.Add("<li><a href=\"#ProcessePrivileges\">Processe Privileges</a></li>");
                }
                if (ProcesseEnvPrivileges.IsChecked == true)
                {
                    htmlPage.Add("<li><a href=\"#ProcesseEnvPrivileges\">Processe Env Privileges</a></li>");
                }
                if (ProcesseVerify.IsChecked == true)
                {
                    htmlPage.Add("<li><a href=\"#ProcesseVerify\">Processe Verify</a></li>");
                }

                htmlPage.Add("</ul>");
                htmlPage.Add("</li>");
            }
            if (MemMap.IsChecked == true || InternetExplorer.IsChecked == true)
            {
                htmlPage.Add("<li class=\"dropdown\">");
                htmlPage.Add("<a class=\"dropdown-toggle\" data-toggle=\"dropdown\" href=\"#\">Process Memory<span class=\"caret\"></span></a>");
                htmlPage.Add("<ul class=\"dropdown-menu\">");

                if (MemMap.IsChecked == true)
                {
                    htmlPage.Add("<li><a href=\"" + ".\\" + fileName + "Extra\\" + fileName + "MEM.htm" + "\">Mem Map</a></li>");
                }
                if (InternetExplorer.IsChecked == true)
                {
                    htmlPage.Add("<li><a href=\"#InternetExplorer\">Internet Explorer</a></li>");
                }
                htmlPage.Add("</ul>");
                htmlPage.Add("</li>");
            }
            if (KernelDrivers.IsChecked == true || KernelSSDT.IsChecked == true || Drivers.IsChecked == true || FileScan.IsChecked == true || MutantScan.IsChecked == true || SymbolicLinks.IsChecked == true || ETHREADobjects.IsChecked == true || UnloadedModules.IsChecked == true)
            {
                htmlPage.Add("<li class=\"dropdown\">");
                htmlPage.Add("<a class=\"dropdown-toggle\" data-toggle=\"dropdown\" href=\"#\">Kernel Memory and Objects<span class=\"caret\"></span></a>");
                htmlPage.Add("<ul class=\"dropdown-menu\">");

                if (KernelDrivers.IsChecked == true)
                {
                    htmlPage.Add("<li><a href=\"#KernelDrivers\">Kernel Drivers</a></li>");
                }
                if (KernelSSDT.IsChecked == true)
                {
                    htmlPage.Add("<li><a href=\"#KernelSSDT\">Kernel SSDT</a></li>");
                }
                if (Drivers.IsChecked == true)
                {
                    htmlPage.Add("<li><a href=\"#Drivers\">Drivers</a></li>");
                }
                if (FileScan.IsChecked == true)
                {
                    htmlPage.Add("<li><a href=\"#FileScan\">File Scan</a></li>");
                }
                if (MutantScan.IsChecked == true)
                {
                    htmlPage.Add("<li><a href=\"#MutantScan\">Mutant Scan</a></li>");
                }
                if (SymbolicLinks.IsChecked == true)
                {
                    htmlPage.Add("<li><a href=\"#SymbolicLinks\">Symbolic Links</a></li>");
                }
                if (ETHREADobjects.IsChecked == true)
                {
                    htmlPage.Add("<li><a href=\"#ETHREADobjects\">ETHREAD Objects</a></li>");
                }
                if (UnloadedModules.IsChecked == true)
                {
                    htmlPage.Add("<li><a href=\"#UnloadedModules\">Unloaded Modules</a></li>");
                }
                htmlPage.Add("</ul>");
                htmlPage.Add("</li>");
            }
            if (Networking.IsChecked == true)
            {
                htmlPage.Add("<li>");
                htmlPage.Add("<a href=\"#Networking\">Networking</a>");
                htmlPage.Add("</li>");
            }
            if (RegistryHives.IsChecked == true || RegistryKeys.IsChecked == true || RegistryDomain.IsChecked == true || RegistryLSA.IsChecked == true || RegistryUserAssist.IsChecked == true || RegistryShell.IsChecked == true || RegistryShim.IsChecked == true || RegistrySID.IsChecked == true)
            {
                htmlPage.Add("<li class=\"dropdown\">");
                htmlPage.Add("<a class=\"dropdown-toggle\" data-toggle=\"dropdown\" href=\"#\">Registry<span class=\"caret\"></span></a>");
                htmlPage.Add("<ul class=\"dropdown-menu\">");

                if (RegistryHives.IsChecked == true)
                {
                    htmlPage.Add("<li><a href=\"#RegistryHives\">Registry Hives</a></li>");
                }
                if (RegistryKeys.IsChecked == true)
                {
                    htmlPage.Add("<li><a href=\"#RegistryKeys\">Registry Keys</a></li>");
                }
                if (RegistryDomain.IsChecked == true)
                {
                    htmlPage.Add("<li><a href=\"#RegistryDomain\">Registry Domain</a></li>");
                }
                if (RegistryLSA.IsChecked == true)
                {
                    htmlPage.Add("<li><a href=\"#RegistryLSA\">Registry LSA</a></li>");
                }
                if (RegistryUserAssist.IsChecked == true)
                {
                    htmlPage.Add("<li><a href=\"#RegistryUserAssist\">Registry User Assist</a></li>");
                }
                if (RegistryShell.IsChecked == true)
                {
                    htmlPage.Add("<li><a href=\"#RegistryShell\">Registry Shell</a></li>");
                }
                if (RegistryShim.IsChecked == true)
                {
                    htmlPage.Add("<li><a href=\"#RegistryShim\">Registry Shim</a></li>");
                }
                if (RegistrySID.IsChecked == true)
                {
                    htmlPage.Add("<li><a href=\"#RegistrySID\">Registry SID</a></li>");
                }
                htmlPage.Add("</ul>");
                htmlPage.Add("</li>");
            }
            if (FileMFT.IsChecked == true)
            {
                htmlPage.Add("<li>");
                htmlPage.Add("<li><a href=\"" + ".\\" + fileName + "Extra\\" + fileName + "MFT.htm" + "\">File System</a>");
                htmlPage.Add("</li>");
            }
            if (BIOS.IsChecked == true || Timeliner.IsChecked == true)
            {

                htmlPage.Add("<li class=\"dropdown\">");
                htmlPage.Add("<a class=\"dropdown-toggle\" data-toggle=\"dropdown\" href=\"#\">Miscellaneous<span class=\"caret\"></span></a>");
                htmlPage.Add("<ul class=\"dropdown-menu\">");

                if (BIOS.IsChecked == true)
                {
                    htmlPage.Add("<li><a href=\"#BIOS\">BIOS</a></li>");
                }
                if (Timeliner.IsChecked == true)
                {
                    htmlPage.Add("<li><a href=\"#Timeliner\">Timeliner</a></li>");
                }
                htmlPage.Add("</ul>");
                htmlPage.Add("</li>");
            }
            if (CrashInfo.IsChecked == true)
            {
                htmlPage.Add("<li>");
                htmlPage.Add("<a href=\"#SystemPower\">System Power</a>");
                htmlPage.Add("</li>");
            }
            if (Sessions.IsChecked == true || WNDObjects.IsChecked == true || DeskScan.IsChecked == true || Atom.IsChecked == true || Clipboard.IsChecked == true || EventHooks.IsChecked == true || gahti.IsChecked == true || MessageHooks.IsChecked == true || UserHandles.IsChecked == true || gditimers.IsChecked == true || DesktopWindows.IsChecked == true)
            {
                htmlPage.Add("<li class=\"dropdown\">");
                htmlPage.Add("<a class=\"dropdown-toggle\" data-toggle=\"dropdown\" href=\"#\">GUI<span class=\"caret\"></span></a>");
                htmlPage.Add("<ul class=\"dropdown-menu\">");

                if (Sessions.IsChecked == true)
                {
                    htmlPage.Add("<li><a href=\"#Sessions\">Sessions</a></li>");
                }
                if (WNDObjects.IsChecked == true)
                {
                    htmlPage.Add("<li><a href=\"#WNDObjects\">WND Objects</a></li>");
                }
                if (DeskScan.IsChecked == true)
                {
                    htmlPage.Add("<li><a href=\"#DeskScan\">Desk Scan</a></li>");
                }
                if (Atom.IsChecked == true)
                {
                    htmlPage.Add("<li><a href=\"#Atom\">Atom</a></li>");
                }
                if (Clipboard.IsChecked == true)
                {
                    htmlPage.Add("<li><a href=\"#Clipboard\">Clipboard</a></li>");
                }
                if (EventHooks.IsChecked == true)
                {
                    htmlPage.Add("<li><a href=\"#EventHooks\">Event Hooks</a></li>");
                }
                if (gahti.IsChecked == true)
                {
                    htmlPage.Add("<li><a href=\"#gahti\">gahti</a></li>");
                }
                if (MessageHooks.IsChecked == true)
                {
                    htmlPage.Add("<li><a href=\"#MessageHooks\">Message Hooks</a></li>");
                }
                if (UserHandles.IsChecked == true)
                {
                    htmlPage.Add("<li><a href=\"#UserHandles\">User Handles</a></li>");
                }
                if (gditimers.IsChecked == true)
                {
                    htmlPage.Add("<li><a href=\"#gditimers\">gditimers</a></li>");
                }
                if (DesktopWindows.IsChecked == true)
                {
                    htmlPage.Add("<li><a href=\"#DesktopWindows\">Desktop Windows</a></li>");
                }
                htmlPage.Add("</ul>");
                htmlPage.Add("</li>");
            }
            if (malfind.IsChecked == true || services.IsChecked == true || ldrmodules.IsChecked == true || apihooks.IsChecked == true || idt.IsChecked == true || gdt.IsChecked == true || threads.IsChecked == true || Callbacks.IsChecked == true || driverirp.IsChecked == true || timers.IsChecked == true)
            {
                htmlPage.Add("<li class=\"dropdown\">");
                htmlPage.Add("<a class=\"dropdown-toggle\" data-toggle=\"dropdown\" href=\"#\">Malware<span class=\"caret\"></span></a>");
                htmlPage.Add("<ul class=\"dropdown-menu\">");

                if (malfind.IsChecked == true)
                {
                    htmlPage.Add("<li><a href=\"#malfind\">Malware Find</a></li>");
                }
                if (services.IsChecked == true)
                {
                    htmlPage.Add("<li><a href=\"#services\">Services</a></li>");
                }
                if (ldrmodules.IsChecked == true)
                {
                    htmlPage.Add("<li><a href=\"#ldrmodules\">LDR Modules</a></li>");
                }
                if (apihooks.IsChecked == true)
                {
                    htmlPage.Add("<li><a href=\"#apihooks\">API Hooks</a></li>");
                }
                if (idt.IsChecked == true)
                {
                    htmlPage.Add("<li><a href=\"#idt\">IDT</a></li>");
                }
                if (gdt.IsChecked == true)
                {
                    htmlPage.Add("<li><a href=\"#gdt\">GDT</a></li>");
                }
                if (threads.IsChecked == true)
                {
                    htmlPage.Add("<li><a href=\"#threads\">Threads</a></li>");
                }
                if (Callbacks.IsChecked == true)
                {
                    htmlPage.Add("<li><a href=\"#Callbacks\">Callbacks</a></li>");
                }
                if (driverirp.IsChecked == true)
                {
                    htmlPage.Add("<li><a href=\"#driverirp\">Driver IRP</a></li>");
                }
                if (timers.IsChecked == true)
                {
                    htmlPage.Add("<li><a href=\"#timers\">Timers</a></li>");
                }
                htmlPage.Add("</ul>");
                htmlPage.Add("</li>");
            }
            if (Conclusion.IsChecked == true)
            {
                htmlPage.Add("<li>");
                htmlPage.Add("<a href=\"#Conclusion\">Conclusion</a>");
                htmlPage.Add("</li>");
            }
            htmlPage.Add("</ul>");
            htmlPage.Add("</div>");
            htmlPage.Add("</nav>");
            htmlPage.Add("<br>");
            htmlPage.Add("<br>");

            htmlPage.Add("<div class=\"container\">  ");
            if (output1 != null)
            {
                htmlPage.Add("<div class=\"row\">");
                htmlPage.Add("<div id=\"ImageInfo\" class=\"col-lg-12\">");
                htmlPage.Add("<h2 class=\"page-header\">Image Info</h2>");
                htmlPage.Add("</div>");
                htmlPage.Add("<div class=\"col-md-12\">");
                string[] outputArray = output1.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                foreach (string aux in outputArray)
                {
                    //teste
                    if (aux.Contains(":"))
                    {
                        string[] auxArray = aux.Split(new[] { ':' }, 2);
                        htmlPage.Add("<p><b>" + auxArray[0] + ":</b>");
                        htmlPage.Add(auxArray[1] + "</p>");
                    }
                    else
                    {
                        htmlPage.Add("<p>" + aux + "</p>");
                    }
                    //htmlPage.Add("<p>" + aux + "</p>");
                }
                //htmlPage.Add(output1);
                /*htmlPage.Add("<p>Values:</p>");
                htmlPage.Add("<ul>");
                htmlPage.Add("<li><strong>Bootstrap v3.3.7</strong>");
                htmlPage.Add("</li>");
                htmlPage.Add("<li>jQuery v1.11.1</li>");
                htmlPage.Add("</ul>");
                htmlPage.Add("</div>");
                */
                htmlPage.Add("</div>");
                htmlPage.Add("<hr>");
            }
            if (output2 != null && Processes.IsChecked == true)
            {
                htmlPage.Add("<div id=\"Processes\" class=\"col-lg-12\">");
                htmlPage.Add("<h2 class=\"page-header\">Processes List</h2>");
                htmlPage.Add("</div>");
                htmlPage.Add("<div class=\"col-md-12\">");
                //a cada nova linha meter texto
                string[] outputArray = output2.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                int countAUX = 0;
                foreach (string aux in outputArray)
                {//cabelhaço a negrito
                    if (countAUX < 2)
                    {
                        htmlPage.Add("<p><b>" + aux + "</b></p>");
                    }
                    else
                    {
                        htmlPage.Add("<p>" + aux + "</p>");
                    }
                    countAUX++;
                }
                htmlPage.Add("</div>");
                htmlPage.Add("<hr>");
            }
            if (output3 != null)
            {
                htmlPage.Add("<div id=\"ProcessesScan\" class=\"col-lg-12\">");
                htmlPage.Add("<h2 class=\"page-header\">Processes Scan List (w/ Hidden Processes)</h2>");
                htmlPage.Add("</div>");
                htmlPage.Add("<div class=\"col-md-12\">");
                string[] outputArray = output3.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                int countAUX = 0;
                foreach (string aux in outputArray)
                {
                    if (countAUX < 2)
                    {
                        htmlPage.Add("<p><b>" + aux + "</b></p>");
                    }
                    else
                    {
                        htmlPage.Add("<p>" + aux + "</p>");
                    }
                    countAUX++;
                }
                htmlPage.Add("</div>");
                htmlPage.Add("<hr>");
            }
            if (output4 != null)
            {
                htmlPage.Add("<div id=\"ProcesseHandles\" class=\"col-lg-12\">");
                htmlPage.Add("<h2 class=\"page-header\">Process Handles</h2>");
                htmlPage.Add("</div>");
                htmlPage.Add("<div class=\"col-md-12\">");
                string[] outputArray = output4.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                int countAUX = 0;
                foreach (string aux in outputArray)
                {
                    if (countAUX < 2)
                    {
                        htmlPage.Add("<p><b>" + aux + "</b></p>");
                    }
                    else
                    {
                        htmlPage.Add("<p>" + aux + "</p>");
                    }
                    countAUX++;
                }
                htmlPage.Add("</div>");
                htmlPage.Add("<hr>");
            }
            if (output5 != null && ProcesseDLL.IsChecked == true)
            {
                htmlPage.Add("<div id=\"ProcesseDLL\" class=\"col-lg-12\">");
                htmlPage.Add("<h2 class=\"page-header\">Process DLLs List</h2>");
                htmlPage.Add("</div>");
                htmlPage.Add("<div class=\"col-md-12\">");
                string[] outputArray = output5.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                foreach (string aux in outputArray)
                {
                    if (aux.Contains("pid:") || aux.Contains("Base                             Size          LoadCount Path") || aux.Contains("------------------ ------------------ ------------------ ----"))
                    {
                        htmlPage.Add("<p><b>" + aux + "</b></p>");
                    }
                    else
                    {
                        htmlPage.Add("<p>" + aux + "</p>");
                    }
                }
                htmlPage.Add("</div>");
                htmlPage.Add("<hr>");
            }
            if (output6 != null)
            {
                htmlPage.Add("<div id=\"ProcesseSID\" class=\"col-lg-12\">");
                htmlPage.Add("<h2 class=\"page-header\">Security Identifiers</h2>");
                htmlPage.Add("</div>");
                htmlPage.Add("<div class=\"col-md-12\">");
                string[] outputArray = output6.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                foreach (string aux in outputArray)
                {
                    if (aux.Contains(":"))
                    {
                        string[] auxArray = aux.Split(new[] { ':' }, 2);
                        htmlPage.Add("<p><b>" + auxArray[0] + ":</b>");
                        htmlPage.Add(auxArray[1] + "</p>");
                    }
                    else
                    {
                        htmlPage.Add("<p>" + aux + "</p>");
                    }
                }
                htmlPage.Add("</div>");
                htmlPage.Add("<hr>");
            }
            if (output7 != null)
            {
                htmlPage.Add("<div id=\"ProcesseCMD\" class=\"col-lg-12\">");
                htmlPage.Add("<h2 class=\"page-header\">Process CMD.exe Commands</h2>");
                htmlPage.Add("</div>");
                htmlPage.Add("<div class=\"col-md-12\">");
                string[] outputArray = output7.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                int countAUX = 0;
                foreach (string aux in outputArray)
                {
                    if (countAUX < 2)
                    {
                        htmlPage.Add("<p><b>" + aux + "</b></p>");
                    }
                    else
                    {
                        htmlPage.Add("<p>" + aux + "</p>");
                    }
                    countAUX++;
                }
                htmlPage.Add("</div>");
                htmlPage.Add("<hr>");
            }
            if (output8 != null && ProcessePrivileges.IsChecked == true)
            {
                htmlPage.Add("<div id=\"ProcessePrivileges\" class=\"col-lg-12\">");
                htmlPage.Add("<h2 class=\"page-header\">Process Privileges List</h2>");
                htmlPage.Add("</div>");
                htmlPage.Add("<div class=\"col-md-12\">");
                string[] outputArray = output8.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                int countAUX = 0;
                foreach (string aux in outputArray)
                {
                    if (countAUX < 2)
                    {
                        htmlPage.Add("<p><b>" + aux + "</b></p>");
                    }
                    else
                    {
                        htmlPage.Add("<p>" + aux + "</p>");
                    }
                    countAUX++;
                }
                htmlPage.Add("</div>");
                htmlPage.Add("<hr>");
            }
            if (output9 != null && ProcesseEnvPrivileges.IsChecked == true)
            {
                htmlPage.Add("<div id=\"ProcesseEnvPrivileges\" class=\"col-lg-12\">");
                htmlPage.Add("<h2 class=\"page-header\">Process Enviorment Privileges</h2>");
                htmlPage.Add("</div>");
                htmlPage.Add("<div class=\"col-md-12\">");
                string[] outputArray = output9.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                int countAUX = 0;
                foreach (string aux in outputArray)
                {
                    if (countAUX < 2)
                    {
                        htmlPage.Add("<p><b>" + aux + "</b></p>");
                    }
                    else
                    {
                        htmlPage.Add("<p>" + aux + "</p>");
                    }
                    countAUX++;
                }
                htmlPage.Add("</div>");
                htmlPage.Add("<hr>");
            }
            if (output10 != null)
            {
                htmlPage.Add("<div id=\"ProcesseVerify\" class=\"col-lg-12\">");
                htmlPage.Add("<h2 class=\"page-header\">Process Info Verification</h2>");
                htmlPage.Add("</div>");
                htmlPage.Add("<div class=\"col-md-12\">");
                string[] outputArray = output10.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                foreach (string aux in outputArray)
                {
                    if (aux.Contains(":"))
                    {
                        string[] auxArray = aux.Split(new[] { ':' }, 2);
                        htmlPage.Add("<p><b>" + auxArray[0] + ":</b>");
                        htmlPage.Add(auxArray[1] + "</p>");
                    }
                    else
                    {
                        htmlPage.Add("<p><b>" + aux + "</b></p>");
                    }
                }
                htmlPage.Add("</div>");
                htmlPage.Add("<hr>");
            }
            if (output11 != null)
            {
                htmlPageMEM.Add("<!doctype html>");
                htmlPageMEM.Add("<html lang=\"en\" class=\"no - js\">");
                htmlPageMEM.Add("<head>");
                htmlPageMEM.Add("<meta charset=\"utf - 8\">");
                htmlPageMEM.Add("<meta http-equiv=\"x - ua - compatible\" content=\"ie = edge\">");
                htmlPageMEM.Add("<meta name=\"viewport\" content=\"width = device - width, initial - scale = 1.0\">");
                htmlPageMEM.Add("<title>Volatility Report</title>");
                htmlPageMEM.Add("<meta name=\"description\" content=\"*****Volatility Report*****\">");
                htmlPageMEM.Add("<link rel=\"stylesheet\" href=\"https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/css/bootstrap.min.css\">");
                htmlPageMEM.Add("<script src=\"https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js\"></script>");
                htmlPageMEM.Add("<script src=\"https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/js/bootstrap.min.js\"></script>");
                htmlPageMEM.Add("</head>");
                htmlPageMEM.Add("<body>");
                htmlPageMEM.Add("<nav class=\"navbar navbar-inverse navbar-fixed-top\">");
                htmlPageMEM.Add("<div class=\"container-fluid\">");
                htmlPageMEM.Add("<div class=\"navbar-header\">");
                htmlPageMEM.Add("<a class=\"navbar-brand\">My Volatility Report - " + DateTime.UtcNow.ToString("dd-MM-yyyy") + "</a>");
                htmlPageMEM.Add("</div>");
                htmlPageMEM.Add("<ul class=\"nav navbar-nav navbar-right\">");
                htmlPageMEM.Add("<li>");
                htmlPageMEM.Add("<a href=\"" + "..\\" + fileName + ".htm" + "\">Home</a>");
                htmlPageMEM.Add("</li>");
                htmlPageMEM.Add("<li><a href=\"" + fileName + "MEM.htm" + "\">Mem Map</a></li>");
                if (FileMFT.IsChecked == true)
                {
                    htmlPageMEM.Add("<li><a href=\"" + fileName + "MFT.htm" + "\">File System</a></li>");
                }
                htmlPageMEM.Add("</ul>");
                htmlPageMEM.Add("</div>");
                htmlPageMEM.Add("</nav>");
                htmlPageMEM.Add("<br>");
                htmlPageMEM.Add("<br>");
                htmlPageMEM.Add("<div class=\"container\">  ");
                //output info
                htmlPageMEM.Add("<div class=\"col-lg-12\">");
                htmlPageMEM.Add("<h2 class=\"page-header\">Memory Pagination</h2>");
                htmlPageMEM.Add("</div>");
                htmlPageMEM.Add("<div class=\"col-md-12\">");
                string[] outputArray = output11.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                foreach (string aux in outputArray)
                {
                    if (aux.Contains("pid:") || aux.Contains("------------------") || aux.Contains("Virtual"))
                    {
                        htmlPage.Add("<p><b>" + aux + ":</b></p>");
                    }
                    else
                    {
                        htmlPage.Add("<p>" + aux + "</p>");
                    }
                }
                htmlPageMEM.Add("</div>");
                htmlPageMEM.Add("<hr>");

                htmlPageMEM.Add("<p>");
                htmlPageMEM.Add("<p>");
                htmlPageMEM.Add("<footer>");
                htmlPageMEM.Add("<div class=\"row\">");
                htmlPageMEM.Add("<div class=\"col-lg-12\">");
                htmlPageMEM.Add("<p>VolatilityEasyReport , " + DateTime.UtcNow.ToString("yyyy") + " - " + System.Security.Principal.WindowsIdentity.GetCurrent().Name + "</p>");
                htmlPageMEM.Add("</div>");
                htmlPageMEM.Add("</div>");
                htmlPageMEM.Add("</footer>");
                htmlPageMEM.Add("</div>");
                htmlPageMEM.Add("</body>");
                htmlPageMEM.Add("</html>");
            }
            if (output12 != null)
            {
                htmlPage.Add("<div id=\"InternetExplorer\" class=\"col-lg-12\">");
                htmlPage.Add("<h2 class=\"page-header\">Internet Explorer History</h2>");
                htmlPage.Add("</div>");
                htmlPage.Add("<div class=\"col-md-12\">");
                string[] outputArray = output12.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                int countAUX = 0;
                foreach (string aux in outputArray)
                {
                    if (countAUX < 2)
                    {
                        htmlPage.Add("<p><b>" + aux + "</b></p>");
                    }
                    else
                    {
                        htmlPage.Add("<p>" + aux + "</p>");
                    }
                    countAUX++;
                }
                htmlPage.Add("</div>");
                htmlPage.Add("<hr>");
            }
            if (output13 != null)
            {
                htmlPage.Add("<div id=\"KernelDrivers\" class=\"col-lg-12\">");
                htmlPage.Add("<h2 class=\"page-header\">Kernel Drivers</h2>");
                htmlPage.Add("</div>");
                htmlPage.Add("<div class=\"col-md-12\">");
                string[] outputArray = output13.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                int countAUX = 0;
                foreach (string aux in outputArray)
                {
                    if (countAUX < 2)
                    {
                        htmlPage.Add("<p><b>" + aux + "</b></p>");
                    }
                    else
                    {
                        htmlPage.Add("<p>" + aux + "</p>");
                    }
                    countAUX++;
                }
                htmlPage.Add("</div>");
                htmlPage.Add("<hr>");
            }
            if (output14 != null)
            {
                htmlPage.Add("<div id=\"KernelSSDT\" class=\"col-lg-12\">");
                htmlPage.Add("<h2 class=\"page-header\">System Service Description Table</h2>");
                htmlPage.Add("</div>");
                htmlPage.Add("<div class=\"col-md-12\">");
                string[] outputArray = output14.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                int countAUX = 0;
                foreach (string aux in outputArray)
                {
                    if (countAUX < 2)
                    {
                        htmlPage.Add("<p><b>" + aux + "</b></p>");
                    }
                    else
                    {
                        htmlPage.Add("<p>" + aux + "</p>");
                    }
                    countAUX++;
                }
                htmlPage.Add("</div>");
                htmlPage.Add("<hr>");
            }
            if (output15 != null)
            {
                htmlPage.Add("<div id=\"Drivers\" class=\"col-lg-12\">");
                htmlPage.Add("<h2 class=\"page-header\">Drivers List</h2>");
                htmlPage.Add("</div>");
                htmlPage.Add("<div class=\"col-md-12\">");
                string[] outputArray = output15.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                int countAUX = 0;
                foreach (string aux in outputArray)
                {
                    if (countAUX < 2)
                    {
                        htmlPage.Add("<p><b>" + aux + "</b></p>");
                    }
                    else
                    {
                        htmlPage.Add("<p>" + aux + "</p>");
                    }
                    countAUX++;
                }
                htmlPage.Add("</div>");
                htmlPage.Add("<hr>");
            }
            if (output16 != null)
            {
                htmlPage.Add("<div id=\"FileScan\" class=\"col-lg-12\">");
                htmlPage.Add("<h2 class=\"page-header\">File Scan</h2>");
                htmlPage.Add("</div>");
                htmlPage.Add("<div class=\"col-md-12\">");
                string[] outputArray = output16.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                int countAUX = 0;
                foreach (string aux in outputArray)
                {
                    if (countAUX < 2)
                    {
                        htmlPage.Add("<p><b>" + aux + "</b></p>");
                    }
                    else
                    {
                        htmlPage.Add("<p>" + aux + "</p>");
                    }
                    countAUX++;
                }
                htmlPage.Add("</div>");
                htmlPage.Add("<hr>");
            }
            if (output17 != null)
            {
                htmlPage.Add("<div id=\"MutantScan\" class=\"col-lg-12\">");
                htmlPage.Add("<h2 class=\"page-header\">Mutant Scan</h2>");
                htmlPage.Add("</div>");
                htmlPage.Add("<div class=\"col-md-12\">");
                string[] outputArray = output17.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                int countAUX = 0;
                foreach (string aux in outputArray)
                {
                    if (countAUX < 2)
                    {
                        htmlPage.Add("<p><b>" + aux + "</b></p>");
                    }
                    else
                    {
                        htmlPage.Add("<p>" + aux + "</p>");
                    }
                    countAUX++;
                }
                htmlPage.Add("</div>");
                htmlPage.Add("<hr>");
            }
            if (output18 != null)
            {
                htmlPage.Add("<div id=\"SymbolicLinks\" class=\"col-lg-12\">");
                htmlPage.Add("<h2 class=\"page-header\">Symbolic Links</h2>");
                htmlPage.Add("</div>");
                htmlPage.Add("<div class=\"col-md-12\">");
                string[] outputArray = output18.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                int countAUX = 0;
                foreach (string aux in outputArray)
                {
                    if (countAUX < 2)
                    {
                        htmlPage.Add("<p><b>" + aux + "</b></p>");
                    }
                    else
                    {
                        htmlPage.Add("<p>" + aux + "</p>");
                    }
                    countAUX++;
                }
                htmlPage.Add("</div>");
                htmlPage.Add("<hr>");
            }
            if (output19 != null)
            {
                htmlPage.Add("<div id=\"ETHREADobjects\" class=\"col-lg-12\">");
                htmlPage.Add("<h2 class=\"page-header\">ETHREAD objects</h2>");
                htmlPage.Add("</div>");
                htmlPage.Add("<div class=\"col-md-12\">");
                string[] outputArray = output19.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                int countAUX = 0;
                foreach (string aux in outputArray)
                {
                    if (countAUX < 2)
                    {
                        htmlPage.Add("<p><b>" + aux + "</b></p>");
                    }
                    else
                    {
                        htmlPage.Add("<p>" + aux + "</p>");
                    }
                    countAUX++;
                }
                htmlPage.Add("</div>");
                htmlPage.Add("<hr>");
            }
            if (output20 != null)
            {
                htmlPage.Add("<div id=\"UnloadedModules\" class=\"col-lg-12\">");
                htmlPage.Add("<h2 class=\"page-header\">Unloaded Modules</h2>");
                htmlPage.Add("</div>");
                htmlPage.Add("<div class=\"col-md-12\">");
                string[] outputArray = output20.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                int countAUX = 0;
                foreach (string aux in outputArray)
                {
                    if (countAUX < 2)
                    {
                        htmlPage.Add("<p><b>" + aux + "</b></p>");
                    }
                    else
                    {
                        htmlPage.Add("<p>" + aux + "</p>");
                    }
                    countAUX++;
                }
                htmlPage.Add("</div>");
                htmlPage.Add("<hr>");
            }
            if (output21 != null && Networking.IsChecked == true)
            {
                htmlPage.Add("<div id=\"Networking\" class=\"col-lg-12\">");
                htmlPage.Add("<h2 class=\"page-header\">Networking</h2>");
                htmlPage.Add("</div>");
                htmlPage.Add("<div class=\"col-md-12\">");
                string[] outputArray = output21.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                int countAUX = 0;
                foreach (string aux in outputArray)
                {
                    if (countAUX < 5)
                    {
                        htmlPage.Add("<p><b>" + aux + "</b></p>");
                    }
                    else
                    {
                        htmlPage.Add("<p>" + aux + "</p>");
                    }
                    countAUX++;
                }
                htmlPage.Add("</div>");
                htmlPage.Add("<hr>");
            }
            if (output22 != null)
            {
                htmlPage.Add("<div id=\"RegistryHives\" class=\"col-lg-12\">");
                htmlPage.Add("<h2 class=\"page-header\">Virtual Adresses of Registry Hives</h2>");
                htmlPage.Add("</div>");
                htmlPage.Add("<div class=\"col-md-12\">");
                string[] outputArray = output22.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                int countAUX = 0;
                foreach (string aux in outputArray)
                {
                    if (countAUX < 2)
                    {
                        htmlPage.Add("<p><b>" + aux + "</b></p>");
                    }
                    else
                    {
                        htmlPage.Add("<p>" + aux + "</p>");
                    }
                    countAUX++;
                }
                htmlPage.Add("</div>");
                htmlPage.Add("<hr>");
            }
            if (output23 != null)
            {
                htmlPage.Add("<div id=\"RegistryKeys\" class=\"col-lg-12\">");
                htmlPage.Add("<h2 class=\"page-header\">Registry Keys</h2>");
                htmlPage.Add("</div>");
                htmlPage.Add("<div class=\"col-md-12\">");
                string[] outputArray = output23.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                int countAUX = 0;
                foreach (string aux in outputArray)
                {
                    if (countAUX < 2)
                    {
                        htmlPage.Add("<p><b>" + aux + "</b></p>");
                    }
                    else
                    {
                        htmlPage.Add("<p>" + aux + "</p>");
                    }
                    countAUX++;
                }
                htmlPage.Add("</div>");
                htmlPage.Add("<hr>");
            }
            if (output24 != null)
            {
                htmlPage.Add("<div id=\"RegistryDomain\" class=\"col-lg-12\">");
                htmlPage.Add("<h2 class=\"page-header\">Accounts/Domain Credentials</h2>");
                htmlPage.Add("</div>");
                htmlPage.Add("<div class=\"col-md-12\">");
                string[] outputArray = output24.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                foreach (string aux in outputArray)
                {
                    if (aux.Contains(":"))
                    {
                        string[] auxArray = aux.Split(new[] { ':' }, 2);
                        htmlPage.Add("<p><b>" + auxArray[0] + ":</b>");
                        htmlPage.Add(auxArray[1] + "</p>");
                    }
                    else
                    {
                        htmlPage.Add("<p>" + aux + "</p>");
                    }
                }
                htmlPage.Add("</div>");
                htmlPage.Add("<hr>");
            }
            if (output25 != null)
            {
                htmlPage.Add("<div id=\"RegistryLSA\" class=\"col-lg-12\">");
                htmlPage.Add("<h2 class=\"page-header\">Local Security Authority</h2>");
                htmlPage.Add("</div>");
                htmlPage.Add("<div class=\"col-md-12\">");
                string[] outputArray = output25.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                foreach (string aux in outputArray)
                {
                    if (aux.Contains("0x0"))
                    {
                        htmlPage.Add("<p>" + aux + "</p>");
                    }
                    else
                    {
                        htmlPage.Add("<p><b>" + aux + ":</b></p>");
                    }
                }
                htmlPage.Add("</div>");
                htmlPage.Add("<hr>");
            }
            if (output26 != null)
            {
                htmlPage.Add("<div id=\"RegistryUserAssist\" class=\"col-lg-12\">");
                htmlPage.Add("<h2 class=\"page-header\">User Assist Keys</h2>");
                htmlPage.Add("</div>");
                htmlPage.Add("<div class=\"col-md-12\">");
                string[] outputArray = output26.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                foreach (string aux in outputArray)
                {
                    if (aux.Contains("pid:"))
                    {
                        htmlPage.Add("<p><b>" + aux + ":</b></p>");
                    }
                    else
                    {
                        htmlPage.Add("<p>" + aux + "</p>");
                    }
                }
                htmlPage.Add("</div>");
                htmlPage.Add("<hr>");
            }
            if (output27 != null)
            {
                htmlPage.Add("<div id=\"RegistryShell\" class=\"col-lg-12\">");
                htmlPage.Add("<h2 class=\"page-header\">Shellbags</h2>");
                htmlPage.Add("</div>");
                htmlPage.Add("<div class=\"col-md-12\">");
                string[] outputArray = output27.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                foreach (string aux in outputArray)
                {
                    if (aux.Contains("Registry:") || aux.Contains("Key:") || aux.Contains("Last updated:") || aux.Contains("Value                     File") || aux.Contains("------------------------- -------------- -"))
                    {
                        htmlPage.Add("<p><b>" + aux + ":</b></p>");
                    }
                    else
                    {
                        htmlPage.Add("<p>" + aux + "</p>");
                    }
                }
                htmlPage.Add("</div>");
                htmlPage.Add("<hr>");
            }
            if (output28 != null)
            {
                htmlPage.Add("<div id=\"RegistryShim\" class=\"col-lg-12\">");
                htmlPage.Add("<h2 class=\"page-header\">Application Compability Shim Cache</h2>");
                htmlPage.Add("</div>");
                htmlPage.Add("<div class=\"col-md-12\">");
                string[] outputArray = output28.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                int countAUX = 0;
                foreach (string aux in outputArray)
                {
                    if (countAUX < 2)
                    {
                        htmlPage.Add("<p><b>" + aux + "</b></p>");
                    }
                    else
                    {
                        htmlPage.Add("<p>" + aux + "</p>");
                    }
                    countAUX++;
                }
                htmlPage.Add("</div>");
                htmlPage.Add("<hr>");
            }
            if (output29 != null)
            {
                htmlPage.Add("<div id=\"RegistrySID\" class=\"col-lg-12\">");
                htmlPage.Add("<h2 class=\"page-header\">Service SIDs</h2>");
                htmlPage.Add("</div>");
                htmlPage.Add("<div class=\"col-md-12\">");
                string[] outputArray = output29.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                int countAUX = 0;
                foreach (string aux in outputArray)
                {
                    if (countAUX < 1)
                    {
                        htmlPage.Add("<p><b>" + aux + "</b></p>");
                    }
                    else
                    {
                        htmlPage.Add("<p>" + aux + "</p>");
                    }
                    countAUX++;
                }
                htmlPage.Add("</div>");
                htmlPage.Add("<hr>");
            }
            if (output30 != null)
            {
                htmlPageMFT.Add("<!doctype html>");
                htmlPageMFT.Add("<html lang=\"en\" class=\"no - js\">");
                htmlPageMFT.Add("<head>");
                htmlPageMFT.Add("<meta charset=\"utf - 8\">");
                htmlPageMFT.Add("<meta http-equiv=\"x - ua - compatible\" content=\"ie = edge\">");
                htmlPageMFT.Add("<meta name=\"viewport\" content=\"width = device - width, initial - scale = 1.0\">");
                htmlPageMFT.Add("<title>Volatility Report</title>");
                htmlPageMFT.Add("<meta name=\"description\" content=\"*****Volatility Report*****\">");
                htmlPageMFT.Add("<link rel=\"stylesheet\" href=\"https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/css/bootstrap.min.css\">");
                htmlPageMFT.Add("<script src=\"https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js\"></script>");
                htmlPageMFT.Add("<script src=\"https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/js/bootstrap.min.js\"></script>");
                htmlPageMFT.Add("</head>");
                htmlPageMFT.Add("<body>");
                htmlPageMFT.Add("<nav class=\"navbar navbar-inverse navbar-fixed-top\">");
                htmlPageMFT.Add("<div class=\"container-fluid\">");
                htmlPageMFT.Add("<div class=\"navbar-header\">");
                htmlPageMFT.Add("<a class=\"navbar-brand\">My Volatility Report - " + DateTime.UtcNow.ToString("dd-MM-yyyy") + "</a>");
                htmlPageMFT.Add("</div>");
                htmlPageMFT.Add("<ul class=\"nav navbar-nav navbar-right\">");
                htmlPageMFT.Add("<li>");
                htmlPageMFT.Add("<a href=\"" + "..\\" + fileName + ".htm" + "\">Home</a>");
                htmlPageMFT.Add("</li>");
                if (MemMap.IsChecked == true)
                {
                    htmlPageMFT.Add("<li><a href=\"" + fileName + "MEM.htm" + "\">Memory Map</a></li>");
                }
                htmlPageMFT.Add("<li><a href=\"" + fileName + "MFT.htm" + "\">File System</a></li>");
                htmlPageMFT.Add("</ul>");
                htmlPageMFT.Add("</div>");
                htmlPageMFT.Add("</nav>");
                htmlPageMFT.Add("<br>");
                htmlPageMFT.Add("<br>");
                htmlPageMFT.Add("<div class=\"container\">  ");
                //output info
                htmlPageMFT.Add("<div  class=\"col-lg-12\">");
                htmlPageMFT.Add("<h2 class=\"page-header\">Master File Table</h2>");
                htmlPageMFT.Add("</div>");
                htmlPageMFT.Add("<div class=\"col-md-12\">");
                string[] outputArray = output30.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                foreach (string aux in outputArray)
                {
                    htmlPageMFT.Add("<p>" + aux + "</p>");
                }
                htmlPageMFT.Add("</div>");
                htmlPageMFT.Add("<hr>");

                htmlPageMFT.Add("<p>");
                htmlPageMFT.Add("<p>");
                htmlPageMFT.Add("<footer>");
                htmlPageMFT.Add("<div class=\"row\">");
                htmlPageMFT.Add("<div class=\"col-lg-12\">");
                htmlPageMFT.Add("<p>VolatilityEasyReport , " + DateTime.UtcNow.ToString("yyyy") + " - " + System.Security.Principal.WindowsIdentity.GetCurrent().Name + "</p>");
                htmlPageMFT.Add("</div>");
                htmlPageMFT.Add("</div>");
                htmlPageMFT.Add("</footer>");
                htmlPageMFT.Add("</div>");
                htmlPageMFT.Add("</body>");
                htmlPageMFT.Add("</html>");
            }
            if (output31 != null)
            {
                htmlPage.Add("<div id=\"BIOS\" class=\"col-lg-12\">");
                htmlPage.Add("<h2 class=\"page-header\">BIOS Keystrokes</h2>");
                htmlPage.Add("</div>");
                htmlPage.Add("<div class=\"col-md-12\">");
                string[] outputArray = output31.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                int countAUX = 0;
                foreach (string aux in outputArray)
                {
                    if (countAUX < 1)
                    {
                        htmlPage.Add("<p><b>" + aux + "</b></p>");
                    }
                    else
                    {
                        htmlPage.Add("<p>" + aux + "</p>");
                    }
                    countAUX++;
                }
                htmlPage.Add("</div>");
                htmlPage.Add("<hr>");
            }
            if (output32 != null)
            {
                htmlPage.Add("<div id=\"Timeliner\" class=\"col-lg-12\">");
                htmlPage.Add("<h2 class=\"page-header\">Artifacts Timeliner</h2>");
                htmlPage.Add("</div>");
                htmlPage.Add("<div class=\"col-md-12\">");
                string[] outputArray = output32.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                int countAUX = 0;
                foreach (string aux in outputArray)
                {
                    if (countAUX < 2)
                    {
                        htmlPage.Add("<p><b>" + aux + "</b></p>");
                    }
                    else
                    {
                        htmlPage.Add("<p>" + aux + "</p>");
                    }
                    countAUX++;
                }
                htmlPage.Add("</div>");
                htmlPage.Add("<hr>");
            }
            if (output33 != null)
            {
                htmlPage.Add("<div id=\"SystemPower\" class=\"col-lg-12\">");
                htmlPage.Add("<h2 class=\"page-header\">Crash Dumps, Hibernation, and Conversion</h2>");
                htmlPage.Add("</div>");
                htmlPage.Add("<div class=\"col-md-12\">");
                string[] outputArray = output33.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                int countAUX = 0;
                foreach (string aux in outputArray)
                {
                    if (countAUX < 2)
                    {
                        htmlPage.Add("<p><b>" + aux + "</b></p>");
                    }
                    else
                    {
                        htmlPage.Add("<p>" + aux + "</p>");
                    }
                    countAUX++;
                }
                htmlPage.Add("</div>");
                htmlPage.Add("<hr>");
            }
            if (output34 != null)
            {
                htmlPage.Add("<div id=\"Sessions\" class=\"col-lg-12\">");
                htmlPage.Add("<h2 class=\"page-header\">Sessions Objects</h2>");
                htmlPage.Add("</div>");
                htmlPage.Add("<div class=\"col-md-12\">");
                string[] outputArray = output34.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                foreach (string aux in outputArray)
                {
                    if (aux.Contains(":"))
                    {
                        string[] auxArray = aux.Split(new[] { ':' }, 2);
                        htmlPage.Add("<p><b>" + auxArray[0] + ":</b>");
                        htmlPage.Add(auxArray[1] + "</p>");
                    }
                    else if (aux.Contains("**************************************************"))
                    {
                        htmlPage.Add("<p><b>" + aux + "</b></p>");
                    }
                    else
                    {
                        htmlPage.Add("<p>" + aux + "</p>");
                    }
                }
                htmlPage.Add("</div>");
                htmlPage.Add("<hr>");
            }
            if (output35 != null)
            {
                htmlPage.Add("<div id=\"WNDObjects\" class=\"col-lg-12\">");
                htmlPage.Add("<h2 class=\"page-header\">WND Stations Objects</h2>");
                htmlPage.Add("</div>");
                htmlPage.Add("<div class=\"col-md-12\">");
                string[] outputArray = output35.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                foreach (string aux in outputArray)
                {
                    if (aux.Contains("**************************************************"))
                    {
                        htmlPage.Add("<p><b>" + aux + "</b></p>");
                    }
                    else
                    {
                        htmlPage.Add("<p>" + aux + "</p>");
                    }
                }
                htmlPage.Add("</div>");
                htmlPage.Add("<hr>");
            }
            if (output36 != null)
            {
                htmlPage.Add("<div id=\"DeskScan\" class=\"col-lg-12\">");
                htmlPage.Add("<h2 class=\"page-header\">Desktop Scan</h2>");
                htmlPage.Add("</div>");
                htmlPage.Add("<div class=\"col-md-12\">");
                string[] outputArray = output36.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                foreach (string aux in outputArray)
                {
                    if (aux.Contains("**************************************************"))
                    {
                        htmlPage.Add("<p><b>" + aux + "</b></p>");
                    }
                    else
                    {
                        htmlPage.Add("<p>" + aux + "</p>");
                    }
                }
                htmlPage.Add("</div>");
                htmlPage.Add("<hr>");
            }
            if (output37 != null)
            {
                htmlPage.Add("<div id=\"Atom\" class=\"col-lg-12\">");
                htmlPage.Add("<h2 class=\"page-header\">Atom Table List</h2>");
                htmlPage.Add("</div>");
                htmlPage.Add("<div class=\"col-md-12\">");
                string[] outputArray = output37.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                int countAUX = 0;
                foreach (string aux in outputArray)
                {
                    if (countAUX < 2)
                    {
                        htmlPage.Add("<p><b>" + aux + "</b></p>");
                    }
                    else
                    {
                        htmlPage.Add("<p>" + aux + "</p>");
                    }
                    countAUX++;
                }
                htmlPage.Add("</div>");
                htmlPage.Add("<hr>");
            }
            if (output38 != null)
            {
                htmlPage.Add("<div id=\"Clipboard\" class=\"col-lg-12\">");
                htmlPage.Add("<h2 class=\"page-header\">User Clipboards</h2>");
                htmlPage.Add("</div>");
                htmlPage.Add("<div class=\"col-md-12\">");
                string[] outputArray = output38.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                int countAUX = 0;
                foreach (string aux in outputArray)
                {
                    if (countAUX < 2)
                    {
                        htmlPage.Add("<p><b>" + aux + "</b></p>");
                    }
                    else
                    {
                        htmlPage.Add("<p>" + aux + "</p>");
                    }
                    countAUX++;
                }
                htmlPage.Add("</div>");
                htmlPage.Add("<hr>");
            }
            if (output39 != null)
            {
                htmlPage.Add("<div id=\"EventHooks\" class=\"col-lg-12\">");
                htmlPage.Add("<h2 class=\"page-header\">Event Hooks</h2>");
                htmlPage.Add("</div>");
                htmlPage.Add("<div class=\"col-md-12\">");
                string[] outputArray = output39.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                foreach (string aux in outputArray)
                {
                    htmlPage.Add("<p>" + aux + "</p>");
                }
                htmlPage.Add("</div>");
                htmlPage.Add("<hr>");
            }
            if (output40 != null)
            {
                htmlPage.Add("<div id=\"gahti\" class=\"col-lg-12\">");
                htmlPage.Add("<h2 class=\"page-header\">USER Object Types</h2>");
                htmlPage.Add("</div>");
                htmlPage.Add("<div class=\"col-md-12\">");
                string[] outputArray = output40.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                int countAUX = 0;
                foreach (string aux in outputArray)
                {
                    if (countAUX < 2)
                    {
                        htmlPage.Add("<p><b>" + aux + "</b></p>");
                    }
                    else
                    {
                        htmlPage.Add("<p>" + aux + "</p>");
                    }
                    countAUX++;
                }
                htmlPage.Add("</div>");
                htmlPage.Add("<hr>");
            }
            if (output41 != null)
            {
                htmlPage.Add("<div id=\"MessageHooks\" class=\"col-lg-12\">");
                htmlPage.Add("<h2 class=\"page-header\">Message Hooks</h2>");
                htmlPage.Add("</div>");
                htmlPage.Add("<div class=\"col-md-12\">");
                string[] outputArray = output41.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                int countAUX = 0;
                foreach (string aux in outputArray)
                {
                    if (countAUX < 2)
                    {
                        htmlPage.Add("<p><b>" + aux + "</b></p>");
                    }
                    else
                    {
                        htmlPage.Add("<p>" + aux + "</p>");
                    }
                    countAUX++;
                }
                htmlPage.Add("</div>");
                htmlPage.Add("<hr>");
            }
            if (output42 != null)
            {
                htmlPage.Add("<div id=\"UserHandles\" class=\"col-lg-12\">");
                htmlPage.Add("<h2 class=\"page-header\">User Handles</h2>");
                htmlPage.Add("</div>");
                htmlPage.Add("<div class=\"col-md-12\">");
                string[] outputArray = output42.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                foreach (string aux in outputArray)
                {
                    if (aux.Contains(":") || aux.Contains("**************************************************") || aux.Contains("Object(V)                      Handle") || aux.Contains("------------------ ------------------ -"))
                    {
                        htmlPage.Add("<p><b>" + aux + ":</b></p>");
                    }
                    else
                    {
                        htmlPage.Add("<p>" + aux + "</p>");
                    }
                }
                htmlPage.Add("</div>");
                htmlPage.Add("<hr>");
            }
            if (output43 != null)
            {
                htmlPage.Add("<div id=\"gditimers\" class=\"col-lg-12\">");
                htmlPage.Add("<h2 class=\"page-header\">User Handles Table API Timers</h2>");
                htmlPage.Add("</div>");
                htmlPage.Add("<div class=\"col-md-12\">");
                string[] outputArray = output43.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                int countAUX = 0;
                foreach (string aux in outputArray)
                {
                    if (countAUX < 2)
                    {
                        htmlPage.Add("<p><b>" + aux + "</b></p>");
                    }
                    else
                    {
                        htmlPage.Add("<p>" + aux + "</p>");
                    }
                    countAUX++;
                }
                htmlPage.Add("</div>");
                htmlPage.Add("<hr>");
            }
            if (output44 != null)
            {
                htmlPage.Add("<div id=\"DesktopWindows\" class=\"col-lg-12\">");
                htmlPage.Add("<h2 class=\"page-header\">Desktop Windows</h2>");
                htmlPage.Add("</div>");
                htmlPage.Add("<div class=\"col-md-12\">");
                string[] outputArray = output44.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                foreach (string aux in outputArray)
                {
                    if (aux.Contains("**************************************************"))
                    {
                        htmlPage.Add("<p><b>" + aux + ":</b></p>");
                    }
                    else
                    {
                        htmlPage.Add("<p>" + aux + "</p>");
                    }
                }
                htmlPage.Add("</div>");
                htmlPage.Add("<hr>");
            }
            if (output45 != null && malfind.IsChecked == true)
            {
                htmlPage.Add("<div id=\"malfind\" class=\"col-lg-12\">");
                htmlPage.Add("<h2 class=\"page-header\">Malware List</h2>");
                htmlPage.Add("</div>");
                htmlPage.Add("<div class=\"col-md-12\">");
                string[] outputArray = output45.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                foreach (string aux in outputArray)
                {
                    if (aux.Contains(":"))
                    {
                        htmlPage.Add("<p><b>" + aux + ":</b></p>");
                    }
                    else
                    {
                        htmlPage.Add("<p>" + aux + "</p>");
                    }
                }
                htmlPage.Add("</div>");
                htmlPage.Add("<hr>");
            }
            if (output46 != null)
            {
                htmlPage.Add("<div id=\"services\" class=\"col-lg-12\">");
                htmlPage.Add("<h2 class=\"page-header\">Services List</h2>");
                htmlPage.Add("</div>");
                htmlPage.Add("<div class=\"col-md-12\">");
                string[] outputArray = output46.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                foreach (string aux in outputArray)
                {
                    if (aux.Contains(":"))
                    {
                        string[] auxArray = aux.Split(new[] { ':' }, 2);
                        htmlPage.Add("<p><b>" + auxArray[0] + ":</b>");
                        htmlPage.Add(auxArray[1] + "</p>");
                    }
                    else
                    {
                        htmlPage.Add("<p>" + aux + "</p>");
                    }
                }
                htmlPage.Add("</div>");
                htmlPage.Add("<hr>");
            }
            if (output47 != null)
            {
                htmlPage.Add("<div id=\"ldrmodules\" class=\"col-lg-12\">");
                htmlPage.Add("<h2 class=\"page-header\">LDR Modules</h2>");
                htmlPage.Add("</div>");
                htmlPage.Add("<div class=\"col-md-12\">");
                string[] outputArray = output47.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                int countAUX = 0;
                foreach (string aux in outputArray)
                {
                    if (countAUX < 2)
                    {
                        htmlPage.Add("<p><b>" + aux + "</b></p>");
                    }
                    else
                    {
                        htmlPage.Add("<p>" + aux + "</p>");
                    }
                    countAUX++;
                }
                htmlPage.Add("</div>");
                htmlPage.Add("<hr>");
            }
            if (output48 != null)
            {
                htmlPage.Add("<div id=\"apihooks\" class=\"col-lg-12\">");
                htmlPage.Add("<h2 class=\"page-header\">API Hooks</h2>");
                htmlPage.Add("</div>");
                htmlPage.Add("<div class=\"col-md-12\">");
                string[] outputArray = output48.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                foreach (string aux in outputArray)
                {
                    if (aux.Contains(":"))
                    {
                        htmlPage.Add("<p><b>" + aux + ":</b></p>");
                    }
                    else
                    {
                        htmlPage.Add("<p>" + aux + "</p>");
                    }
                }
                htmlPage.Add("</div>");
                htmlPage.Add("<hr>");
            }
            if (output49 != null)
            {
                htmlPage.Add("<div id=\"idt\" class=\"col-lg-12\">");
                htmlPage.Add("<h2 class=\"page-header\">Interrupt Descriptor Table</h2>");
                htmlPage.Add("</div>");
                htmlPage.Add("<div class=\"col-md-12\">");
                string[] outputArray = output49.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                int countAUX = 0;
                foreach (string aux in outputArray)
                {
                    if (countAUX < 2)
                    {
                        htmlPage.Add("<p><b>" + aux + "</b></p>");
                    }
                    else
                    {
                        htmlPage.Add("<p>" + aux + "</p>");
                    }
                    countAUX++;
                }
                htmlPage.Add("</div>");
                htmlPage.Add("<hr>");
            }
            if (output50 != null)
            {
                htmlPage.Add("<div id=\"gdt\" class=\"col-lg-12\">");
                htmlPage.Add("<h2 class=\"page-header\">Global Description Table</h2>");
                htmlPage.Add("</div>");
                htmlPage.Add("<div class=\"col-md-12\">");
                string[] outputArray = output50.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                int countAUX = 0;
                foreach (string aux in outputArray)
                {
                    if (countAUX < 2)
                    {
                        htmlPage.Add("<p><b>" + aux + "</b></p>");
                    }
                    else
                    {
                        htmlPage.Add("<p>" + aux + "</p>");
                    }
                    countAUX++;
                }
                htmlPage.Add("</div>");
                htmlPage.Add("<hr>");
            }
            if (output51 != null)
            {
                htmlPage.Add("<div id=\"threads\" class=\"col-lg-12\">");
                htmlPage.Add("<h2 class=\"page-header\">Threads List</h2>");
                htmlPage.Add("</div>");
                htmlPage.Add("<div class=\"col-md-12\">");
                string[] outputArray = output51.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                foreach (string aux in outputArray)
                {
                    if (aux.Contains("------"))
                    {
                        htmlPage.Add("<p><b>" + aux + ":</b></p>");
                    }
                    else
                    {
                        htmlPage.Add("<p>" + aux + "</p>");
                    }
                }
                htmlPage.Add("</div>");
                htmlPage.Add("<hr>");
            }
            if (output52 != null)
            {
                htmlPage.Add("<div id=\"Callbacks\" class=\"col-lg-12\">");
                htmlPage.Add("<h2 class=\"page-header\">Callbacks</h2>");
                htmlPage.Add("</div>");
                htmlPage.Add("<div class=\"col-md-12\">");
                string[] outputArray = output52.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                int countAUX = 0;
                foreach (string aux in outputArray)
                {
                    if (countAUX < 2)
                    {
                        htmlPage.Add("<p><b>" + aux + "</b></p>");
                    }
                    else
                    {
                        htmlPage.Add("<p>" + aux + "</p>");
                    }
                    countAUX++;
                }
                htmlPage.Add("</div>");
                htmlPage.Add("<hr>");
            }
            if (output53 != null)
            {
                htmlPage.Add("<div id=\"driverirp\" class=\"col-lg-12\">");
                htmlPage.Add("<h2 class=\"page-header\">Drivers IRP</h2>");
                htmlPage.Add("</div>");
                htmlPage.Add("<div class=\"col-md-12\">");
                string[] outputArray = output53.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                foreach (string aux in outputArray)
                {
                    if (aux.Contains(":") || aux.Contains("--------------------------------------------------"))
                    {
                        htmlPage.Add("<p><b>" + aux + ":</b></p>");
                    }
                    else
                    {
                        htmlPage.Add("<p>" + aux + "</p>");
                    }
                }
                htmlPage.Add("</div>");
                htmlPage.Add("<hr>");
            }
            if (output54 != null)
            {
                htmlPage.Add("<div id=\"timers\" class=\"col-lg-12\">");
                htmlPage.Add("<h2 class=\"page-header\">Kernel Timers</h2>");
                htmlPage.Add("</div>");
                htmlPage.Add("<div class=\"col-md-12\">");
                string[] outputArray = output54.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                int countAUX = 0;
                foreach (string aux in outputArray)
                {
                    if (countAUX < 2)
                    {
                        htmlPage.Add("<p><b>" + aux + "</b></p>");
                    }
                    else
                    {
                        htmlPage.Add("<p>" + aux + "</p>");
                    }
                    countAUX++;
                }
                htmlPage.Add("</div>");
                htmlPage.Add("<hr>");
            }
            if (output55 != null)
            {
                //aqui a conclusion
                htmlPage.Add("<div id=\"Conclusion\" class=\"col-lg-12\">");
                htmlPage.Add("<h2 class=\"page-header\">Conclusion</h2>");
                htmlPage.Add("</div>");
                htmlPage.Add("<div class=\"col-md-12\">");
                string[] outputArray = output55.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                //int countAUX = 0;
                foreach (string aux in outputArray)
                {
                    if (aux.Contains("Process Name:") && aux.Contains("PID:") || aux.Contains("This is a resume"))
                    {
                        htmlPage.Add("<p><b>" + aux + "</b></p>");
                    }
                    else
                    {
                        htmlPage.Add("<p>" + aux + "</p>");
                    }
                    //countAUX++;
                }
                htmlPage.Add("</div>");
                htmlPage.Add("<hr>");
            }

            htmlPage.Add("<p>");
            htmlPage.Add("<p>");
            htmlPage.Add("<footer>");
            htmlPage.Add("<div class=\"row\">");
            htmlPage.Add("<div class=\"col-lg-12\">");
            htmlPage.Add("<p>VolatilityEasyReport , " + DateTime.UtcNow.ToString("yyyy") + " - " + System.Security.Principal.WindowsIdentity.GetCurrent().Name + "</p>");
            htmlPage.Add("</div>");
            htmlPage.Add("</div>");
            htmlPage.Add("</footer>");
            htmlPage.Add("</div>");
            htmlPage.Add("</body>");
            htmlPage.Add("</html>");

            //test internet connection
            Ping myPing = new Ping();
            String host = "google.com";
            byte[] buffer = new byte[32];
            int timeout = 1000;
            PingOptions pingOptions = new PingOptions();
            PingReply reply = myPing.Send(host, timeout, buffer, pingOptions);
            if (reply.Status == IPStatus.Success)
            {
                // presumably online
            }
            else
            {
                MessageBox.Show("No Internet connection detected. Some functions of the created HTML page may not work.");
            }

            if (folder != null)
            {
                if (htmlPageMEM.Count != 0)
                {
                    Directory.CreateDirectory(folder + "\\" + fileName + "Extra");
                    File.WriteAllLines(folder + "\\" + fileName + "Extra\\" + fileName + "MEM" + ".htm", htmlPageMEM);
                }
                if (htmlPageMFT.Count != 0)
                {
                    Directory.CreateDirectory(folder + "\\" + fileName + "Extra");
                    File.WriteAllLines(folder + "\\" + fileName + "Extra\\" + fileName + "MFT" + ".htm", htmlPageMFT);
                }
                CentralWindow.historyListCons.Add(DateTime.UtcNow.ToString("HH:mm") + " | Report " + fileName + " created and saved in " + folder);
                MessageBox.Show("FileSaved to " + folder);
                File.WriteAllLines(folder + "\\" + fileName + ".htm", htmlPage);
                System.Diagnostics.Process.Start(folder + "\\" + fileName + ".htm");
            }
            else
            {
                if (htmlPageMEM.Count != 0)
                {
                    Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + @"Saves\HTMLPage\" + fileName + "Extra");
                    File.WriteAllLines(AppDomain.CurrentDomain.BaseDirectory + @"Saves\HTMLPage\" + fileName + "Extra\\" + fileName + "MEM" + ".htm", htmlPageMEM);
                }
                if (htmlPageMFT.Count != 0)
                {
                    Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + @"Saves\HTMLPage\" + fileName + "Extra");
                    File.WriteAllLines(AppDomain.CurrentDomain.BaseDirectory + @"Saves\HTMLPage\" + fileName + "Extra\\" + fileName + "MFT" + ".htm", htmlPageMFT);
                }
                CentralWindow.historyListCons.Add(DateTime.UtcNow.ToString("HH:mm") + " | Report " + fileName + " created and saved in " + AppDomain.CurrentDomain.BaseDirectory + @"Saves\HTMLPage\");
                MessageBox.Show("FileSaved to " + AppDomain.CurrentDomain.BaseDirectory + @"Saves\HTMLPage\");
                File.WriteAllLines(AppDomain.CurrentDomain.BaseDirectory + @"Saves\HTMLPage\" + fileName + ".htm", htmlPage);
                System.Diagnostics.Process.Start(AppDomain.CurrentDomain.BaseDirectory + @"Saves\HTMLPage\" + fileName + ".htm");
            }

        }

        private void ProgressCancel_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("This process will be canceled. Continue?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                CentralWindow.historyListCons.Add(DateTime.UtcNow.ToString("HH:mm") + " | Report " + fileName + " Cancelled");
                //cancelWasClicked = true;
                //ProgressCancel.IsEnabled = false;
                MessageBox.Show("The Process was Canceled");
                process.Kill();
                ProgressCancel.IsEnabled = true;
                BuildReport.IsEnabled = true;
                lbl_Progress.Visibility = Visibility.Hidden;
                lbl_NumberProgress.Visibility = Visibility.Hidden;
                pbStatus.Visibility = Visibility.Hidden;
                ProgressCancel.Visibility = Visibility.Hidden;
                this.Close();
            }
        }

        private void cleanOutputs()
        {
            output1 = null;
            output2 = null;
            output3 = null;
            output4 = null;
            output5 = null;
            output6 = null;
            output7 = null;
            output8 = null;
            output9 = null;
            output10 = null;
            output11 = null;
            output12 = null;
            output13 = null;
            output14 = null;
            output15 = null;
            output16 = null;
            output17 = null;
            output18 = null;
            output19 = null;
            output20 = null;
            output21 = null;
            output22 = null;
            output23 = null;
            output24 = null;
            output25 = null;
            output26 = null;
            output27 = null;
            output28 = null;
            output29 = null;
            output30 = null;
            output31 = null;
            output32 = null;
            output33 = null;
            output34 = null;
            output35 = null;
            output36 = null;
            output37 = null;
            output38 = null;
            output39 = null;
            output40 = null;
            output41 = null;
            output42 = null;
            output43 = null;
            output44 = null;
            output45 = null;
            output46 = null;
            output47 = null;
            output48 = null;
            output49 = null;
            output50 = null;
            output51 = null;
            output52 = null;
            output53 = null;
            output54 = null;
            output55 = null;
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

        private void SelectAll_Click(object sender, RoutedEventArgs e)
        {
            if (selectedAUX == false)
            {
                selectedAUX = true;
                //ImageInfo.IsChecked = true;
                Processes.IsChecked = true;
                ProcessesScan.IsChecked = true;
                ProcesseHandles.IsChecked = true;
                ProcesseDLL.IsChecked = true;
                ProcesseSID.IsChecked = true;
                ProcesseCMD.IsChecked = true;
                ProcessePrivileges.IsChecked = true;
                ProcesseEnvPrivileges.IsChecked = true;
                ProcesseVerify.IsChecked = true;
                MemMap.IsChecked = true;
                InternetExplorer.IsChecked = true;
                KernelDrivers.IsChecked = true;
                KernelSSDT.IsChecked = true;
                Drivers.IsChecked = true;
                FileScan.IsChecked = true;
                MutantScan.IsChecked = true;
                SymbolicLinks.IsChecked = true;
                ETHREADobjects.IsChecked = true;
                UnloadedModules.IsChecked = true;
                Networking.IsChecked = true;
                RegistryHives.IsChecked = true;
                RegistryKeys.IsChecked = true;
                RegistryDomain.IsChecked = true;
                RegistryLSA.IsChecked = true;
                RegistryUserAssist.IsChecked = true;
                RegistryShell.IsChecked = true;
                RegistryShim.IsChecked = true;
                RegistrySID.IsChecked = true;
                FileMFT.IsChecked = true;
                BIOS.IsChecked = true;
                Timeliner.IsChecked = true;
                CrashInfo.IsChecked = true;
                Sessions.IsChecked = true;
                WNDObjects.IsChecked = true;
                DeskScan.IsChecked = true;
                Atom.IsChecked = true;
                Clipboard.IsChecked = true;
                EventHooks.IsChecked = true;
                gahti.IsChecked = true;
                MessageHooks.IsChecked = true;
                UserHandles.IsChecked = true;
                gditimers.IsChecked = true;
                DesktopWindows.IsChecked = true;
                malfind.IsChecked = true;
                services.IsChecked = true;
                ldrmodules.IsChecked = true;
                apihooks.IsChecked = true;
                idt.IsChecked = true;
                gdt.IsChecked = true;
                threads.IsChecked = true;
                Callbacks.IsChecked = true;
                driverirp.IsChecked = true;
                timers.IsChecked = true;
                Conclusion.IsChecked = true;
            }
            else
            {
                selectedAUX = false;
                Processes.IsChecked = false;
                ProcessesScan.IsChecked = false;
                ProcesseHandles.IsChecked = false;
                ProcesseDLL.IsChecked = false;
                ProcesseSID.IsChecked = false;
                ProcesseCMD.IsChecked = false;
                ProcessePrivileges.IsChecked = false;
                ProcesseEnvPrivileges.IsChecked = false;
                ProcesseVerify.IsChecked = false;
                MemMap.IsChecked = false;
                InternetExplorer.IsChecked = false;
                KernelDrivers.IsChecked = false;
                KernelSSDT.IsChecked = false;
                Drivers.IsChecked = false;
                FileScan.IsChecked = false;
                MutantScan.IsChecked = false;
                SymbolicLinks.IsChecked = false;
                ETHREADobjects.IsChecked = false;
                UnloadedModules.IsChecked = false;
                Networking.IsChecked = false;
                RegistryHives.IsChecked = false;
                RegistryKeys.IsChecked = false;
                RegistryDomain.IsChecked = false;
                RegistryLSA.IsChecked = false;
                RegistryUserAssist.IsChecked = false;
                RegistryShell.IsChecked = false;
                RegistryShim.IsChecked = false;
                RegistrySID.IsChecked = false;
                FileMFT.IsChecked = false;
                BIOS.IsChecked = false;
                Timeliner.IsChecked = false;
                CrashInfo.IsChecked = false;
                Sessions.IsChecked = false;
                WNDObjects.IsChecked = false;
                DeskScan.IsChecked = false;
                Atom.IsChecked = false;
                Clipboard.IsChecked = false;
                EventHooks.IsChecked = false;
                gahti.IsChecked = false;
                MessageHooks.IsChecked = false;
                UserHandles.IsChecked = false;
                gditimers.IsChecked = false;
                DesktopWindows.IsChecked = false;
                malfind.IsChecked = false;
                services.IsChecked = false;
                ldrmodules.IsChecked = false;
                apihooks.IsChecked = false;
                idt.IsChecked = false;
                gdt.IsChecked = false;
                threads.IsChecked = false;
                Callbacks.IsChecked = false;
                driverirp.IsChecked = false;
                timers.IsChecked = false;
                Conclusion.IsChecked = false;
            }
        }
    }
}

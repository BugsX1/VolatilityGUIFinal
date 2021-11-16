using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace VolatilityGUI
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class CentralWindow : Window
    {
        private static string imageRam;
        private static string imageRamProfile;
        private static string loadAux;
        private static string commandCB;
        private string output;
        private List<TabItem> _tabItems;
        private TabItem _tabAdd;
        private static string loadedFile;
        private static string closingAUX;
        private static string key;
        private string fileSave;
        private string folderAux;
        private string advanceToogle;
        private string pid;
        private string offset;
        private string type;
        private string hiveSYSTEM;
        private string hiveSAM;
        private int hashToogle = 0;
        private static List<string> pidList = new List<string>();
        private static List<string> historyList = new List<string>();
        System.Diagnostics.Process process = new System.Diagnostics.Process();
        private bool processCanceled = false;
        private string previouslySearchText = "";
        private int auxCount = 0;
        private int auxClick = 0;


        public CentralWindow()
        {
            InitializeComponent();
            historyList.Add(DateTime.UtcNow.ToString("HH:mm") + " | VolatilityGUI Started");
            if (imageRam == null)
            {
                ComboBox.Text = "No RAM Image Selected";
                ComboBox.IsEnabled = false;
                txtFind.Text = "No RAM Image";
                txtFind.IsEnabled = false;
                FindCommand.IsEnabled = false;
                RunCommand.IsEnabled = false;
                EasyReport.IsEnabled = false;
                txtFind.IsReadOnly = true;
                AboutRAMTab.IsEnabled = false;
            }
            else
            {
                historyList.Add(DateTime.UtcNow.ToString("HH:mm") + " | RAM Image Loaded " + imageRam);
            }
            closingAUX = "something";
            //tab control
            // initialize tabItem array
            _tabItems = new List<TabItem>();

            // add a tabItem with + in header 
            //_tabAdd = new TabItem();
            // _tabAdd.Header = "+";
            //_tabItems.Add(_tabAdd);

            // add first tab
            this.AddTabItem("Tutorial");

            // bind tab control
            tabDynamic.DataContext = _tabItems;

            tabDynamic.SelectedIndex = 0;

            historyList.Add(DateTime.UtcNow.ToString("HH:mm") + " | Tutorial Tab Created");

            /*
            //cenas de mover
            Grid.SetRowSpan(lbl_About , 2);
            Grid.SetRow(Keyword , 1);
            Grid.SetRow(txtFind, 1);
            Grid.SetRow(FindCommand, 1);
            Grid.SetRow(Result, 1);
            Grid.SetRow(lbl_Status, 1);
            Grid.SetRow(tabDynamic, 2);
            Grid.SetRowSpan(tabDynamic, 9);
            */
        }

        //-----------------Contrusctors------------------------------
        public static string imageRamCons
        {
            get { return imageRam; }
            set { imageRam = value; }
        }

        public static string closingAUXCons
        {
            get { return closingAUX; }
            set { closingAUX = value; }
        }

        public static string imageRamProfileCons
        {
            get { return imageRamProfile; }
            set { imageRamProfile = value; }
        }

        public static string loadAuxCons
        {
            get { return loadAux; }
            set { loadAux = value; }
        }

        public static string loadedFileCons
        {
            get { return loadedFile; }
            set { loadedFile = value; }
        }
        public static List<string> pidListCons
        {
            get { return pidList; }
            set { pidList = value; }
        }
        public static string keyCons
        {
            get { return key; }
            set { key = value; }
        }

        public static List<string> historyListCons
        {
            get { return historyList; }
            set { historyList = value; }
        }

        //----------------------------------Buttons & Event Windows---------------------------------------------
        private void EasyReport_Click(object sender, RoutedEventArgs e)
        {
            if (imageRam != null)
            {
                EasyReportWindow objSecondWindow = new EasyReportWindow();
                objSecondWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("No RAM Image Selected");
            }
        }
        private void NewRAM_Click(object sender, RoutedEventArgs e)
        {
            NewRAMWindow objSecondWindow = new NewRAMWindow();
            objSecondWindow.ShowDialog();
        }

        private void AboutRAM_Click(object sender, RoutedEventArgs e)
        {
            if (imageRam != null)
            {
                AboutRAMWindow objSecondWindow = new AboutRAMWindow();
                objSecondWindow.ShowDialog();
            }
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow objSecondWindow = new AboutWindow();
            objSecondWindow.ShowDialog();
        }

        private void ExtractSomeDLL_Click(object sender, RoutedEventArgs e)
        {
            if (imageRam != null)
            {
                //DLLWindow objSecondWindow = new DLLWindow();
                DLLWindow objSecondWindow = new DLLWindow();
                objSecondWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("No RAM Image Selected");
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            fileSave = "FileVolatilityGUI";
            fileSave += Environment.NewLine;
            fileSave += "Ram:" + imageRam + "EndRam";
            fileSave += Environment.NewLine;
            fileSave += "Profile:" + imageRamProfile + "EndProfileRam";
            fileSave += Environment.NewLine;
            fileSave += "hiveSYSTEM:" + hiveSYSTEM + "EndhiveSYSTEM";
            fileSave += Environment.NewLine;
            fileSave += "hiveSAM:" + hiveSAM + "EndhiveSAM";
            fileSave += Environment.NewLine;
            fileSave += "TextBox:";
            string tabItemsAux = string.Join(",", _tabItems);
            var charsToRemove = new string[] { "System.Windows.Controls.TabItem Header:", " Content:" };
            foreach (var c in charsToRemove)
            {
                tabItemsAux = tabItemsAux.Replace(c, string.Empty);
            }
            fileSave += tabItemsAux;
            fileSave += "EndTextBox";
            fileSave += Environment.NewLine;
            fileSave += Environment.NewLine;
            fileSave += "Content:";
            List<string> contentAux = tabItemsAux.Split(',').ToList<string>();
            foreach (var i in contentAux)
            {
                fileSave += Environment.NewLine;
                RichTextBox myTextBoxTemp = (RichTextBox)this.FindName("txtR" + i);
                TextRange textRangeTemp = new TextRange(myTextBoxTemp.Document.ContentStart, myTextBoxTemp.Document.ContentEnd);
                fileSave += i + ":";
                //fileSave += Environment.NewLine;
                fileSave += textRangeTemp.Text;
                //fileSave += Environment.NewLine;
                fileSave += i + "end";
            }

            if (loadedFile != null)
            {
                //fazer save normal
                string fileSaveEncrypt = StringCipher.Encrypt(fileSave, key);
                File.WriteAllText(loadedFile, fileSaveEncrypt);
                MessageBox.Show("Saved!");
            }
            else
            {
                /*
                //cenas de mover
                if (advanceCommandLabel.Visibility == Visibility.Visible)
                {
                    Grid.SetRowSpan(lbl_About, 5);
                    Grid.SetRow(Keyword, 4);
                    Grid.SetRow(txtFind, 4);
                    Grid.SetRow(FindCommand, 4);
                    Grid.SetRow(Result, 4);
                    Grid.SetRow(lbl_Status, 4);
                    Grid.SetRow(tabDynamic, 5);
                    Grid.SetRowSpan(tabDynamic, 5);
                    Grid.SetRow(keyLabel2, 3);
                    Grid.SetRow(keybox, 3);
                    Grid.SetRow(keyBtn, 3);
                    Grid.SetRow(keyBtn2, 3);
                    Grid.SetRow(keyCloseBtn, 3);
                }
                else
                {
                    Grid.SetRowSpan(lbl_About, 3);
                    Grid.SetRow(Keyword, 2);
                    Grid.SetRow(txtFind, 2);
                    Grid.SetRow(FindCommand, 2);
                    Grid.SetRow(Result, 2);
                    Grid.SetRow(lbl_Status, 2);
                    Grid.SetRow(tabDynamic, 3);
                    Grid.SetRowSpan(tabDynamic, 8);
                    Grid.SetRow(keyLabel2, 2);
                    Grid.SetRow(keybox, 2);
                    Grid.SetRow(keyBtn, 2);
                    Grid.SetRow(keyBtn2, 2);
                    Grid.SetRow(keyCloseBtn, 2);
                }
                */

                keybox.Visibility = Visibility.Visible;
                keyBtn.Visibility = Visibility.Visible;
                //keyLabel.Visibility = Visibility.Visible;
                keyLabel2.Visibility = Visibility.Visible;
                keyCloseBtn.Visibility = Visibility.Visible;
            }
            /*
            if (loadedFile != null)
            {
                //fazer save normal
                string fileSaveEncrypt = Protect(fileSave);
                File.WriteAllText(loadedFile, fileSaveEncrypt);
                MessageBox.Show("Saved!");
            }
            else
            {
                SaveFileDialog saveDialog = new SaveFileDialog()
                {
                    Filter = "Text Files(*.txt)|*.txt|All(*.*)|*"
                };

                if (saveDialog.ShowDialog() == true)
                {
                    loadedFile = saveDialog.FileName;
                    this.Title = "VolatilityGUI - " + saveDialog.FileName;
                    //string fileSaveEncrypt = StringCipher.Encrypt(plaintext, password);
                    string fileSaveEncrypt = Protect(fileSave);
                    File.WriteAllText(saveDialog.FileName, fileSaveEncrypt, Encoding.UTF8);
                    MessageBox.Show("Saved!");
                }
            }
            */
        }

        private void SaveAs_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(hiveSYSTEM);
            MessageBox.Show(hiveSAM);

            fileSave = "FileVolatilityGUI";
            fileSave += Environment.NewLine;
            fileSave += "Ram:" + imageRam + "EndRam";
            fileSave += Environment.NewLine;
            fileSave += "Profile:" + imageRamProfile + "EndProfileRam";
            fileSave += Environment.NewLine;
            fileSave += "hiveSYSTEM:" + hiveSYSTEM + "EndhiveSYSTEM";
            fileSave += Environment.NewLine;
            fileSave += "hiveSAM:" + hiveSAM + "EndhiveSAM";
            fileSave += Environment.NewLine;
            fileSave += "TextBox:";
            string tabItemsAux = string.Join(",", _tabItems);
            var charsToRemove = new string[] { "System.Windows.Controls.TabItem Header:", " Content:" };
            foreach (var c in charsToRemove)
            {
                tabItemsAux = tabItemsAux.Replace(c, string.Empty);
            }
            fileSave += tabItemsAux;
            fileSave += "EndTextBox";
            fileSave += Environment.NewLine;
            fileSave += Environment.NewLine;
            fileSave += "Content:";
            List<string> contentAux = tabItemsAux.Split(',').ToList<string>();
            foreach (var i in contentAux)
            {
                fileSave += Environment.NewLine;
                RichTextBox myTextBoxTemp = (RichTextBox)this.FindName("txtR" + i);
                TextRange textRangeTemp = new TextRange(myTextBoxTemp.Document.ContentStart, myTextBoxTemp.Document.ContentEnd);
                fileSave += i + ":";
                //fileSave += Environment.NewLine;
                fileSave += textRangeTemp.Text;
                //fileSave += Environment.NewLine;
                fileSave += i + "end";
            }
            keybox.Visibility = Visibility.Visible;
            keyBtn2.Visibility = Visibility.Visible;
            //keyLabel.Visibility = Visibility.Visible;
            keyLabel2.Visibility = Visibility.Visible;
            keyCloseBtn.Visibility = Visibility.Visible;

            //cenas de mover
            /*
            if (advanceCommandLabel.Visibility == Visibility.Visible)
            {
                Grid.SetRowSpan(lbl_About, 5);
                Grid.SetRow(Keyword, 4);
                Grid.SetRow(txtFind, 4);
                Grid.SetRow(FindCommand, 4);
                Grid.SetRow(Result, 4);
                Grid.SetRow(lbl_Status, 4);
                Grid.SetRow(tabDynamic, 5);
                Grid.SetRowSpan(tabDynamic, 6);
                Grid.SetRow(keyLabel2, 3);
                Grid.SetRow(keybox, 3);
                Grid.SetRow(keyBtn, 3);
                Grid.SetRow(keyBtn2, 3);
                Grid.SetRow(keyCloseBtn, 3);
            }
            else
            {
                Grid.SetRowSpan(lbl_About, 3);
                Grid.SetRow(Keyword, 2);
                Grid.SetRow(txtFind, 2);
                Grid.SetRow(FindCommand, 2);
                Grid.SetRow(Result, 2);
                Grid.SetRow(lbl_Status, 2);
                Grid.SetRow(tabDynamic, 3);
                Grid.SetRowSpan(tabDynamic, 8);
                Grid.SetRow(keyLabel2, 1);
                Grid.SetRow(keybox, 1);
                Grid.SetRow(keyBtn, 1);
                Grid.SetRow(keyBtn2, 1);
                Grid.SetRow(keyCloseBtn, 1);
            }
            */

            /*
            SaveFileDialog saveDialog = new SaveFileDialog()
            {
                Filter = "Text Files(*.txt)|*.txt|All(*.*)|*"
            };

            if (saveDialog.ShowDialog() == true)
            {
                loadedFile = saveDialog.FileName;
                this.Title = "VolatilityGUI - " + saveDialog.FileName;
                string fileSaveEncrypt = Protect(fileSave);
                File.WriteAllText(saveDialog.FileName, fileSaveEncrypt, Encoding.UTF8);
            }
            */
        }

        private void SaveKey_Click(object sender, RoutedEventArgs e)
        {
            if (keybox.Text.Trim().Length < 1)
            {
                MessageBox.Show("No key provided");
            }
            else
            {
                SaveFileDialog saveDialog = new SaveFileDialog()
                {
                    Filter = "Text Files(*.txt)|*.txt|All(*.*)|*"
                };

                if (saveDialog.ShowDialog() == true)
                {
                    loadedFile = saveDialog.FileName;
                    this.Title = "VolatilityGUI - " + saveDialog.FileName;
                    historyList.Add(DateTime.UtcNow.ToString("HH:mm") + " | File Saved : " + saveDialog.SafeFileName);
                    key = keybox.Text;
                    string fileSaveEncrypt = StringCipher.Encrypt(fileSave, keybox.Text);
                    File.WriteAllText(saveDialog.FileName, fileSaveEncrypt, Encoding.UTF8);
                    MessageBox.Show("Saved!");
                    keybox.Visibility = Visibility.Hidden;
                    keyBtn.Visibility = Visibility.Hidden;
                    //keyLabel.Visibility = Visibility.Hidden;
                    keyLabel2.Visibility = Visibility.Hidden;
                    keyCloseBtn.Visibility = Visibility.Hidden;

                    /*
                    //cenas de mover
                    if (advanceCommandLabel.Visibility == Visibility.Visible)
                    {
                        Grid.SetRowSpan(lbl_About, 4);
                        Grid.SetRow(Keyword, 3);
                        Grid.SetRow(txtFind, 3);
                        Grid.SetRow(FindCommand, 3);
                        Grid.SetRow(Result, 3);
                        Grid.SetRow(lbl_Status, 3);
                        Grid.SetRow(tabDynamic, 4);
                        Grid.SetRowSpan(tabDynamic, 7);
                    }
                    else
                    {
                        Grid.SetRowSpan(lbl_About, 2);
                        Grid.SetRow(Keyword, 1);
                        Grid.SetRow(txtFind, 1);
                        Grid.SetRow(FindCommand, 1);
                        Grid.SetRow(Result, 1);
                        Grid.SetRow(lbl_Status, 1);
                        Grid.SetRow(tabDynamic, 2);
                        Grid.SetRowSpan(tabDynamic, 9);
                    }
                    */
                }

            }
        }

        private void SaveKeyAs_Click(object sender, RoutedEventArgs e)
        {
            if (keybox.Text.Trim().Length < 1)
            {
                MessageBox.Show("No key provided");
            }
            else
            {
                SaveFileDialog saveDialog = new SaveFileDialog()
                {
                    Filter = "Text Files(*.txt)|*.txt|All(*.*)|*"
                };

                if (saveDialog.ShowDialog() == true)
                {
                    loadedFile = saveDialog.FileName;
                    this.Title = "VolatilityGUI - " + saveDialog.FileName;
                    historyList.Add(DateTime.UtcNow.ToString("HH:mm") + " | File Saved : " + saveDialog.SafeFileName);
                    key = keybox.Text;
                    string fileSaveEncrypt = StringCipher.Encrypt(fileSave, keybox.Text);
                    File.WriteAllText(saveDialog.FileName, fileSaveEncrypt, Encoding.UTF8);
                    keybox.Visibility = Visibility.Hidden;
                    keyBtn2.Visibility = Visibility.Hidden;
                    //keyLabel.Visibility = Visibility.Hidden;
                    keyLabel2.Visibility = Visibility.Hidden;
                    keyCloseBtn.Visibility = Visibility.Hidden;

                    /*
                    //cenas de mover
                    if (advanceCommandLabel.Visibility == Visibility.Visible)
                    {
                        Grid.SetRowSpan(lbl_About, 4);
                        Grid.SetRow(Keyword, 3);
                        Grid.SetRow(txtFind, 3);
                        Grid.SetRow(FindCommand, 3);
                        Grid.SetRow(Result, 3);
                        Grid.SetRow(lbl_Status, 3);
                        Grid.SetRow(tabDynamic, 4);
                        Grid.SetRowSpan(tabDynamic, 7);
                    }
                    else
                    {
                        Grid.SetRowSpan(lbl_About, 2);
                        Grid.SetRow(Keyword, 1);
                        Grid.SetRow(txtFind, 1);
                        Grid.SetRow(FindCommand, 1);
                        Grid.SetRow(Result, 1);
                        Grid.SetRow(lbl_Status, 1);
                        Grid.SetRow(tabDynamic, 2);
                        Grid.SetRowSpan(tabDynamic, 9);
                    }
                    */
                }
            }
        }
        private void CloseKey_Click(object sender, RoutedEventArgs e)
        {
            keybox.Visibility = Visibility.Hidden;
            keyBtn.Visibility = Visibility.Hidden;
            keyBtn2.Visibility = Visibility.Hidden;
            //keyLabel.Visibility = Visibility.Hidden;
            keyLabel2.Visibility = Visibility.Hidden;
            keyCloseBtn.Visibility = Visibility.Hidden;

            /*
            //cenas de mover
            if (advanceCommandLabel.Visibility == Visibility.Visible)
            {
                Grid.SetRowSpan(lbl_About, 4);
                Grid.SetRow(Keyword, 3);
                Grid.SetRow(txtFind, 3);
                Grid.SetRow(FindCommand, 3);
                Grid.SetRow(Result, 3);
                Grid.SetRow(lbl_Status, 3);
                Grid.SetRow(tabDynamic, 4);
                Grid.SetRowSpan(tabDynamic, 7);
            }
            else
            {
                Grid.SetRowSpan(lbl_About, 2);
                Grid.SetRow(Keyword, 1);
                Grid.SetRow(txtFind, 1);
                Grid.SetRow(FindCommand, 1);
                Grid.SetRow(Result, 1);
                Grid.SetRow(lbl_Status, 1);
                Grid.SetRow(tabDynamic, 2);
                Grid.SetRowSpan(tabDynamic, 9);
            }
            */
        }

        private void History_Click(object sender, RoutedEventArgs e)
        {
            HistoryWindow objSecondWindow = new HistoryWindow();
            objSecondWindow.Show();
        }

        private void ForensicH_Click(object sender, RoutedEventArgs e)
        {
            if (imageRam != null)
            {
                ForensicWindow objSecondWindow = new ForensicWindow();
                objSecondWindow.Show();
            }
            else
            {
                MessageBox.Show("No RAM Image loaded");
            }
        }

        private void VirusTotal_Click(object sender, RoutedEventArgs e)
        {
            if (imageRam != null)
            {
                VirusTotalWindow objSecondWindow = new VirusTotalWindow();
                objSecondWindow.Show();
            }
            else
            {
                MessageBox.Show("No RAM Image loaded");
            }
        }

        private void YaraScanH_Click(object sender, RoutedEventArgs e)
        {
            if (imageRam != null)
            {
                YaraScanWindow objSecondWindow = new YaraScanWindow();
                objSecondWindow.Show();
            }
            else
            {
                MessageBox.Show("No RAM Image loaded");
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            string tabName = (sender as Button).CommandParameter.ToString();

            var item = tabDynamic.Items.Cast<TabItem>().Where(i => i.Name.Equals(tabName)).SingleOrDefault();
            historyList.Add(DateTime.UtcNow.ToString("HH:mm") + " | Deleted Tab : " + tabName);

            TabItem tab = item as TabItem;

            if (tab != null)
            {
                if (_tabItems.Count < 2)
                {
                    MessageBox.Show("Cannot remove last tab.");
                }
                else if (MessageBox.Show(string.Format("Are you sure you want to remove the tab '{0}'?", tab.Header.ToString()),
                    "Remove Tab", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    if (loadedFile != null)
                        this.Title = "VolatilityGUI - " + loadedFile + "*";
                    // get selected tab
                    TabItem selectedTab = tabDynamic.SelectedItem as TabItem;
                    string selectedTabAUX = selectedTab.Name;

                    // clear tab control binding
                    tabDynamic.DataContext = null;

                    _tabItems.Remove(tab);

                    // bind tab control
                    tabDynamic.DataContext = _tabItems;

                    this.UnregisterName("txtR" + selectedTabAUX);

                    // select previously selected tab. if that is removed then select first tab
                    if (selectedTab == null || selectedTab.Equals(tab))
                    {
                        selectedTab = _tabItems[0];
                    }
                    tabDynamic.SelectedItem = selectedTab;
                }
            }
        }

        private void btnSelectCBCommand_Click(object sender, RoutedEventArgs e)
        {
            //ComboBoxItem typeItem = (ComboBoxItem)ComboBoxDependent.SelectedItem;
            string valueCB = (string)ComboBoxDependent.SelectedItem;
            advanceCommandInput.Text = "";

            if (loadedFile != null)
                this.Title = "VolatilityGUI - " + loadedFile + "*";

            if (valueCB != null)
            {
                getCommandRun(valueCB);
            }
            else
            {
                MessageBox.Show("No Value Selected");
            }
        }
        private void ExtractAllDLL_Click(object sender, RoutedEventArgs e)
        {
            if (imageRam != null)
            {
                System.IO.Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + @"Saves\DLL");
                var dlg = new CommonOpenFileDialog();
                dlg.Title = "Choose Download Folder";
                dlg.IsFolderPicker = true;
                dlg.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory + @"Saves\DLL";
                dlg.AllowNonFileSystemItems = false;
                dlg.DefaultDirectory = AppDomain.CurrentDomain.BaseDirectory + @"Saves\DLL";
                dlg.EnsureFileExists = true;
                dlg.EnsurePathExists = true;
                dlg.EnsureReadOnly = false;
                dlg.EnsureValidNames = true;
                dlg.Multiselect = false;
                dlg.ShowPlacesList = true;

                if (dlg.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    folderAux = dlg.FileName;
                    historyList.Add(DateTime.UtcNow.ToString("HH:mm") + " | Extracted all DLLs to " + folderAux);
                    commandCB = "dlldump";
                    runCommand();
                }
                else
                {
                    MessageBox.Show("No valid path select");
                }
            }
            else
            {
                MessageBox.Show("No RAM Image Selected");
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (closingAUX != null)
            {
                MessageBoxResult result = MessageBox.Show("Any unsave changes will not be saved. Continue?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.No)
                {
                    e.Cancel = true;
                }
            }
        }

        private void btnFind_Click(object sender, RoutedEventArgs e)
        {

            TabItem selectedTab = tabDynamic.SelectedItem as TabItem;
            string selectedTabAUX = selectedTab.Name;
            RichTextBox myRichTextBox = (RichTextBox)this.FindName("txtR" + selectedTabAUX);

            RichTextBox rich = myRichTextBox;

            TextRange textRange = new TextRange(myRichTextBox.Document.ContentStart, myRichTextBox.Document.ContentEnd);

            //clear up highlighted text before starting a new search
            textRange.ClearAllProperties();
            lbl_Status.Text = "";

            //get the richtextbox text
            string textBoxText = textRange.Text;

            //get search text
            string searchText = txtFind.Text;

            historyList.Add(DateTime.UtcNow.ToString("HH:mm") + " | Find Function (" + searchText + ")");

            if (string.IsNullOrWhiteSpace(textBoxText) || string.IsNullOrWhiteSpace(searchText))
            {
                lbl_Status.Text = "Please provide search text or source text to search from";
            }

            else
            {
                if (txtFind.Text.StartsWith("$regex:") || txtFind.Text.StartsWith("$Regex:"))
                {
                    if (previouslySearchText == searchText)
                    {
                        auxCount = 0;
                        auxClick++;
                    }
                    else
                    {
                        auxClick = 0;
                        auxCount = 0;
                    }
                    //get rule
                    string auxRegex = txtFind.Text.Substring(7).Trim();
                    Regex reg = new Regex(auxRegex, RegexOptions.Compiled | RegexOptions.IgnoreCase);
                    TextPointer position = rich.Document.ContentStart;
                    List<TextRange> ranges = new List<TextRange>();
                    while (position != null)
                    {
                        if (position.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.Text)
                        {
                            string text = position.GetTextInRun(LogicalDirection.Forward);
                            var matchs = reg.Matches(text);

                            foreach (Match match in matchs)
                            {
                                auxCount++;

                                TextPointer start = position.GetPositionAtOffset(match.Index);
                                TextPointer end = start.GetPositionAtOffset(1);

                                TextRange textrange = new TextRange(start, end);
                                ranges.Add(textrange);

                                if (auxCount == auxClick)
                                {
                                    var characterRect = start.GetCharacterRect(LogicalDirection.Forward);
                                    rich.ScrollToHorizontalOffset(rich.HorizontalOffset + characterRect.Left - rich.ActualWidth / 2d);
                                    rich.ScrollToVerticalOffset(rich.VerticalOffset + characterRect.Top - rich.ActualHeight / 2d);
                                }
                            }
                        }
                        position = position.GetNextContextPosition(LogicalDirection.Forward);
                    }
                    int rangesCount = ranges.Count;
                    if (rangesCount == 0)
                    {
                        lbl_Status.Text = "No Match Found ! for regex rule " + auxRegex;
                    }
                    else
                    {
                        lbl_Status.Text = "Total Match Found : " + ranges.Count + " for regex rule " + auxRegex;
                    }
                    foreach (TextRange range in ranges)
                    {
                        range.ApplyPropertyValue(TextElement.BackgroundProperty, new SolidColorBrush(Colors.Yellow));
                    }
                    previouslySearchText = txtFind.Text;
                }
                else
                {
                    if (previouslySearchText == searchText)
                    {
                        auxCount = 0;
                        auxClick++;
                    }
                    else
                    {
                        auxClick = 0;
                        auxCount = 0;
                    }
                    //using regex to get the search count
                    Regex regex = new Regex(searchText);
                    int count_MatchFound = Regex.Matches(textBoxText, regex.ToString()).Count;
                    for (TextPointer startPointer = rich.Document.ContentStart;
                            startPointer.CompareTo(rich.Document.ContentEnd) <= 0;
                                startPointer = startPointer.GetNextContextPosition(LogicalDirection.Forward))
                    {
                        
                        //check if end of text
                        if (startPointer.CompareTo(rich.Document.ContentEnd) == 0)
                        {
                            break;
                        }
                        //get the adjacent string
                        string parsedString = startPointer.GetTextInRun(LogicalDirection.Forward);

                        //check if the search string present here
                        int indexOfParseString = parsedString.IndexOf(searchText);

                        if (indexOfParseString >= 0) //present
                        {
                            //setting up the pointer here at this matched index
                            startPointer = startPointer.GetPositionAtOffset(indexOfParseString);

                            if (startPointer != null)
                            {
                                auxCount++;
                                //next pointer will be the length of the search string
                                TextPointer nextPointer = startPointer.GetPositionAtOffset(searchText.Length);

                                //create the text range
                                TextRange searchedTextRange = new TextRange(startPointer, nextPointer);

                                //color up 
                                searchedTextRange.ApplyPropertyValue(TextElement.BackgroundProperty, new SolidColorBrush(Colors.Yellow));

                                //add other setting property
                                if (auxCount/2 == auxClick)
                                {
                                    var characterRect = startPointer.GetCharacterRect(LogicalDirection.Forward);
                                    rich.ScrollToHorizontalOffset(rich.HorizontalOffset + characterRect.Left - rich.ActualWidth / 2d);
                                    rich.ScrollToVerticalOffset(rich.VerticalOffset + characterRect.Top - rich.ActualHeight / 2d);
                                    auxCount++;
                                }

                            }
                        }
                        
                    }

                    //update the label text with count
                    if (count_MatchFound > 0)
                    {
                        lbl_Status.Text = "Total Match Found : " + count_MatchFound + " for keyword " + searchText;
                    }
                    else
                    {
                        lbl_Status.Text = "No Match Found ! for keyword " + searchText;
                    }

                    previouslySearchText = searchText;
                }
            }
        }

        private void ProgressCancel_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("This process will be canceled. Continue?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                //cancelWasClicked = true;
                //ProgressCancel.IsEnabled = false;                
                processCanceled = true;
                process.Kill();
                CancelLabel.Visibility = Visibility.Hidden;
                pbStatus.Visibility = Visibility.Hidden;
                cancelBtn.Visibility = Visibility.Hidden;
                MessageBox.Show("The Process was Canceled");
                RunCommand.IsEnabled = true;
                RunCommand.IsEnabled = true;
                FindCommand.IsEnabled = true;
                EasyReport.IsEnabled = true;
                NewRamH.IsEnabled = true;
                AboutRAMTab.IsEnabled = true;
                SaveH.IsEnabled = true;
                SaveAsH.IsEnabled = true;
                HistoryH.IsEnabled = true;
                VirusH.IsEnabled = true;
                YaraScanH.IsEnabled = true;
                ForensicH.IsEnabled = true;

                /*
                //cenas de mover
                if (advanceCommandLabel.Visibility == Visibility.Visible)
                {
                    Grid.SetRowSpan(lbl_About, 4);
                    Grid.SetRow(Keyword, 3);
                    Grid.SetRow(txtFind, 3);
                    Grid.SetRow(FindCommand, 3);
                    Grid.SetRow(Result, 3);
                    Grid.SetRow(lbl_Status, 3);
                    Grid.SetRow(tabDynamic, 4);
                    Grid.SetRowSpan(tabDynamic, 6);
                }
                else
                {
                    Grid.SetRowSpan(lbl_About, 2);
                    Grid.SetRow(Keyword, 1);
                    Grid.SetRow(txtFind, 1);
                    Grid.SetRow(FindCommand, 1);
                    Grid.SetRow(Result, 1);
                    Grid.SetRow(lbl_Status, 1);
                    Grid.SetRow(tabDynamic, 2);
                    Grid.SetRowSpan(tabDynamic, 9);
                }
                */
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem typeItem = (ComboBoxItem)ComboBox.SelectedItem;
            string aux9 = typeItem.Content.ToString();
            if (aux9 == "Processes and DLLs")
            {
                ComboBoxDependent.IsEnabled = true;
                advanceToogle = null;
                disableAdvanceStuff();
                ComboBoxDependent.Items.Clear();
                ComboBoxDependent.Items.Add("PSList");
                ComboBoxDependent.Items.Add("PSTree");
                ComboBoxDependent.Items.Add("PSScan");
                ComboBoxDependent.Items.Add("DLLList");
                ComboBoxDependent.Items.Add("Handles");
                ComboBoxDependent.Items.Add("GetSids");
                ComboBoxDependent.Items.Add("CMDScan");
                ComboBoxDependent.Items.Add("Consoles");
                ComboBoxDependent.Items.Add("Privs");
                ComboBoxDependent.Items.Add("Envars");
                ComboBoxDependent.Items.Add("VerInfo");
            }
            else if (aux9 == "Process Memory")
            {
                ComboBoxDependent.IsEnabled = true;
                advanceToogle = null;
                disableAdvanceStuff();
                ComboBoxDependent.Items.Clear();
                ComboBoxDependent.Items.Add("MemMap");
                ComboBoxDependent.Items.Add("VadInfo");
                ComboBoxDependent.Items.Add("VadWalk");
                ComboBoxDependent.Items.Add("IEHistory");
            }
            else if (aux9 == "Kernel Memory and Objects")
            {
                ComboBoxDependent.IsEnabled = true;
                advanceToogle = null;
                disableAdvanceStuff();
                ComboBoxDependent.Items.Clear();
                ComboBoxDependent.Items.Add("Modules");
                ComboBoxDependent.Items.Add("ModScan");
                ComboBoxDependent.Items.Add("Ssdt");
                ComboBoxDependent.Items.Add("DriverScan");
                ComboBoxDependent.Items.Add("FileScan");
                ComboBoxDependent.Items.Add("MutantScan");
                ComboBoxDependent.Items.Add("SymLinkScan");
                ComboBoxDependent.Items.Add("ThrdScan");
                ComboBoxDependent.Items.Add("UnloadedModules");
            }
            else if (aux9 == "Networking")
            {
                ComboBoxDependent.IsEnabled = true;
                advanceToogle = null;
                disableAdvanceStuff();
                ComboBoxDependent.Items.Clear();
                ComboBoxDependent.Items.Add("Connections");
                ComboBoxDependent.Items.Add("ConnScan");
                ComboBoxDependent.Items.Add("Sockets");
                ComboBoxDependent.Items.Add("NetScan");
            }
            else if (aux9 == "Registry")
            {
                ComboBoxDependent.IsEnabled = true;
                advanceToogle = null;
                disableAdvanceStuff();
                ComboBoxDependent.Items.Clear();
                ComboBoxDependent.Items.Add("HiveList");
                ComboBoxDependent.Items.Add("PrintKey");
                ComboBoxDependent.Items.Add("HashDump");
                ComboBoxDependent.Items.Add("LSADump");
                ComboBoxDependent.Items.Add("UserAssist");
                ComboBoxDependent.Items.Add("ShellBags");
                ComboBoxDependent.Items.Add("ShimCache");
                ComboBoxDependent.Items.Add("GetServiceSIDs");
            }
            else if (aux9 == "Crash Dumps, Hibernation, and Conversion")
            {
                ComboBoxDependent.IsEnabled = true;
                advanceToogle = null;
                disableAdvanceStuff();
                ComboBoxDependent.Items.Clear();
                ComboBoxDependent.Items.Add("CrashInfo");
                ComboBoxDependent.Items.Add("HibInfo");
                ComboBoxDependent.Items.Add("VboxInfo");
                ComboBoxDependent.Items.Add("VMWareInfo");
            }
            else if (aux9 == "File System/Miscellaneous")
            {
                ComboBoxDependent.IsEnabled = true;
                advanceToogle = null;
                disableAdvanceStuff();
                ComboBoxDependent.Items.Clear();
                ComboBoxDependent.Items.Add("mftparser");
                ComboBoxDependent.Items.Add("BIOSkbd");
                ComboBoxDependent.Items.Add("TimeLiner");
            }
            else if (aux9 == "GUI")
            {
                ComboBoxDependent.IsEnabled = true;
                advanceToogle = null;
                disableAdvanceStuff();
                ComboBoxDependent.Items.Clear();
                ComboBoxDependent.Items.Add("Sessions");
                ComboBoxDependent.Items.Add("WNDScan");
                ComboBoxDependent.Items.Add("DeskScan");
                ComboBoxDependent.Items.Add("AtomScan");
                ComboBoxDependent.Items.Add("Atoms");
                ComboBoxDependent.Items.Add("Clipboard");
                ComboBoxDependent.Items.Add("EventHooks");
                ComboBoxDependent.Items.Add("Gahti");
                ComboBoxDependent.Items.Add("MessageHooks");
                ComboBoxDependent.Items.Add("UserHandles");
                ComboBoxDependent.Items.Add("GdiTimers");
                ComboBoxDependent.Items.Add("Windows");
                ComboBoxDependent.Items.Add("WinTree");
            }
            else if (aux9 == "MAL")
            {
                ComboBoxDependent.IsEnabled = true;
                advanceToogle = null;
                disableAdvanceStuff();
                ComboBoxDependent.Items.Clear();
                ComboBoxDependent.Items.Add("MalFind");
                ComboBoxDependent.Items.Add("SVCScan");
                ComboBoxDependent.Items.Add("LDRModules");
                ComboBoxDependent.Items.Add("APIHooks");
                ComboBoxDependent.Items.Add("IDT");
                ComboBoxDependent.Items.Add("GDT");
                ComboBoxDependent.Items.Add("Threads");
                ComboBoxDependent.Items.Add("Callbacks");
                ComboBoxDependent.Items.Add("DriverIRP");
                ComboBoxDependent.Items.Add("DeviceTree");
                ComboBoxDependent.Items.Add("PSXView");
                ComboBoxDependent.Items.Add("Timers");
                ComboBoxDependent.Items.Add("IMPScan");
            }
            else if (aux9 == "Advance Commands")
            {
                ComboBoxDependent.IsEnabled = true;
                //RunCommand.Margin = new Thickness(684, 149, 1121, 892);
                ComboBoxDependent.Items.Clear();
                ComboBoxDependent.Items.Add("HiveDump");
                ComboBoxDependent.Items.Add("Advance_MemMap");
                ComboBoxDependent.Items.Add("Advance_VadInfo");
                ComboBoxDependent.Items.Add("Advance_VadWalk");
                ComboBoxDependent.Items.Add("Advance_mftparser");
                ComboBoxDependent.Items.Add("Advance_TimeLiner");
                ComboBoxDependent.Items.Add("Advance_IMPScan");
                advanceToogle = "something";
                advanceCommandButton.Visibility = Visibility.Visible;
                advanceCommandInput.Visibility = Visibility.Visible;
                advanceCommandLabel.Visibility = Visibility.Visible;
                advanceCommandButton.IsEnabled = true;
                advanceCommandInput.IsEnabled = true;
                advanceCommandLabel.IsEnabled = true;
                lbl_About.Text = "Now you can write your own commands following the Volatility Commands Guidelines. If you want to write your own commands please don't select any option from the list";
                lbl_About.Text += Environment.NewLine;
                lbl_About.Text += Environment.NewLine;
                lbl_About.Text += "If the command you want to input has parameters please input them like you would on Volatility like 'dlllist -p xxx'";
                lbl_About.Text += Environment.NewLine;
                lbl_About.Text += Environment.NewLine;
                lbl_About.Text += "The Selection Box as also been field with more advance commands that depend on other conditions";

                /*
                //cenas de mover
                Grid.SetRowSpan(lbl_About, 4);
                Grid.SetRow(Keyword, 3);
                Grid.SetRow(txtFind, 3);
                Grid.SetRow(FindCommand, 3);
                Grid.SetRow(Result, 3);
                Grid.SetRow(lbl_Status, 3);
                Grid.SetRow(tabDynamic, 4);
                Grid.SetRowSpan(tabDynamic, 7);
                g10.Visibility = Visibility.Visible;
                g11.Visibility = Visibility.Visible;
                g12.Visibility = Visibility.Visible;
                g20.Visibility = Visibility.Visible;
                g21.Visibility = Visibility.Visible;
                g22.Visibility = Visibility.Visible;
                */
            }
        }

        private void ComboBoxDependent_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //falta meter texto nas cenas e quando se escolhe o pid ou outro adicionar info
            if (ComboBoxDependent.SelectedValue != null)
            {
                string aux8 = ComboBoxDependent.SelectedValue.ToString();

                switch (aux8)
                {
                    case "PSList":
                        lbl_About.Text = "List the processes of a system. This walks the doubly-linked list pointed to by PsActiveProcessHead and shows the offset, process name, process ID, the parent process ID, number of threads, number of handles, and date/time when the process started and exited. This plugin does not detect hidden or unlinked processes (but psscan can do that). Note the two processes System and smss.exe will not have a Session ID, because System starts before sessions are established and smss.exe is the session manager itself.";
                        lbl_About.BorderBrush = Brushes.Black;
                        sandImg.Visibility = Visibility.Hidden;
                        break;
                    case "PSTree":
                        lbl_About.Text = "View the process listing in tree form. This enumerates processes using the same technique as pslist, so it will also not show hidden or unlinked processes. Child process are indicated using indention and periods.";
                        lbl_About.BorderBrush = Brushes.Black;
                        sandImg.Visibility = Visibility.Hidden;
                        break;
                    case "PSScan":
                        lbl_About.Text = "Enumerate processes using pool tag scanning (_POOL_HEADER). This can find processes that previously terminated (inactive) and processes that have been hidden or unlinked by a rootkit. The downside is that rootkits can still hide by overwriting the pool tag values (though not commonly seen in the wild).";
                        lbl_About.BorderBrush = Brushes.Black;
                        sandImg.Visibility = Visibility.Hidden;
                        break;
                    case "DLLList":
                        lbl_About.Text = "To display a process's loaded DLLs. It walks the doubly-linked list of _LDR_DATA_TABLE_ENTRY structures which is pointed to by the PEB's InLoadOrderModuleList. DLLs are automatically added to this list when a process calls LoadLibrary (or some derivative such as LdrLoadDll) and they aren't removed until FreeLibrary is called and the reference count reaches zero.";
                        lbl_About.BorderBrush = Brushes.Black;
                        sandImg.Visibility = Visibility.Hidden;
                        break;
                    case "Handles":
                        lbl_About.Text = "To display the open handles in a process. This applies to files, registry keys, mutexes, named pipes, events, window stations, desktops, threads, and all other types of securable executive objects.";
                        lbl_About.BorderBrush = Brushes.Black;
                        sandImg.Visibility = Visibility.Hidden;
                        break;
                    case "GetSids":
                        lbl_About.Text = "To view the SIDs (Security Identifiers) associated with a process. Among other things, this can help you identify processes which have maliciously escalated privileges and which processes belong to specific users.";
                        lbl_About.BorderBrush = Brushes.Black;
                        sandImg.Visibility = Visibility.Hidden;
                        break;
                    case "CMDScan":
                        lbl_About.Text = "The cmdscan plugin searches the memory of csrss.exe on XP/2003/Vista/2008 and conhost.exe on Windows 7 for commands that attackers entered through a console shell (cmd.exe). This is one of the most powerful commands you can use to gain visibility into an attackers actions on a victim system, whether they opened cmd.exe through an RDP session or proxied input/output to a command shell from a networked backdoor.This plugin finds structures known as COMMAND_HISTORY by looking for a known constant value(MaxHistory) and then applying sanity checks. ";
                        lbl_About.BorderBrush = Brushes.Black;
                        sandImg.Visibility = Visibility.Hidden;
                        break;
                    case "Consoles":
                        lbl_About.Text = "Similar to cmdscan the consoles plugin finds commands that attackers typed into cmd.exe or executed via backdoors. However, instead of scanning for COMMAND_HISTORY, this plugin scans for CONSOLE_INFORMATION. The major advantage to this plugin is it not only prints the commands attackers typed, but it collects the entire screen buffer (input and output).";
                        lbl_About.BorderBrush = Brushes.Black;
                        sandImg.Visibility = Visibility.Hidden;
                        break;
                    case "Privs":
                        lbl_About.Text = "This plugin shows you which process privileges are present, enabled, and/or enabled by default.";
                        lbl_About.BorderBrush = Brushes.Black;
                        sandImg.Visibility = Visibility.Hidden;
                        break;
                    case "Envars":
                        lbl_About.Text = "To display a process's environment variables. Typically this will show the number of CPUs installed and the hardware architecture (though the kdbgscan output is a much more reliable source), the process's current directory, temporary directory, session name, computer name, user name, and various other interesting artifacts.";
                        lbl_About.BorderBrush = Brushes.Black;
                        sandImg.Visibility = Visibility.Hidden;
                        break;
                    case "VerInfo":
                        lbl_About.Text = "To display the version information embedded in PE files. Not all PE files have version information, and many malware authors forge it to include false data, but nonetheless this command can be very helpful with identifying binaries and for making correlations with other files.";
                        lbl_About.BorderBrush = Brushes.Red;
                        sandImg.Visibility = Visibility.Visible;
                        break;
                    case "IEHistory":
                        lbl_About.Text = "This plugin recovers fragments of IE history index.dat cache files. It can find basic accessed links (via FTP or HTTP), redirected links (--REDR), and deleted entries (--LEAK). It applies to any process which loads and uses the wininet.dll library, not just Internet Explorer. Typically that includes Windows Explorer and even malware samples. ";
                        lbl_About.BorderBrush = Brushes.Black;
                        sandImg.Visibility = Visibility.Hidden;
                        break;
                    case "Modules":
                        lbl_About.Text = "To view the list of kernel drivers loaded on the system, use the modules command. This walks the doubly-linked list of LDR_DATA_TABLE_ENTRY structures pointed to by PsLoadedModuleList. Similar to the pslist command, this relies on finding the KDBG structure. In rare cases, you may need to use kdbgscan to find the most appropriate KDBG structure address and then supply it to this plugin like --kdbg=ADDRESS.It cannot find hidden/ unlinked kernel drivers, however modscan serves that purpose.";
                        lbl_About.BorderBrush = Brushes.Black;
                        sandImg.Visibility = Visibility.Hidden;
                        break;
                    case "ModScan":
                        lbl_About.Text = "The modscan command finds LDR_DATA_TABLE_ENTRY structures by scanning physical memory for pool tags. This can pick up previously unloaded drivers and drivers that have been hidden/unlinked by rootkits. Unlike modules the order of results has no relationship with the order in which the drivers loaded. As you can see below, DumpIt.sys was found at the lowest physical offset, but it was probably one of the last drivers to load (since it was used to acquire memory).";
                        lbl_About.BorderBrush = Brushes.Black;
                        sandImg.Visibility = Visibility.Hidden;
                        break;
                    case "Ssdt":
                        lbl_About.Text = "To list the functions in the Native and GUI SSDTs, use the ssdt command. This displays the index, function name, and owning driver for each entry in the SSDT.";
                        lbl_About.BorderBrush = Brushes.Black;
                        sandImg.Visibility = Visibility.Hidden;
                        break;
                    case "DriverScan":
                        lbl_About.Text = "To find DRIVER_OBJECTs in physical memory using pool tag scanning, use the driverscan command. This is another way to locate kernel modules, although not all kernel modules have an associated DRIVER_OBJECT. The DRIVER_OBJECT is what contains the 28 IRP (Major Function) tables, thus the driverirp command is based on the methodology used by driverscan.";
                        lbl_About.BorderBrush = Brushes.Black;
                        sandImg.Visibility = Visibility.Hidden;
                        break;
                    case "FileScan":
                        lbl_About.Text = "To find FILE_OBJECTs in physical memory using pool tag scanning. This will find open files even if a rootkit is hiding the files on disk and if the rootkit hooks some API functions to hide the open handles on a live system. The output shows the physical offset of the FILE_OBJECT, file name, number of pointers to the object, number of handles to the object, and the effective permissions granted to the object.";
                        lbl_About.BorderBrush = Brushes.Black;
                        sandImg.Visibility = Visibility.Hidden;
                        break;
                    case "MutantScan":
                        lbl_About.Text = "To scan physical memory for KMUTANT objects with pool tag scanning. By default, it displays all objects, but you can pass -s or --silent to only show named mutexes. The CID column contains the process ID and thread ID of the mutex owner if one exists.";
                        lbl_About.BorderBrush = Brushes.Black;
                        sandImg.Visibility = Visibility.Hidden;
                        break;
                    case "SymLinkScan":
                        lbl_About.Text = "This plugin scans for symbolic link objects and outputs their information. In the past, this has been used to link drive letters (i.e. D:, E:, F:, etc) to true crypt volumes (i.e. \\Device\\TrueCryptVolume).";
                        lbl_About.BorderBrush = Brushes.Black;
                        sandImg.Visibility = Visibility.Hidden;
                        break;
                    case "ThrdScan":
                        lbl_About.Text = "To find ETHREAD objects in physical memory with pool tag scanning. Since an ETHREAD contains fields that identify its parent process, you can use this technique to find hidden processes. One such use case is documented in the psxview command. Also, for verbose details, try the threads plugin.";
                        lbl_About.BorderBrush = Brushes.Black;
                        sandImg.Visibility = Visibility.Hidden;
                        break;
                    case "UnloadedModules":
                        lbl_About.Text = "Windows stores information on recently unloaded drivers for debugging purposes. This gives you an alternative way to determine what happened on a system, besides the well known modules and modscan plugins.";
                        lbl_About.BorderBrush = Brushes.Black;
                        sandImg.Visibility = Visibility.Hidden;
                        break;
                    case "Connections":
                        lbl_About.Text = "To view TCP connections that were active at the time of the memory acquisition. This walks the singly-linked list of connection structures pointed to by a non-exported symbol in the tcpip.sys module.This command is for x86 and x64 Windows XP and Windows 2003 Server only.";
                        lbl_About.BorderBrush = Brushes.Black;
                        sandImg.Visibility = Visibility.Hidden;
                        break;
                    case "ConnScan":
                        lbl_About.Text = "To find _TCPT_OBJECT structures using pool tag scanning. This can find artifacts from previous connections that have since been terminated, in addition to the active ones. In the output below, you'll notice some fields have been partially overwritten, but some of the information is still accurate. For example, the very last entry's Pid field is 0, but all other fields are still in tact. Thus, while it may find false positives sometimes, you also get the benefit of detecting as much information as possible.This command is for x86 and x64 Windows XP and Windows 2003 Server only.";
                        lbl_About.BorderBrush = Brushes.Black;
                        sandImg.Visibility = Visibility.Hidden;
                        break;
                    case "Sockets":
                        lbl_About.Text = "To detect listening sockets for any protocol (TCP, UDP, RAW, etc). This walks a singly-linked list of socket structures which is pointed to by a non-exported symbol in the tcpip.sys module.This command is for x86 and x64 Windows XP and Windows 2003 Server only.";
                        lbl_About.BorderBrush = Brushes.Black;
                        sandImg.Visibility = Visibility.Hidden;
                        break;
                    case "NetScan":
                        lbl_About.Text = "To scan for network artifacts in 32- and 64-bit Windows Vista, Windows 2008 Server and Windows 7 memory dumps. This finds TCP endpoints, TCP listeners, UDP endpoints, and UDP listeners. It distinguishes between IPv4 and IPv6, prints the local and remote IP (if applicable), the local and remote port (if applicable), the time when the socket was bound or when the connection was established, and the current state (for TCP connections only).";
                        lbl_About.BorderBrush = Brushes.Black;
                        sandImg.Visibility = Visibility.Hidden;
                        break;
                    case "HiveList":
                        lbl_About.Text = "To locate the virtual addresses of registry hives in memory, and the full paths to the corresponding hive on disk. If you want to print values from a certain hive, run this command first so you can see the address of the hives.";
                        lbl_About.BorderBrush = Brushes.Black;
                        sandImg.Visibility = Visibility.Hidden;
                        break;
                    case "PrintKey":
                        lbl_About.Text = "To display the subkeys, values, data, and data types contained within a specified registry key. By default, printkey will search all hives and print the key information (if found) for the requested key. Therefore, if the key is located in more than one hive, the information for the key will be printed for each hive that contains it.";
                        lbl_About.BorderBrush = Brushes.Black;
                        sandImg.Visibility = Visibility.Hidden;
                        break;
                    case "HashDump":
                        lbl_About.Text = "To extract and decrypt cached domain credentials stored in the registry. Hashes can now be cracked using John the Ripper, rainbow tables, etc.";
                        if (hiveSYSTEM == null && hiveSAM == null) // aqui (hiveSYSTEM == null && hiveSAM == null)
                        {
                            lbl_About.Text += " Currently retriving SAM & SYSTEM Values";
                            commandCB = "hivelist";
                            hashToogle = 1;
                            runCommand();
                        }
                        lbl_About.BorderBrush = Brushes.Black;
                        sandImg.Visibility = Visibility.Hidden;
                        break;
                    case "LSADump":
                        lbl_About.Text = "To dump LSA secrets from the registry. This exposes information such as the default password (for systems with autologin enabled), the RDP public key, and credentials used by DPAPI.";
                        lbl_About.BorderBrush = Brushes.Black;
                        sandImg.Visibility = Visibility.Hidden;
                        break;
                    case "UserAssist":
                        lbl_About.Text = "To get the UserAssist keys from a sample.";
                        lbl_About.BorderBrush = Brushes.Black;
                        sandImg.Visibility = Visibility.Hidden;
                        break;
                    case "ShellBags":
                        lbl_About.Text = "This plugin parses and prints Shellbag information obtained from the registry.";
                        lbl_About.BorderBrush = Brushes.Black;
                        sandImg.Visibility = Visibility.Hidden;
                        break;
                    case "ShimCache":
                        lbl_About.Text = "This plugin parses the Application Compatibility Shim Cache registry key.";
                        lbl_About.BorderBrush = Brushes.Black;
                        sandImg.Visibility = Visibility.Hidden;
                        break;
                    case "GetServiceSIDs":
                        lbl_About.Text = "The getservicesids command calculates the SIDs for services on a machine and outputs them in Python dictionary format for future use. The service names are taken from the registry (\"SYSTEM\\CurrentControlSet\\Services\").";
                        lbl_About.BorderBrush = Brushes.Red;
                        sandImg.Visibility = Visibility.Visible;
                        break;
                    case "CrashInfo":
                        lbl_About.Text = "Information from the crashdump header can be printed using the crashinfo command. You will see information like that of the Microsoft dumpcheck utility.";
                        lbl_About.BorderBrush = Brushes.Black;
                        sandImg.Visibility = Visibility.Hidden;
                        break;
                    case "HibInfo":
                        lbl_About.Text = "The hibinfo command reveals additional information stored in the hibernation file, including the state of the Control Registers, such as CR0, etc. It also identifies the time at which the hibernation file was created, the state of the hibernation file, and the version of windows being hibernated.";
                        lbl_About.BorderBrush = Brushes.Black;
                        sandImg.Visibility = Visibility.Hidden;
                        break;
                    case "VboxInfo":
                        lbl_About.Text = "To pull details from a virtualbox core dump.";
                        lbl_About.BorderBrush = Brushes.Black;
                        sandImg.Visibility = Visibility.Hidden;
                        break;
                    case "VMWareInfo":
                        lbl_About.Text = "Use this plugin to analyze header information from vmware saved state (vmss) or vmware snapshot (vmsn) files. The metadata contains CPU registers, the entire VMX configuration file, memory run information, and PNG screenshots of the guest VM.";
                        lbl_About.BorderBrush = Brushes.Black;
                        sandImg.Visibility = Visibility.Hidden;
                        break;
                    case "Sessions":
                        lbl_About.Text = "This command analyzes the unique _MM_SESSION_SPACE objects and prints details related to the processes running in each logon session, mapped drivers, paged/non-paged pools etc. The alternate process lists output by this plugin are leveraged by the psxview plugin for rootkit detection.";
                        lbl_About.BorderBrush = Brushes.Black;
                        sandImg.Visibility = Visibility.Hidden;
                        break;
                    case "WNDScan":
                        lbl_About.Text = "This command scans for tagWINDOWSTATION objects and prints details on the window station, its global atom table, available clipboard formats, and processes or threads currently interacting with the clipboard.";
                        lbl_About.BorderBrush = Brushes.Black;
                        sandImg.Visibility = Visibility.Hidden;
                        break;
                    case "DeskScan":
                        lbl_About.Text = "This command subclasses the wndscan plugin and for each window station found, it walks the list of desktops. It can be used for the following purposes. Find rogue desktops used to hide applications from logged-on users. Detect desktops created by ransomware. Link threads to their desktops. Analyze the desktop heap for memory corruptions. Profile dekstop heap allocations to locate USER objects.";
                        lbl_About.BorderBrush = Brushes.Black;
                        sandImg.Visibility = Visibility.Hidden;
                        break;
                    case "AtomScan":
                        lbl_About.Text = "This command scans physical memory for atom tables. For each table found, it enumerates the bucket of atoms - including session global atoms and window station global atoms. It does not include process local atoms.";
                        lbl_About.BorderBrush = Brushes.Black;
                        sandImg.Visibility = Visibility.Hidden;
                        break;
                    case "Atoms":
                        lbl_About.Text = "This command is similar to atomscan, but it allows us to associate atom tables with their owning window station. We need this command in conjunction with atomscan because there are many reasons an atom must be tied to its session or window station";
                        lbl_About.BorderBrush = Brushes.Black;
                        sandImg.Visibility = Visibility.Hidden;
                        break;
                    case "Clipboard":
                        lbl_About.Text = "This command recovers data from users' clipboards. It walks the array of tagCLIP objects pointed to by tagWINDOWSTATION.pClipBase and takes the format (i.e. unicode, ansi, ole, bmp) and the handle value. Then it walks the USER handle table (also see the userhandles plugin) and filters for TYPE_CLIPDATA objects. It matches the handle value of those objects with the handles from tagCLIP so that a format can be associated with the raw data.";
                        lbl_About.BorderBrush = Brushes.Black;
                        sandImg.Visibility = Visibility.Hidden;
                        break;
                    case "EventHooks":
                        lbl_About.Text = "This command enumerates event hooks installed via the SetWinEventHook API. It prints the minimum and maximum event IDs to which the hook applies, the targeted threads, owning processes, and offset to the hook procedure.";
                        lbl_About.BorderBrush = Brushes.Black;
                        sandImg.Visibility = Visibility.Hidden;
                        break;
                    case "Gahti":
                        lbl_About.Text = "This command uses an algorithmic approach to finding the win32kgahti symbol which is an array of tagHANDLETYPEINFO structures - one for each type of USER object for the system. Windows XP has typically 20 objects and Windows 7 has 22, including TYPE_FREE. The plugin shows you the 4-byte tag associated with allocations, where the objects are allocated from (desktop heap, shared heap, session pool), and how the objects are owned (thread owned, process owned, or anonymous).";
                        lbl_About.BorderBrush = Brushes.Black;
                        sandImg.Visibility = Visibility.Hidden;
                        break;
                    case "MessageHooks":
                        lbl_About.Text = "This command prints both local and global message hooks, installed via SetWindowsHookEx APIs. This is a common trick used by malware to inject code into other processes and log keystrokes, record mouse movements, etc.";
                        lbl_About.BorderBrush = Brushes.Black;
                        sandImg.Visibility = Visibility.Hidden;
                        break;
                    case "UserHandles":
                        lbl_About.Text = "This command locates the session-specific tagSHAREDINFO structure, walks the aheList member (an array of _HANDLEENTRY) structures. It determines if each handle entry is thread or process owned, shows the object type, and its offset in session space. This plugin is not very verbose, its just meant to show an overview of the USER objects currently in use by each thread or process; and it serves as an API for other plugins that do want verbose details on an object type.";
                        lbl_About.BorderBrush = Brushes.Black;
                        sandImg.Visibility = Visibility.Hidden;
                        break;
                    case "GdiTimers":
                        lbl_About.Text = "This command leverages the USER handle table API as described above and for each TYPE_TIMER, it dereferences the object as a tagTIMER and prints details on the fields. Malware uses timers often to schedule routine functions, such as contacting a C2 server or making sure a hidden process remains hidden.";
                        lbl_About.BorderBrush = Brushes.Black;
                        sandImg.Visibility = Visibility.Hidden;
                        break;
                    case "Windows":
                        lbl_About.Text = "This command enumerates all windows (visible or not) in all desktops of the system. It walks windows in their Z-Order (i.e. front to back focus) starting at the desktops spwnd value (the foreground window). For each window it shows details on the window's title, class atoms, the owning thread and process, the visibility properties, the left/right/top/bottom coordinates, the flags and ex-flags, and the window procedure address.";
                        lbl_About.BorderBrush = Brushes.Black;
                        sandImg.Visibility = Visibility.Hidden;
                        break;
                    case "WinTree":
                        lbl_About.Text = "This command enumerates windows in the same way as the windows command, but it prints less verbose details so that the parent/child relationship can be easily expressed in a tree form. Instead of a \"flat\" view, you can see which windows are contained within other windows.";
                        lbl_About.BorderBrush = Brushes.Black;
                        sandImg.Visibility = Visibility.Hidden;
                        break;
                    case "MalFind":
                        lbl_About.Text = "The malfind command helps find hidden or injected code/DLLs in user mode memory, based on characteristics such as VAD tag and page permissions. Note: malfind does not detect DLLs injected into a process using CreateRemoteThread->LoadLibrary.DLLs injected with this technique are not hidden and thus you can view them with dlllist. The purpose of malfind is to locate DLLs that standard methods/ tools do not see.";
                        lbl_About.BorderBrush = Brushes.Black;
                        sandImg.Visibility = Visibility.Hidden;
                        break;
                    case "SVCScan":
                        lbl_About.Text = "Volatility is the only memory forensics framework with the ability to list services without using the Windows API on a live machine. To see which services are registered on your memory image, use the svcscan command. The output shows the process ID of each service (if its active and pertains to a usermode process), the service name, service display name, service type, and current status. It also shows the binary path for the registered service - which will be an EXE for usermode services and a driver name for services that run from kernel mode.";
                        lbl_About.BorderBrush = Brushes.Black;
                        sandImg.Visibility = Visibility.Hidden;
                        break;
                    case "LDRModules":
                        lbl_About.Text = "There are many ways to hide a DLL. One of the ways involves unlinking the DLL from one (or all) of the linked lists in the PEB. However, when this is done, there is still information contained within the VAD (Virtual Address Descriptor) which identifies the base address of the DLL and its full path on disk. To cross-reference this information (known as memory mapped files) with the 3 PEB lists.";
                        lbl_About.BorderBrush = Brushes.Black;
                        sandImg.Visibility = Visibility.Hidden;
                        break;
                    case "apihooks":
                        lbl_About.Text = "To find API hooks in user mode or kernel mode. This finds IAT, EAT, Inline style hooks, and several special types of hooks. For Inline hooks, it detects CALLs and JMPs to direct and indirect locations, and it detects PUSH/RET instruction sequences. It also detects CALLs or JMPs to registers after an immediate value (address) is moved into the register. The special types of hooks that it detects include syscall hooking in ntdll.dll and calls to unknown code pages in kernel memory.";
                        lbl_About.BorderBrush = Brushes.Red;
                        sandImg.Visibility = Visibility.Visible;
                        break;
                    case "IDT":
                        lbl_About.Text = "To print the system's IDT (Interrupt Descriptor Table). If there are multiple processors on the system, the IDT for each individual CPU is displayed. You'll see the CPU number, the GDT selector, the current address and owning module, and the name of the PE section in which the IDT function resides. If you supply the --verbose parameter, a disassembly of the IDT function will be shown.";
                        lbl_About.BorderBrush = Brushes.Black;
                        sandImg.Visibility = Visibility.Hidden;
                        break;
                    case "GDT":
                        lbl_About.Text = "To print the system's GDT (Global Descriptor Table). This is useful for detecting rootkits like Alipop that install a call gate so that user mode programs can call directly into kernel mode (using a CALL FAR instruction). If your system has multiple CPUs, the GDT for each processor is shown.";
                        lbl_About.BorderBrush = Brushes.Black;
                        sandImg.Visibility = Visibility.Hidden;
                        break;
                    case "Threads":
                        lbl_About.Text = "The command gives you extensive details on threads, including the contents of each thread's registers (if available), a disassembly of code at the thread's start address, and various other fields that may be relevant to an investigation.";
                        lbl_About.BorderBrush = Brushes.Black;
                        sandImg.Visibility = Visibility.Hidden;
                        break;
                    case "Callbacks":
                        lbl_About.Text = "Volatility is the only memory forensics platform with the ability to print an assortment of important notification routines and kernel callbacks. Rootkits, anti-virus suites, dynamic analysis tools (such as Sysinternals' Process Monitor and Tcpview), and many components of the Windows kernel use of these callbacks to monitor and/or react to events. Detect the following. PsSetCreateProcessNotifyRoutine (process creation). PsSetCreateThreadNotifyRoutine (thread creation). PsSetImageLoadNotifyRoutine (DLL/image load). IoRegisterFsRegistrationChange (file system registration). KeRegisterBugCheck and KeRegisterBugCheckReasonCallback. CmRegisterCallback (registry callbacks on XP). CmRegisterCallbackEx (registry callbacks on Vista and 7). IoRegisterShutdownNotification (shutdown callbacks). DbgSetDebugPrintCallback (debug print callbacks on Vista and 7). DbgkLkmdRegisterCallback (debug callbacks on 7).";
                        lbl_About.BorderBrush = Brushes.Black;
                        sandImg.Visibility = Visibility.Hidden;
                        break;
                    case "DriverIRP":
                        lbl_About.Text = "To print a driver's IRP (Major Function) table. This command inherits from driverscan so that its able to locate DRIVER_OBJECTs. Then it cycles through the function table, printing the purpose of each function, the function's address, and the owning module of the address.";
                        break;
                    case "DeviceTree":
                        lbl_About.Text = "Windows uses a layered driver architecture, or driver chain so that multiple drivers can inspect or respond to an IRP. Rootkits often insert drivers (or devices) into this chain for filtering purposes (to hide files, hide network connections, steal keystrokes or mouse movements). The devicetree plugin shows the relationship of a driver object to its devices (by walking _DRIVER_OBJECT.DeviceObject.NextDevice) and any attached devices (_DRIVER_OBJECT.DeviceObject.AttachedDevice).";
                        lbl_About.BorderBrush = Brushes.Black;
                        sandImg.Visibility = Visibility.Hidden;
                        break;
                    case "PSXView":
                        lbl_About.Text = "This plugin helps you detect hidden processes by comparing what PsActiveProcessHead contains with what is reported by various other sources of process listings. It compares the following. PsActiveProcessHead linked list. EPROCESS pool scanning. ETHREAD pool scanning (then it references the owning EPROCESS). PspCidTable. Csrss.exe handle table. Csrss.exe internal linked list.";
                        lbl_About.BorderBrush = Brushes.Black;
                        sandImg.Visibility = Visibility.Hidden;
                        break;
                    case "Timers":
                        lbl_About.Text = "This command prints installed kernel timers (KTIMER) and any associated DPCs (Deferred Procedure Calls). Rootkits such as Zero Access, Rustock, and Stuxnet register timers with a DPC. Although the malware tries to be stealthy and hide in kernel space in a number of different ways, by finding the KTIMERs and looking at the address of the DPC, you can quickly find the malicious code ranges.";
                        lbl_About.BorderBrush = Brushes.Black;
                        sandImg.Visibility = Visibility.Hidden;
                        break;
                    case "HiveDump":
                        lbl_About.Text = "To recursively list all subkeys in a hive, use the hivedump command and pass it the virtual address to the desired hive choosen from the PID ComboBox.";
                        ComboBoxPID.Visibility = Visibility.Hidden;
                        ComboBoxPID.IsEnabled = false;
                        lbl_PID.Visibility = Visibility.Hidden;
                        ComboBoxType.Visibility = Visibility.Hidden;
                        ComboBoxType.IsEnabled = false;
                        lbl_Type.Visibility = Visibility.Hidden;
                        TextBoxOffset.Visibility = Visibility.Hidden;
                        TextBoxOffset.IsEnabled = false;
                        lbl_Offset.Visibility = Visibility.Hidden;
                        ComboBoxOffset.Visibility = Visibility.Visible;
                        ComboBoxOffset.IsEnabled = true;
                        lbl_CBOffset.Visibility = Visibility.Visible;
                        if (ComboBoxOffset.Items.Count == 0)
                        {
                            commandCB = "hivelist";
                            ComboBoxDependent.IsEnabled = false;
                            ComboBoxOffset.IsEnabled = false;
                            runCommand();
                        }
                        lbl_About.BorderBrush = Brushes.Red;
                        sandImg.Visibility = Visibility.Visible;
                        break;
                    case "Advance_MemMap":
                        lbl_About.Text = "The memmap command shows you exactly which pages are memory resident, given a specific process DTB from the ComboBox(or kernel DTB if you use this plugin on the Idle or System process). It shows you the virtual address of the page, the corresponding physical offset of the page, and the size of the page. The map information generated by this plugin comes from the underlying address space's get_available_addresses method.";
                        ComboBoxPID.Visibility = Visibility.Visible;
                        ComboBoxPID.IsEnabled = true;
                        lbl_PID.Visibility = Visibility.Visible;
                        ComboBoxType.Visibility = Visibility.Hidden;
                        ComboBoxType.IsEnabled = false;
                        lbl_Type.Visibility = Visibility.Hidden;
                        TextBoxOffset.Visibility = Visibility.Hidden;
                        TextBoxOffset.IsEnabled = false;
                        lbl_Offset.Visibility = Visibility.Hidden;
                        ComboBoxOffset.Visibility = Visibility.Hidden;
                        ComboBoxOffset.IsEnabled = false;
                        lbl_CBOffset.Visibility = Visibility.Hidden;
                        if (ComboBoxPID.Items.Count == 0)
                        {
                            commandCB = "dlllist";
                            ComboBoxDependent.IsEnabled = false;
                            ComboBoxPID.IsEnabled = false;
                            runCommand();
                        }
                        lbl_About.BorderBrush = Brushes.Red;
                        sandImg.Visibility = Visibility.Visible;
                        break;
                    case "Advance_VadInfo":
                        lbl_About.Text = "The vadinfo command displays extended information about a process's VAD nodes. In particular, it shows. The address of the MMVAD structure in kernel memory. The starting and ending virtual addresses in process memory that the MMVAD structure pertains to. The VAD Tag. The VAD flags, control flags, etc. The name of the memory mapped file (if one exists). The memory protection constant (permissions). You must choose a PID.";
                        ComboBoxPID.Visibility = Visibility.Visible;
                        ComboBoxPID.IsEnabled = true;
                        lbl_PID.Visibility = Visibility.Visible;
                        ComboBoxType.Visibility = Visibility.Hidden;
                        ComboBoxType.IsEnabled = false;
                        lbl_Type.Visibility = Visibility.Hidden;
                        TextBoxOffset.Visibility = Visibility.Hidden;
                        TextBoxOffset.IsEnabled = false;
                        lbl_Offset.Visibility = Visibility.Hidden;
                        ComboBoxOffset.Visibility = Visibility.Hidden;
                        ComboBoxOffset.IsEnabled = false;
                        lbl_CBOffset.Visibility = Visibility.Hidden;
                        if (ComboBoxPID.Items.Count == 0)
                        {
                            commandCB = "dlllist";
                            ComboBoxDependent.IsEnabled = false;
                            ComboBoxPID.IsEnabled = false;
                            runCommand();
                        }
                        lbl_About.BorderBrush = Brushes.Red;
                        sandImg.Visibility = Visibility.Visible;
                        break;
                    case "Advance_VadWalk":
                        lbl_About.Text = "To inspect a process's VAD nodes in table form. You must input a PID.";
                        ComboBoxPID.Visibility = Visibility.Visible;
                        ComboBoxPID.IsEnabled = true;
                        lbl_PID.Visibility = Visibility.Visible;
                        ComboBoxType.Visibility = Visibility.Hidden;
                        ComboBoxType.IsEnabled = false;
                        lbl_Type.Visibility = Visibility.Hidden;
                        TextBoxOffset.Visibility = Visibility.Hidden;
                        TextBoxOffset.IsEnabled = false;
                        lbl_Offset.Visibility = Visibility.Hidden;
                        ComboBoxOffset.Visibility = Visibility.Hidden;
                        ComboBoxOffset.IsEnabled = false;
                        lbl_CBOffset.Visibility = Visibility.Hidden;
                        if (ComboBoxPID.Items.Count == 0)
                        {
                            commandCB = "dlllist";
                            ComboBoxDependent.IsEnabled = false;
                            ComboBoxPID.IsEnabled = false;
                            runCommand();
                        }
                        lbl_About.BorderBrush = Brushes.Red;
                        sandImg.Visibility = Visibility.Visible;
                        break;
                    case "Advance_mftparser":
                        lbl_About.Text = "This plugin scans for potential Master File Table (MFT) entries in memory (using \"FILE\" and \"BAAD\" signatures) and prints out information for certain attributes, currently: $FILE_NAME ($FN), $STANDARD_INFORMATION ($SI), $FN and $SI attributes from the $ATTRIBUTE_LIST, $OBJECT_ID (default output only) and resident $DATA. You must input the offset, this offset can be checked with various process";
                        ComboBoxPID.Visibility = Visibility.Hidden;
                        ComboBoxPID.IsEnabled = false;
                        lbl_PID.Visibility = Visibility.Hidden;
                        ComboBoxType.Visibility = Visibility.Hidden;
                        ComboBoxType.IsEnabled = false;
                        lbl_Type.Visibility = Visibility.Hidden;
                        TextBoxOffset.Visibility = Visibility.Visible;
                        TextBoxOffset.IsEnabled = true;
                        lbl_Offset.Visibility = Visibility.Visible;
                        ComboBoxOffset.Visibility = Visibility.Hidden;
                        ComboBoxOffset.IsEnabled = false;
                        lbl_CBOffset.Visibility = Visibility.Hidden;
                        lbl_About.BorderBrush = Brushes.Red;
                        sandImg.Visibility = Visibility.Visible;
                        break;
                    case "Advance_TimeLiner":
                        lbl_About.Text = "This timeliner plugin creates a timeline from various artifacts in memory from the following sources in the ComboBox";
                        ComboBoxType.Items.Clear();
                        ComboBoxPID.Visibility = Visibility.Hidden;
                        ComboBoxPID.IsEnabled = false;
                        lbl_PID.Visibility = Visibility.Hidden;
                        ComboBoxType.Visibility = Visibility.Visible;
                        ComboBoxType.IsEnabled = true;
                        lbl_Type.Visibility = Visibility.Visible;
                        TextBoxOffset.Visibility = Visibility.Hidden;
                        TextBoxOffset.IsEnabled = false;
                        lbl_Offset.Visibility = Visibility.Hidden;
                        ComboBoxOffset.Visibility = Visibility.Hidden;
                        ComboBoxOffset.IsEnabled = false;
                        lbl_CBOffset.Visibility = Visibility.Hidden;
                        ComboBoxType.Items.Add("ImageDate");
                        ComboBoxType.Items.Add("Process");
                        ComboBoxType.Items.Add("LoadTime");
                        ComboBoxType.Items.Add("TimeDateStamp");
                        ComboBoxType.Items.Add("Thread");
                        ComboBoxType.Items.Add("Socket");
                        ComboBoxType.Items.Add("EvtLog");
                        ComboBoxType.Items.Add("IEHistory");
                        ComboBoxType.Items.Add("_CMHIVE");
                        ComboBoxType.Items.Add("_HBASE_BLOCK");
                        ComboBoxType.Items.Add("_CM_KEY_BODY");
                        ComboBoxType.Items.Add("Userassist");
                        ComboBoxType.Items.Add("Shimcache");
                        ComboBoxType.Items.Add("Timer");
                        ComboBoxType.Items.Add("Symlink");
                        lbl_About.BorderBrush = Brushes.Black;
                        sandImg.Visibility = Visibility.Hidden;
                        break;
                    case "Advance_IMPScan":
                        lbl_About.Text = "In order to fully reverse engineer code that you find in memory dumps, its necessary to see which functions the code imports. In other words, which API functions it calls. When you dump binaries with dlldump, moddump, or procdump, the IAT (Import Address Table) may not properly be reconstructed due to the high likelihood that one or more pages in the PE header or IAT are not memory resident (paged). You must choose a PID.";
                        ComboBoxPID.Visibility = Visibility.Visible;
                        ComboBoxPID.IsEnabled = true;
                        lbl_PID.Visibility = Visibility.Visible;
                        ComboBoxType.Visibility = Visibility.Hidden;
                        ComboBoxType.IsEnabled = false;
                        lbl_Type.Visibility = Visibility.Hidden;
                        TextBoxOffset.Visibility = Visibility.Hidden;
                        TextBoxOffset.IsEnabled = false;
                        lbl_Offset.Visibility = Visibility.Hidden;
                        ComboBoxOffset.Visibility = Visibility.Hidden;
                        ComboBoxOffset.IsEnabled = false;
                        lbl_CBOffset.Visibility = Visibility.Hidden;
                        lbl_About.BorderBrush = Brushes.Red;
                        sandImg.Visibility = Visibility.Visible;
                        break;
                    case "MemMap":
                        lbl_About.Text = "The memmap command shows you exactly which pages are memory resident. It shows you the virtual address of the page, the corresponding physical offset of the page, and the size of the page. The map information generated by this plugin comes from the underlying address space's get_available_addresses method.";
                        lbl_About.BorderBrush = Brushes.Red;
                        sandImg.Visibility = Visibility.Visible;
                        break;
                    case "VadInfo":
                        lbl_About.Text = "The vadinfo command displays extended information about a process's all VAD nodes. In particular, it shows. The address of the MMVAD structure in kernel memory. The starting and ending virtual addresses in process memory that the MMVAD structure pertains to. The VAD Tag. The VAD flags, control flags, etc. The name of the memory mapped file (if one exists). The memory protection constant (permissions).";
                        lbl_About.BorderBrush = Brushes.Red;
                        sandImg.Visibility = Visibility.Visible;
                        break;
                    case "VadWalk":
                        lbl_About.Text = "To inspect a process's all VAD nodes in table form.";
                        lbl_About.BorderBrush = Brushes.Red;
                        sandImg.Visibility = Visibility.Visible;
                        break;
                    case "mftparser":
                        lbl_About.Text = "This plugin scans for potential Master File Table (MFT) entries in memory (using \"FILE\" and \"BAAD\" signatures) and prints out information for certain attributes, currently: $FILE_NAME ($FN), $STANDARD_INFORMATION ($SI), $FN and $SI attributes from the $ATTRIBUTE_LIST, $OBJECT_ID (default output only) and resident $DATA.";
                        lbl_About.BorderBrush = Brushes.Red;
                        sandImg.Visibility = Visibility.Visible;
                        break;
                    case "BIOSkbd":
                        lbl_About.Text = "To read keystrokes from the BIOS area of memory. This can reveal passwords typed into HP, Intel, and Lenovo BIOS and SafeBoot, TrueCrypt, and BitLocker software. Depending on the tool used to acquire memory, not all memory samples will contain the necessary BIOS area.";
                        lbl_About.BorderBrush = Brushes.Black;
                        sandImg.Visibility = Visibility.Hidden;
                        break;
                    case "TimeLiner":
                        lbl_About.Text = "This timeliner plugin creates a timeline from various artifacts in memory from the all the sources in the ComboBox";
                        lbl_About.BorderBrush = Brushes.Red;
                        sandImg.Visibility = Visibility.Visible;
                        break;
                    case "IMPScan":
                        lbl_About.Text = "In order to fully reverse engineer code that you find in memory dumps, its necessary to see which functions the code imports. In other words, which API functions it calls. When you dump binaries with dlldump, moddump, or procdump, the IAT (Import Address Table) may not properly be reconstructed due to the high likelihood that one or more pages in the PE header or IAT are not memory resident (paged).";
                        lbl_About.BorderBrush = Brushes.Red;
                        sandImg.Visibility = Visibility.Visible;
                        break;
                }
            }
            else
            {
                lbl_About.Text = "";
            }

        }

        private void advanceCommandButton_Click(object sender, RoutedEventArgs e)
        {
            if (advanceCommandInput.Text != null)
            {
                commandCB = advanceCommandInput.Text;
                runCommand();
            }
        }

        private void ExtractAllMem_Click(object sender, RoutedEventArgs e)
        {
            if (imageRam != null)
            {
                System.IO.Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + @"Saves\Mem Pages");
                var dlg = new CommonOpenFileDialog();
                dlg.Title = "Choose Download Folder";
                dlg.IsFolderPicker = true;
                dlg.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory + @"Saves\Mem Pages";
                dlg.AllowNonFileSystemItems = false;
                dlg.DefaultDirectory = AppDomain.CurrentDomain.BaseDirectory + @"Saves\Mem Pages";
                dlg.EnsureFileExists = true;
                dlg.EnsurePathExists = true;
                dlg.EnsureReadOnly = false;
                dlg.EnsureValidNames = true;
                dlg.Multiselect = false;
                dlg.ShowPlacesList = true;

                if (dlg.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    folderAux = dlg.FileName;
                    historyList.Add(DateTime.UtcNow.ToString("HH:mm") + " | Extracted all Memory pages to " + folderAux);
                    commandCB = "memdump";
                    runCommand();
                }
                else
                {
                    MessageBox.Show("No valid path select");
                }
            }
            else
            {
                MessageBox.Show("No RAM Image Selected");
            }

        }

        private void ExtractAllExes_Click(object sender, RoutedEventArgs e)
        {
            if (imageRam != null)
            {
                System.IO.Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + @"Saves\EXEs");
                var dlg = new CommonOpenFileDialog();
                dlg.Title = "Choose Download Folder";
                dlg.IsFolderPicker = true;
                dlg.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory + @"Saves\EXEs";
                dlg.AllowNonFileSystemItems = false;
                dlg.DefaultDirectory = AppDomain.CurrentDomain.BaseDirectory + @"Saves\EXEs";
                dlg.EnsureFileExists = true;
                dlg.EnsurePathExists = true;
                dlg.EnsureReadOnly = false;
                dlg.EnsureValidNames = true;
                dlg.Multiselect = false;
                dlg.ShowPlacesList = true;

                if (dlg.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    folderAux = dlg.FileName;
                    historyList.Add(DateTime.UtcNow.ToString("HH:mm") + " | Extracted all EXEs to " + folderAux);
                    commandCB = "procdump";
                    runCommand();
                }
                else
                {
                    MessageBox.Show("No valid path select");
                }
            }
            else
            {
                MessageBox.Show("No RAM Image Selected");
            }
        }

        private void ExtractAllVad_Click(object sender, RoutedEventArgs e)
        {
            if (imageRam != null)
            {
                System.IO.Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + @"Saves\VAD Pages");
                var dlg = new CommonOpenFileDialog();
                dlg.Title = "Choose Download Folder";
                dlg.IsFolderPicker = true;
                dlg.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory + @"Saves\VAD Pages";
                dlg.AllowNonFileSystemItems = false;
                dlg.DefaultDirectory = AppDomain.CurrentDomain.BaseDirectory + @"Saves\VAD Pages";
                dlg.EnsureFileExists = true;
                dlg.EnsurePathExists = true;
                dlg.EnsureReadOnly = false;
                dlg.EnsureValidNames = true;
                dlg.Multiselect = false;
                dlg.ShowPlacesList = true;

                if (dlg.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    folderAux = dlg.FileName;
                    historyList.Add(DateTime.UtcNow.ToString("HH:mm") + " | Extracted all VADs pages to " + folderAux);
                    commandCB = "vaddump";
                    runCommand();
                }
                else
                {
                    MessageBox.Show("No valid path select");
                }
            }
            else
            {
                MessageBox.Show("No RAM Image Selected");
            }
        }

        private void ExtractAllEvt_Click(object sender, RoutedEventArgs e)
        {
            if (imageRam != null)
            {
                System.IO.Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + @"Saves\Event Logs");
                var dlg = new CommonOpenFileDialog();
                dlg.Title = "Choose Download Folder";
                dlg.IsFolderPicker = true;
                dlg.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory + @"Saves\Event Logs";
                dlg.AllowNonFileSystemItems = false;
                dlg.DefaultDirectory = AppDomain.CurrentDomain.BaseDirectory + @"Saves\Event Logs";
                dlg.EnsureFileExists = true;
                dlg.EnsurePathExists = true;
                dlg.EnsureReadOnly = false;
                dlg.EnsureValidNames = true;
                dlg.Multiselect = false;
                dlg.ShowPlacesList = true;

                if (dlg.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    folderAux = dlg.FileName;
                    historyList.Add(DateTime.UtcNow.ToString("HH:mm") + " | Extracted all Event logs to " + folderAux);
                    commandCB = "evtlogs";
                    runCommand();
                }
                else
                {
                    MessageBox.Show("No valid path select");
                }
            }
            else
            {
                MessageBox.Show("No RAM Image Selected");
            }
        }

        private void ExtractAllDrivers_Click(object sender, RoutedEventArgs e)
        {
            if (imageRam != null)
            {
                System.IO.Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + @"Saves\Drivers");
                var dlg = new CommonOpenFileDialog();
                dlg.Title = "Choose Download Folder";
                dlg.IsFolderPicker = true;
                dlg.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory + @"Saves\Drivers";
                dlg.AllowNonFileSystemItems = false;
                dlg.DefaultDirectory = AppDomain.CurrentDomain.BaseDirectory + @"Saves\Drivers";
                dlg.EnsureFileExists = true;
                dlg.EnsurePathExists = true;
                dlg.EnsureReadOnly = false;
                dlg.EnsureValidNames = true;
                dlg.Multiselect = false;
                dlg.ShowPlacesList = true;

                if (dlg.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    folderAux = dlg.FileName;
                    historyList.Add(DateTime.UtcNow.ToString("HH:mm") + " | Extracted all Drivers to " + folderAux);
                    commandCB = "moddump";
                    runCommand();
                }
                else
                {
                    MessageBox.Show("No valid path select");
                }
            }
            else
            {
                MessageBox.Show("No RAM Image Selected");
            }
        }
        private void ExtractAllRegistry_Click(object sender, RoutedEventArgs e)
        {
            if (imageRam != null)
            {
                System.IO.Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + @"Saves\Registry");
                var dlg = new CommonOpenFileDialog();
                dlg.Title = "Choose Download Folder";
                dlg.IsFolderPicker = true;
                dlg.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory + @"Saves\Registry";
                dlg.AllowNonFileSystemItems = false;
                dlg.DefaultDirectory = AppDomain.CurrentDomain.BaseDirectory + @"Saves\Registry";
                dlg.EnsureFileExists = true;
                dlg.EnsurePathExists = true;
                dlg.EnsureReadOnly = false;
                dlg.EnsureValidNames = true;
                dlg.Multiselect = false;
                dlg.ShowPlacesList = true;

                if (dlg.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    folderAux = dlg.FileName;
                    historyList.Add(DateTime.UtcNow.ToString("HH:mm") + " | Extracted all Registry to " + folderAux);
                    commandCB = "dumpregistry";
                    runCommand();
                }
                else
                {
                    MessageBox.Show("No valid path select");
                }
            }
            else
            {
                MessageBox.Show("No RAM Image Selected");
            }
        }
        private void ExtractSomeMem_Click(object sender, RoutedEventArgs e)
        {
            if (imageRam != null)
            {
                MEMWindow objSecondWindow = new MEMWindow();
                objSecondWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("No RAM Image Selected");
            }
        }
        private void ExtractSomeExes_Click(object sender, RoutedEventArgs e)
        {
            if (imageRam != null)
            {
                //DLLWindow objSecondWindow = new DLLWindow();
                EXEWindow objSecondWindow = new EXEWindow();
                objSecondWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("No RAM Image Selected");
            }
        }
        private void ExtractSomeVad_Click(object sender, RoutedEventArgs e)
        {
            if (imageRam != null)
            {
                VADWindow objSecondWindow = new VADWindow();
                objSecondWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("No RAM Image Selected");
            }
        }

        //-------------------------------TabFucntions-------------------------------------------------------

        private TabItem AddTabItem(string nameAUX)
        {

            historyList.Add(DateTime.UtcNow.ToString("HH:mm") + " | Created Tab " + nameAUX);

            int count = _tabItems.Count;

            // create new tab item
            TabItem tab = new TabItem();

            tab.Header = nameAUX; //string.Format("Tab {0}", count);
            tab.Name = nameAUX; //string.Format("tab{0}", count);
            tab.HeaderTemplate = tabDynamic.FindResource("TabHeader") as DataTemplate;

            // add controls to tab item, this case I added just a textbox
            //TextBox txt = new TextBox();
            RichTextBox txtR = new RichTextBox();

            //txt.Name = "txt" + nameAUX;
            txtR.Name = "txtR" + nameAUX;

            //FlowDocument txtF = new FlowDocument();
            //Paragraph txtP = new Paragraph();
            //TextBlock txtB = new TextBlock();
            //txtF.Name = "txtF" + nameAUX;
            //txtP.Name = "txtP" + nameAUX;
            //txtB.Name = "txtB" + nameAUX;

            //aqui tuorial
            if (nameAUX == "Tutorial")
            {
                output = "Tutorial";
                output += Environment.NewLine;
                Paragraph paragraph = new Paragraph();
                paragraph.Inlines.Add(new Bold(new Run("Tutorial")));
                paragraph.Inlines.Add(Environment.NewLine);
                paragraph.Inlines.Add(Environment.NewLine);
                //Load
                paragraph.Inlines.Add(new Bold(new Run("Load RAM")));
                paragraph.Inlines.Add(Environment.NewLine);
                paragraph.Inlines.Add(new Run("The first step in the program is to load a Ram Image, for this go to the 'New RAM Image' in the top navbar."));
                paragraph.Inlines.Add(Environment.NewLine);
                paragraph.Inlines.Add(new Run("This will let you choose a file and see basic information about it before loading it in the software."));
                paragraph.Inlines.Add(Environment.NewLine);
                paragraph.Inlines.Add(Environment.NewLine);
                //Change
                paragraph.Inlines.Add(new Bold(new Run("Change RAM")));
                paragraph.Inlines.Add(Environment.NewLine);
                paragraph.Inlines.Add(new Run("If for any reason you need to change the loaded RAM Image go to the 'New RAM Image' in the top navbar."));
                paragraph.Inlines.Add(Environment.NewLine);
                paragraph.Inlines.Add(new Run("Warning! This will erase all the previous work so if necessary Save before doing this."));
                paragraph.Inlines.Add(Environment.NewLine);
                paragraph.Inlines.Add(Environment.NewLine);
                //Saves
                paragraph.Inlines.Add(new Bold(new Run("About Ram Image")));
                paragraph.Inlines.Add(Environment.NewLine);
                paragraph.Inlines.Add(new Run("In case you need to see the basic information of the RAM Image without loading it again go to the 'About RAM Image' in the top navbar."));
                paragraph.Inlines.Add(Environment.NewLine);
                paragraph.Inlines.Add(new Run("In this window is also possible to change the profile that the software works it. This changes the profile for every part of the software, this could cause problems. Unless you are sure about this function please don't use it."));
                paragraph.Inlines.Add(Environment.NewLine);
                paragraph.Inlines.Add(Environment.NewLine);
                //Saves
                paragraph.Inlines.Add(new Bold(new Run("Save/Save As")));
                paragraph.Inlines.Add(Environment.NewLine);
                paragraph.Inlines.Add(new Run("To save the current state of the project go to the 'Save' in the top navbar. If the project was loaded or saved before it will remember the file."));
                paragraph.Inlines.Add(Environment.NewLine);
                paragraph.Inlines.Add(new Run("To create a new save file indepedent from the previous file go to the 'Save As' in the top navbar."));
                paragraph.Inlines.Add(Environment.NewLine);
                paragraph.Inlines.Add(Environment.NewLine);
                //Cypher
                paragraph.Inlines.Add(new Bold(new Run("Cypher")));
                paragraph.Inlines.Add(Environment.NewLine);
                paragraph.Inlines.Add(new Run("When a project is saved for the first time or a new save file is created it will ask for a password."));
                paragraph.Inlines.Add(Environment.NewLine);
                paragraph.Inlines.Add(new Run("Please remember this password it will be necessary to load the file and there is no away to recover it."));
                paragraph.Inlines.Add(Environment.NewLine);
                paragraph.Inlines.Add(Environment.NewLine);
                //History
                paragraph.Inlines.Add(new Bold(new Run("History")));
                paragraph.Inlines.Add(Environment.NewLine);
                paragraph.Inlines.Add(new Run("Every action in the software is recorded and it's possible to see when a certain action was taken in the 'History' module in the top nav bar."));
                paragraph.Inlines.Add(Environment.NewLine);
                paragraph.Inlines.Add(new Run("This information is just for the current session and it's not saved in the save file."));
                paragraph.Inlines.Add(Environment.NewLine);
                paragraph.Inlines.Add(Environment.NewLine);
                //Commands Theme
                paragraph.Inlines.Add(new Bold(new Run("Commands Themes")));
                paragraph.Inlines.Add(Environment.NewLine);
                paragraph.Inlines.Add(new Run("Because of the number of commands possible with Volatility Framework the software separates them by theme. That can be chosen in the 'Commands Theme' ComboBox."));
                paragraph.Inlines.Add(Environment.NewLine);
                paragraph.Inlines.Add(new Run("Before choosing a specific command you must choose a theme. The themes follow the Command Reference from Volatility Foundation."));
                paragraph.Inlines.Add(Environment.NewLine);
                paragraph.Inlines.Add(Environment.NewLine);
                //Commands
                paragraph.Inlines.Add(new Bold(new Run("Commands")));
                paragraph.Inlines.Add(Environment.NewLine);
                paragraph.Inlines.Add(new Run("After choosing a theme, it's possible to run a command picking one in the 'Commands' ComboBox and pressing the 'Run' button."));
                paragraph.Inlines.Add(Environment.NewLine);
                paragraph.Inlines.Add(new Run("It's possible to cancel the command with the 'Cancel' button. If the command finishes running it will create a new Tab with the name of the run command."));
                paragraph.Inlines.Add(Environment.NewLine);
                paragraph.Inlines.Add(new Run("It's not possible to run the same command twice if the Tab of the command already exists. You can close any Tab by pressing the 'X' in the specific Tab. It's not possible to have 0 Tabs, so deleting the last Tab is not permitted"));
                paragraph.Inlines.Add(Environment.NewLine);
                paragraph.Inlines.Add(Environment.NewLine);
                //About Commands
                paragraph.Inlines.Add(new Bold(new Run("About Commands")));
                paragraph.Inlines.Add(Environment.NewLine);
                paragraph.Inlines.Add(new Run("Each time a command is chosen in the 'Commands' ComboBox a brief explanation of the commands will be given in the 'About Command' box."));
                paragraph.Inlines.Add(Environment.NewLine);
                paragraph.Inlines.Add(new Run("Some commands will have some information about its impact on the software like 'Slow', so please read the description about the command before running."));
                paragraph.Inlines.Add(Environment.NewLine);
                paragraph.Inlines.Add(Environment.NewLine);
                //About Commands
                //paragraph.Inlines.Add(new Bold(new Run("About Commands")));
                //paragraph.Inlines.Add(Environment.NewLine);
                //paragraph.Inlines.Add(new Run("Each time a commands is chosen in the 'Commands' ComboBox a brief explanation of the commands will be given in the 'About Command' box."));
                //paragraph.Inlines.Add(Environment.NewLine);
                //paragraph.Inlines.Add(new Run("Some commands will have some information about it's impact on the software like 'Slow', so please read the discription about the command before running."));
                //paragraph.Inlines.Add(Environment.NewLine);
                //paragraph.Inlines.Add(Environment.NewLine);
                //Advance Commands
                paragraph.Inlines.Add(new Bold(new Run("Advance Commands")));
                paragraph.Inlines.Add(Environment.NewLine);
                paragraph.Inlines.Add(new Run("In the 'Commands Theme' ComboBox there is an option called 'Advance Commands' choosing this option will enable some more advance commands and the possibility to run manual commands like in the CMD and the output will create a new Tab."));
                paragraph.Inlines.Add(Environment.NewLine);
                paragraph.Inlines.Add(new Run("These commands are slow by nature and normally depend on a second variable, depending on the command a 3rd ComboBox will appear and a value must be chosen. Once again the 'About Command' box is the best place to know this information."));
                paragraph.Inlines.Add(Environment.NewLine);
                paragraph.Inlines.Add(new Run("Multiple Tabs of this commands can co-exist depending the second variable of the specific command doesn't already exist."));
                paragraph.Inlines.Add(Environment.NewLine);
                paragraph.Inlines.Add(new Run("Manual Commands must follow the Volatility Framework guidelines so the command must be something like 'dlllist -p 1892'."));
                paragraph.Inlines.Add(Environment.NewLine);
                paragraph.Inlines.Add(Environment.NewLine);
                //Find
                paragraph.Inlines.Add(new Bold(new Run("Find")));
                paragraph.Inlines.Add(Environment.NewLine);
                paragraph.Inlines.Add(new Run("The Find function works only on the current selected Tab, to see the results on a different Tab the Find function must run again. The Tab will have all the matches highlighted in yellow and the box 'Find Results' will show how many results there have been found for the specific keyword or rule."));
                paragraph.Inlines.Add(Environment.NewLine);
                paragraph.Inlines.Add(new Run("This works with keywords or senteces by simply inserting your keyword/s in the 'Keyword/Regex' Box. Ex: '88', 'process' , 'system.exe 4'."));
                paragraph.Inlines.Add(Environment.NewLine);
                paragraph.Inlines.Add(new Run("The Find Function also works with regex, you must start the input in the 'Keyword/Regex' Box with $regex. or $Regex: , after that you can input your regex rule. Ex: '$regex: [0-8]', '$Regex:^0x'."));
                paragraph.Inlines.Add(Environment.NewLine);
                paragraph.Inlines.Add(Environment.NewLine);
                //YaraScan
                paragraph.Inlines.Add(new Bold(new Run("YaraScan")));
                paragraph.Inlines.Add(Environment.NewLine);
                paragraph.Inlines.Add(new Run("The command YaraScan because of its complexity exists in a separated window that can be found in the 'YaraScan' module in the top navbar."));
                paragraph.Inlines.Add(Environment.NewLine);
                paragraph.Inlines.Add(new Run("You can write all your rules in the 'Rules' Box and run the command on the entire system or a specific PID by choosing it in the present ComboBox. The result will be displayed in the output window. Closing this window will erase the rules and the output so if it's necessary save it to another file before closing."));
                paragraph.Inlines.Add(Environment.NewLine);
                paragraph.Inlines.Add(Environment.NewLine);
                //VirusTotal
                paragraph.Inlines.Add(new Bold(new Run("VirusTotal")));
                paragraph.Inlines.Add(Environment.NewLine);
                paragraph.Inlines.Add(new Run("To facilitate the search of malicious code inside the RAM Image without leaving the software you can use the VirusTotal API, to do this go to the 'VirusTotal API' module in the top navbar."));
                paragraph.Inlines.Add(Environment.NewLine);
                paragraph.Inlines.Add(new Run("Inside this window you will be able to search both Processes and all DLLs or just the DLLs relative to a specific process. First, you should fill the list of the desired variable and press the button 'Fill List'. After a while it will show the complete list of all the desired targets."));
                paragraph.Inlines.Add(Environment.NewLine);
                paragraph.Inlines.Add(new Run("When the list is filled you will see both the name of the file and a link pressing this link will take you to the VirusTotal website to the report of the specific file. If the report doesn't exist you can run the API on a specific file by pressing the 'Run' button of the pretended file."));
                paragraph.Inlines.Add(Environment.NewLine);
                paragraph.Inlines.Add(new Run("For the API to work you must insert your API key in the API key box, if you don't have a key you can request it on the website of VirusTotal. If you have a private API key check the 'Private Key' checkbox for the program to run without limitations."));
                paragraph.Inlines.Add(Environment.NewLine);
                paragraph.Inlines.Add(new Run("After running the API you can press the same link and it will show a complete VirusTotal report. Note that with a public key you will only be able to do 4 requests a minute and 500 requests a day, while the VirusTotal window is open with will control this. Also, note that the VirusTotal API is not able to work with files with more than 20MB."));
                paragraph.Inlines.Add(Environment.NewLine);
                paragraph.Inlines.Add(Environment.NewLine);
                //Extract ALL
                paragraph.Inlines.Add(new Bold(new Run("Extract All")));
                paragraph.Inlines.Add(Environment.NewLine);
                paragraph.Inlines.Add(new Run("To extract the various files that the Volatility Framework is able to extract go to the 'Extract All' module in the top navbar. There you will find all the options of the extraction."));
                paragraph.Inlines.Add(Environment.NewLine);
                paragraph.Inlines.Add(new Run("After pressing one of the options the software will ask for a place to save the files. Note that this process is not able to be canceled because the extraction is incremental and this would mean that some of the files would already be downloaded despite being canceled."));
                paragraph.Inlines.Add(Environment.NewLine);
                paragraph.Inlines.Add(Environment.NewLine);
                //Extract Selected
                paragraph.Inlines.Add(new Bold(new Run("Extract Selected")));
                paragraph.Inlines.Add(Environment.NewLine);
                paragraph.Inlines.Add(new Run("To extract the selected types of files that the Volatility Framework is able to extract go to the 'Extract Selected' module in the top navbar. There you will find all the options of the extraction."));
                paragraph.Inlines.Add(Environment.NewLine);
                paragraph.Inlines.Add(new Run("After pressing the options 'Memory Pages/Exes/VAD Nodes' this will open a new window where you can select the process to extract the selected file from. Note that this process is not able to be canceled because the extraction is incremental and this would mean that some of the files would already be downloaded despite being canceled."));
                paragraph.Inlines.Add(Environment.NewLine);
                paragraph.Inlines.Add(new Run("Pressing the option 'DLLs' will open a different window where you can select to extract all the dlls related to a process, by pressing the button 'Download all selected PID DLLs', or select a number of dlls to extract from the process chosen in the 'PID' ComboBox, by pressing the button 'Find PID DLL'. After selecting the pretended dlls, by pressing the checkbox in front of the dll, you can press the button 'Extract Selected DLL' where the program will only extract the selected dlls. Note that you can only select the dlls from the page you currently are in, changing the page will remove all previous clicked checkboxes."));
                paragraph.Inlines.Add(Environment.NewLine);
                paragraph.Inlines.Add(Environment.NewLine);
                //Easy Report
                paragraph.Inlines.Add(new Bold(new Run("Easy Report")));
                paragraph.Inlines.Add(Environment.NewLine);
                paragraph.Inlines.Add(new Run("To get a web report of the various commands of the Image you can go to the 'EasyReport' module by pressing the 'Easy Report'. This will open a new window with all the options to create a report, if you have any questions about the options check the tooltip of the pretended option."));
                paragraph.Inlines.Add(Environment.NewLine);
                paragraph.Inlines.Add(new Run("After selecting all the pretended options you can press the button 'Build Report' to start running. Depending on the number of options selected this can take a while, you can cancel at any time. After finishing this process it will ask for a save destination for the HTML file, if no directory is selected the program will save the file in a predestined directory inside the installation folder. After this the page will automatically be shown in the default browser."));
                paragraph.Inlines.Add(Environment.NewLine);
                paragraph.Inlines.Add(new Run("If you want the report in a PDF please use the native ability from the browser to print into a PDF by pressing the right-click button on the mouse and printing."));
                paragraph.Inlines.Add(Environment.NewLine);
                paragraph.Inlines.Add(new Run("The 'Conclusion' option is not a specific command but a combination of commands, this option can run all by itself where it will gather relevant information about the most suspicious processes inside the Image. Note that this option may not work depending on what Volatility founds. Careful with the output of the conclusion for more detailed information run other options."));
                paragraph.Inlines.Add(Environment.NewLine);
                paragraph.Inlines.Add(Environment.NewLine);
                //About
                paragraph.Inlines.Add(new Bold(new Run("About Program")));
                paragraph.Inlines.Add(Environment.NewLine);
                paragraph.Inlines.Add(new Run("To see general info about the software consult the 'About' in the top navbar."));
                paragraph.Inlines.Add(Environment.NewLine);
                //end
                txtR.Document = new FlowDocument(paragraph);
                paragraph = null;
            }
            else
            {
                txtR.AppendText(output);
            }

            txtR.IsReadOnly = true;

            //tab.Content = txt;
            tab.Content = txtR;

            //add
            tabDynamic.DataContext = null;

            // insert tab item right before the last (+) tab item
            _tabItems.Insert(count, tab);

            //add
            tabDynamic.DataContext = _tabItems;
            tabDynamic.SelectedItem = tab;

            //teste
            RegisterTextBox(txtR.Name, txtR);

            return tab;
        }


        void RegisterTextBox(string rTextBoxName, RichTextBox rTextBox)
        {
            if ((RichTextBox)this.FindName(rTextBoxName) != null)
                this.UnregisterName(rTextBoxName);
            this.RegisterName(rTextBoxName, rTextBox);
        }

        private void tabDynamic_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            TabItem tab = tabDynamic.SelectedItem as TabItem;
            if (tab == null) return;

            if (tab.Equals(_tabAdd))
            {
                // clear tab control binding
                tabDynamic.DataContext = null;

                TabItem newTab = this.AddTabItem("Tutorial");

                // bind tab control
                tabDynamic.DataContext = _tabItems;

                // select newly added tab item
                tabDynamic.SelectedItem = newTab;
            }
        }

        //----------------------Save&LoadFunctions--------------------------------------------------

        public void LoadFunction()
        {
            imageRamProfile = BetweenStringsWithInput("Profile:", "EndProfileRam", loadAux);
            hiveSAM = BetweenStringsWithInput("hiveSAM:", "EndhiveSAM", loadAux);
            hiveSYSTEM = BetweenStringsWithInput("hiveSYSTEM:", "EndhiveSYSTEM", loadAux);
            if (hiveSAM == "" || hiveSYSTEM == "")
            {
                hiveSAM = null;
                hiveSYSTEM = null;
            }
            this.Title = "VolatilityGUI - " + loadedFile;
            string textBoxesTemp = BetweenStringsWithInput("TextBox:", "EndTextBox", loadAux);
            List<string> textBoxesTempList = textBoxesTemp.Split(',').ToList<string>();
            foreach (var i in textBoxesTempList)
            {
                if (i != "Tutorial")
                {
                    //se mudar o header da tab inicial mudar esta condiçao
                    output = BetweenStringsWithInput(i + ":", i + "end", loadAux);
                    AddTabItem(i);
                }
            }
        }

        public static string Protect(string str)
        {
            byte[] entropy = Encoding.UTF8.GetBytes(Assembly.GetExecutingAssembly().FullName);
            byte[] data = Encoding.UTF8.GetBytes(str);
            string protectedData = Convert.ToBase64String(ProtectedData.Protect(data, entropy, DataProtectionScope.CurrentUser));
            return protectedData;
        }

        //--------------------------CMDFunctions----------------------------------------------
        void runCommand()
        {
            /*
            //cenas de mover
            if(advanceCommandLabel.Visibility == Visibility.Visible)
            {
                Grid.SetRowSpan(lbl_About, 5);
                Grid.SetRow(Keyword, 4);
                Grid.SetRow(txtFind, 4);
                Grid.SetRow(FindCommand, 4);
                Grid.SetRow(Result, 4);
                Grid.SetRow(lbl_Status, 4);
                Grid.SetRow(tabDynamic, 5);
                Grid.SetRowSpan(tabDynamic, 5);
                Grid.SetRow(CancelLabel, 3);
                Grid.SetRow(pbStatus, 3);
                Grid.SetRow(cancelBtn, 3);
                g10.Visibility = Visibility.Visible;
                g11.Visibility = Visibility.Visible;
                g12.Visibility = Visibility.Visible;
                g20.Visibility = Visibility.Visible;
                g21.Visibility = Visibility.Visible;
                g22.Visibility = Visibility.Visible;
                g30.Visibility = Visibility.Hidden;
                g31.Visibility = Visibility.Hidden;
                g32.Visibility = Visibility.Hidden;
            }
            else
            {
                Grid.SetRowSpan(lbl_About, 3);
                Grid.SetRow(Keyword, 2);
                Grid.SetRow(txtFind, 2);
                Grid.SetRow(FindCommand, 2);
                Grid.SetRow(Result, 2);
                Grid.SetRow(lbl_Status, 2);
                Grid.SetRow(tabDynamic, 3);
                Grid.SetRowSpan(tabDynamic, 8);
                Grid.SetRow(CancelLabel, 1);
                Grid.SetRow(pbStatus, 1);
                Grid.SetRow(cancelBtn, 1);
                g10.Visibility = Visibility.Hidden;
                g11.Visibility = Visibility.Hidden;
                g12.Visibility = Visibility.Hidden;
                g20.Visibility = Visibility.Hidden;
                g21.Visibility = Visibility.Hidden;
                g22.Visibility = Visibility.Hidden;
                g30.Visibility = Visibility.Hidden;
                g31.Visibility = Visibility.Hidden;
                g32.Visibility = Visibility.Hidden;
            }
            */

            RunCommand.IsEnabled = false;
            FindCommand.IsEnabled = false;
            EasyReport.IsEnabled = false;
            NewRamH.IsEnabled = false;
            AboutRAMTab.IsEnabled = false;
            SaveH.IsEnabled = false;
            SaveAsH.IsEnabled = false;
            HistoryH.IsEnabled = false;
            VirusH.IsEnabled = false;
            YaraScanH.IsEnabled = false;
            ForensicH.IsEnabled = false;
            BackgroundWorker bg = new BackgroundWorker();
            pbStatus.Visibility = Visibility.Visible;
            if (advanceToogle == null)
            {
                cancelBtn.Visibility = Visibility.Visible;
                CancelLabel.Visibility = Visibility.Visible;
            }

            if (commandCB == "memmap" && advanceToogle != null || commandCB == "vadinfo" && advanceToogle != null || commandCB == "vadwalk" && advanceToogle != null || commandCB == "impscan" && advanceToogle != null)
            {
                RunCommand.IsEnabled = false;
                advanceCommandButton.IsEnabled = false;
                cancelBtn.Visibility = Visibility.Visible;
                CancelLabel.Visibility = Visibility.Visible;
                bg.DoWork += new DoWorkEventHandler(MethodToGetInfoPID);
            }
            else if (commandCB == "mftparser" && advanceToogle != null || commandCB == "hivedump" && advanceToogle != null)
            {
                RunCommand.IsEnabled = false;
                advanceCommandButton.IsEnabled = false;
                cancelBtn.Visibility = Visibility.Visible;
                CancelLabel.Visibility = Visibility.Visible;
                bg.DoWork += new DoWorkEventHandler(MethodToGetInfoOffset);
            }
            else if (commandCB == "timeliner" && advanceToogle != null)
            {
                RunCommand.IsEnabled = false;
                advanceCommandButton.IsEnabled = false;
                cancelBtn.Visibility = Visibility.Visible;
                CancelLabel.Visibility = Visibility.Visible;
                bg.DoWork += new DoWorkEventHandler(MethodToGetInfoType);
            }
            else if (commandCB == "hashdump")
            {
                RunCommand.IsEnabled = false;
                advanceCommandButton.IsEnabled = false;
                cancelBtn.Visibility = Visibility.Visible;
                CancelLabel.Visibility = Visibility.Visible;
                bg.DoWork += new DoWorkEventHandler(MethodToGetInfoHashDump);
            }
            else if (folderAux == null)
            {
                RunCommand.IsEnabled = false;
                advanceCommandButton.IsEnabled = false;
                bg.DoWork += new DoWorkEventHandler(MethodToGetInfo);
            }
            else
            {
                cancelBtn.Visibility = Visibility.Hidden;
                CancelLabel.Visibility = Visibility.Hidden;
                bg.DoWork += new DoWorkEventHandler(MethodToGetInfoExtract);
            }
            bg.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bg_RunWorkerCompleted);
            //show marquee here
            bg.RunWorkerAsync();

        }

        void MethodToGetInfo(Object sender, DoWorkEventArgs args)
        {
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.FileName = "CMD.exe";

            startInfo.WorkingDirectory = Directory.GetCurrentDirectory();

            startInfo.Arguments = "/c volatility.exe -f " + "\"" + imageRam + "\"" + " --profile=" + imageRamProfile + " " + commandCB;
            startInfo.CreateNoWindow = true;
            process.StartInfo = startInfo;
            process.Start();
            output = process.StandardOutput.ReadToEnd();

            if (output == "")
                output = "No result.";

            process.WaitForExit();

        }

        void MethodToGetInfoExtract(Object sender, DoWorkEventArgs args)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.FileName = "CMD.exe";
            startInfo.WorkingDirectory = @Directory.GetCurrentDirectory();
            startInfo.Arguments = "/c volatility.exe -f " + "\"" + imageRam + "\"" + " --profile=" + imageRamProfile + " " + commandCB + " --dump-dir=" + "\"" + folderAux + "\"";
            startInfo.CreateNoWindow = true;
            process.StartInfo = startInfo;
            process.Start();
            string outputSL = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            MessageBox.Show("Extraction Completed");
        }
        void MethodToGetInfoPID(Object sender, DoWorkEventArgs args)
        {
            //System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.FileName = "CMD.exe";
            startInfo.WorkingDirectory = @Directory.GetCurrentDirectory();
            startInfo.Arguments = "/c volatility.exe -f " + "\"" + imageRam + "\"" + " --profile=" + imageRamProfile + " " + commandCB + " -p " + pid;
            startInfo.CreateNoWindow = true;
            process.StartInfo = startInfo;
            process.Start();
            output = process.StandardOutput.ReadToEnd();

            if (output == "")
                output = "No result.";

            process.WaitForExit();

        }
        void MethodToGetInfoOffset(Object sender, DoWorkEventArgs args)
        {
            //System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.FileName = "CMD.exe";
            startInfo.WorkingDirectory = @Directory.GetCurrentDirectory();
            startInfo.Arguments = "/c volatility.exe -f " + "\"" + imageRam + "\"" + " --profile=" + imageRamProfile + " " + commandCB + " -o " + offset;
            startInfo.CreateNoWindow = true;
            process.StartInfo = startInfo;
            process.Start();
            output = process.StandardOutput.ReadToEnd();

            //aqui
            //MessageBox.Show(startInfo.Arguments);

            if (output == "")
                output = "No result.";

            process.WaitForExit();

        }
        void MethodToGetInfoType(Object sender, DoWorkEventArgs args)
        {
            //System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.FileName = "CMD.exe";
            startInfo.WorkingDirectory = @Directory.GetCurrentDirectory();
            startInfo.Arguments = "/c volatility.exe -f " + "\"" + imageRam + "\"" + " --profile=" + imageRamProfile + " " + commandCB + " --type=" + type;
            startInfo.CreateNoWindow = true;
            process.StartInfo = startInfo;
            process.Start();
            output = process.StandardOutput.ReadToEnd();

            if (output == "")
                output = "No result.";

            process.WaitForExit();

        }

        void MethodToGetInfoHashDump(Object sender, DoWorkEventArgs args)
        {
            //System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.FileName = "CMD.exe";
            startInfo.WorkingDirectory = @Directory.GetCurrentDirectory();
            startInfo.Arguments = "/c volatility.exe -f " + "\"" + imageRam + "\"" + " --profile=" + imageRamProfile + " hashdump -y " + hiveSYSTEM + " -s " + hiveSAM;
            //MessageBox.Show(startInfo.Arguments);
            startInfo.CreateNoWindow = true;
            process.StartInfo = startInfo;
            process.Start();
            output = process.StandardOutput.ReadToEnd();



            process.WaitForExit();

        }

        void bg_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs args)
        {
            if (output == "")
                output = "No result.";
            //this method will be called once background worker has completed it's task
            //hide the marquee
            RunCommand.IsEnabled = true;
            FindCommand.IsEnabled = true;
            EasyReport.IsEnabled = true;
            NewRamH.IsEnabled = true;
            AboutRAMTab.IsEnabled = true;
            SaveH.IsEnabled = true;
            SaveAsH.IsEnabled = true;
            HistoryH.IsEnabled = true;
            VirusH.IsEnabled = true;
            YaraScanH.IsEnabled = true;
            ForensicH.IsEnabled = true;
            CancelLabel.Visibility = Visibility.Hidden;
            pbStatus.Visibility = Visibility.Hidden;
            cancelBtn.Visibility = Visibility.Hidden;
            RunCommand.IsEnabled = true;
            if (advanceToogle != null)
            {
                advanceCommandButton.IsEnabled = true;
            }
            if (ComboBoxPID.Items.Count == 0 && advanceToogle != null && commandCB == "dlllist")
            {
                ComboBoxDependent.IsEnabled = true;
                ComboBoxPID.IsEnabled = true;
                Regex exp = new Regex("pid:(.*)", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                MatchCollection matchCollection = exp.Matches(output);
                foreach (Match y in matchCollection)
                {
                    string aux = y.ToString();
                    string aux1 = Regex.Match(aux, @"\d+").Value;
                    ComboBoxPID.Items.Add(aux1);
                }
            }
            else if (ComboBoxOffset.Items.Count == 0 && advanceToogle != null && commandCB == "hivelist")
            {
                ComboBoxDependent.IsEnabled = true;
                ComboBoxOffset.IsEnabled = true;
                //falta mudar regra para ir buscar offsset
                Regex exp = new Regex("0xf(.*)", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                MatchCollection matchCollection = exp.Matches(output);
                foreach (Match y in matchCollection)
                {
                    //string aux = y.ToString();
                    //string aux1 = Regex.Match(aux, @"\d+").Value;
                    string yAux = y.ToString().Replace(" ", " | ");
                    ComboBoxOffset.Items.Add(yAux);
                }
            }
            if (folderAux == null)
            {
                if (advanceToogle == null)
                {
                    if (hiveSAM == null && hiveSYSTEM == null && commandCB == "hivelist" && hashToogle == 1)
                    {
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
                            if (hiveSYSTEM == null)
                            {
                                hiveSYSTEM = x.Value.Split(' ')[0];
                            }
                        }
                        //hashToogle = 0;


                    }
                    if (processCanceled == false)
                    {
                        //update the textbox
                        switch (commandCB)
                        {
                            case "pslist":
                                AddTabItem("PSList");
                                break;
                            case "pstree":
                                AddTabItem("PSTree");
                                break;
                            case "psscan":
                                AddTabItem("PSScan");
                                break;
                            case "dlllist":
                                AddTabItem("DLLList");
                                break;
                            case "handles":
                                AddTabItem("Handles");
                                break;
                            case "getsids":
                                AddTabItem("GetSids");
                                break;
                            case "cmdscan":
                                AddTabItem("CMDScan");
                                break;
                            case "consoles":
                                AddTabItem("Consoles");
                                break;
                            case "privs":
                                AddTabItem("Privs");
                                break;
                            case "envars":
                                AddTabItem("Envars");
                                break;
                            case "verinfo":
                                AddTabItem("VerInfo");
                                break;
                            case "memmap":
                                AddTabItem("MemMap");
                                break;
                            case "vadinfo":
                                AddTabItem("VadInfo");
                                break;
                            case "vadwalk":
                                AddTabItem("VadWalk");
                                break;
                            case "iehistory":
                                AddTabItem("IEHistory");
                                break;
                            case "modules":
                                AddTabItem("Modules");
                                break;
                            case "modscan":
                                AddTabItem("ModScan");
                                break;
                            case "ssdt":
                                AddTabItem("Ssdt");
                                break;
                            case "driverscan":
                                AddTabItem("DriverScan");
                                break;
                            case "filescan":
                                AddTabItem("FileScan");
                                break;
                            case "mutantscan":
                                AddTabItem("MutantScan");
                                break;
                            case "symlinkscan":
                                AddTabItem("SymLinkScan");
                                break;
                            case "thrdscan":
                                AddTabItem("ThrdScan");
                                break;
                            case "unloadedmodules":
                                AddTabItem("UnloadedModules");
                                break;
                            case "connections":
                                AddTabItem("Connections");
                                break;
                            case "connscan":
                                AddTabItem("ConnScan");
                                break;
                            case "sockets":
                                AddTabItem("Sockets");
                                break;
                            case "netscan":
                                AddTabItem("NetScan");
                                break;
                            case "hivelist":
                                if (hashToogle == 0)
                                {
                                    AddTabItem("HiveList");
                                }
                                else
                                {
                                    hashToogle = 0;
                                }
                                break;
                            case "printkey":
                                AddTabItem("PrintKey");
                                break;
                            case "hashdump":
                                AddTabItem("HashDump");
                                break;
                            case "lsadump":
                                AddTabItem("LSADump");
                                break;
                            case "userassist":
                                AddTabItem("UserAssist");
                                break;
                            case "shellbags":
                                AddTabItem("ShellBags");
                                break;
                            case "shimcache":
                                AddTabItem("ShimCache");
                                break;
                            case "getservicesids":
                                AddTabItem("GetServiceSIDs");
                                break;
                            case "crashinfo":
                                AddTabItem("CrashInfo");
                                break;
                            case "hibinfo":
                                AddTabItem("HibInfo");
                                break;
                            case "vboxinfo":
                                AddTabItem("VboxInfo");
                                break;
                            case "vmwareinfo":
                                AddTabItem("VMWareInfo");
                                break;
                            case "sessions":
                                AddTabItem("Sessions");
                                break;
                            case "wndscan":
                                AddTabItem("WNDScan");
                                break;
                            case "deskscan":
                                AddTabItem("DeskScan");
                                break;
                            case "atomscan":
                                AddTabItem("AtomScan");
                                break;
                            case "atoms":
                                AddTabItem("Atoms");
                                break;
                            case "clipboard":
                                AddTabItem("Clipboard");
                                break;
                            case "eventhooks":
                                AddTabItem("EventHooks");
                                break;
                            case "gahti":
                                AddTabItem("Gahti");
                                break;
                            case "messagehooks":
                                AddTabItem("MessageHooks");
                                break;
                            case "userhandles":
                                AddTabItem("UserHandles");
                                break;
                            case "gditimers":
                                AddTabItem("GdiTimers");
                                break;
                            case "windows":
                                AddTabItem("Windows");
                                break;
                            case "wintree":
                                AddTabItem("WinTree");
                                break;
                            case "malfind":
                                AddTabItem("MalFind");
                                break;
                            case "svcscan":
                                AddTabItem("SVCScan");
                                break;
                            case "ldrmodules":
                                AddTabItem("LDRModules");
                                break;
                            case "apihooks":
                                AddTabItem("APIHooks");
                                break;
                            case "IDT":
                                AddTabItem("idt");
                                break;
                            case "GDT":
                                AddTabItem("gdt");
                                break;
                            case "threads":
                                AddTabItem("Threads");
                                break;
                            case "callbacks":
                                AddTabItem("Callbacks");
                                break;
                            case "driverirp":
                                AddTabItem("DriverIRP");
                                break;
                            case "devicetree":
                                AddTabItem("DeviceTree");
                                break;
                            case "psxview":
                                AddTabItem("PSXView");
                                break;
                            case "timers":
                                AddTabItem("Timers");
                                break;
                            case "mftparser":
                                AddTabItem("mftparser");
                                break;
                            case "bioskdb":
                                AddTabItem("BIOSkdb");
                                break;
                            case "timeliner":
                                AddTabItem("TimeLiner");
                                break;
                            case "impscan":
                                AddTabItem("IMPScan");
                                break;
                        }
                    }
                    else
                    {
                        processCanceled = false;
                    }
                }
                else
                {
                    if (advanceCommandInput.Text != "")
                    {
                        string ADVCommandCB = commandCB.Split(' ')[0];
                        AddTabItem("Advance_" + ADVCommandCB);
                    }
                    else
                    {
                        switch (commandCB)
                        {
                            case "hivedump":
                                AddTabItem("HiveDump_" + offset);
                                break;
                            case "memmap":
                                AddTabItem("MemMap_" + pid);
                                break;
                            case "vadinfo":
                                AddTabItem("VadInfo_" + pid);
                                break;
                            case "vadwalk":
                                AddTabItem("VadWalk_" + pid);
                                break;
                            case "mftparser":
                                AddTabItem("mftparser_" + offset);
                                break;
                            case "timeliner":
                                AddTabItem("TimeLiner_" + type);
                                break;
                            case "impscan":
                                AddTabItem("IMPScan_" + pid);
                                break;
                        }
                    }
                }
            }
            else
            {
                folderAux = null;
            }
        }

        void getCommandRun(string valueCommand)
        {
            if (valueCommand.StartsWith("Advance_"))
                valueCommand = valueCommand.Split('_')[1];

            if (ComboBoxPID.IsEnabled == false && ComboBoxType.IsEnabled == false && TextBoxOffset.IsEnabled == false && ComboBoxOffset.IsEnabled == false)
            {
                if ((RichTextBox)this.FindName("txtR" + valueCommand) == null)
                {
                    commandCB = valueCommand.ToLower();
                    runCommand();
                }
                else
                {
                    MessageBox.Show("This Command Tab Is Already Present");
                }
            }
            else if (ComboBoxPID.IsEnabled == true)
            {
                if (ComboBoxPID.SelectedItem != null)
                {
                    pid = ComboBoxPID.SelectedItem.ToString();
                    //mudar a verificacao para fazer o especifico
                    if ((RichTextBox)this.FindName("txtR" + valueCommand + "_" + pid) == null)
                    {
                        commandCB = valueCommand.ToLower();
                        runCommand();
                    }
                    else
                    {
                        MessageBox.Show("This Command Tab Is Already Present");
                    }
                }
                else
                {
                    MessageBox.Show("No PID Selected");
                }
            }
            else if (ComboBoxType.IsEnabled == true)
            {
                if (ComboBoxType.SelectedItem != null)
                {
                    type = ComboBoxType.SelectedItem.ToString();
                    //mudar a verificacao para fazer o especifico
                    if ((RichTextBox)this.FindName("txtR" + valueCommand + "_" + type) == null)
                    {
                        commandCB = valueCommand.ToLower();
                        runCommand();
                    }
                    else
                    {
                        MessageBox.Show("This Command Tab Is Already Present");
                    }
                }
                else
                {
                    MessageBox.Show("No Type Selected");
                }
            }
            else if (TextBoxOffset.IsEnabled == true)
            {
                //verificar se o offset existe, pode ser pesado
                if (TextBoxOffset.Text != null)
                {
                    offset = TextBoxOffset.Text;
                    //mudar a verificacao para fazer o especifico
                    if ((RichTextBox)this.FindName("txtR" + valueCommand + "_" + offset) == null)
                    {
                        commandCB = valueCommand.ToLower();
                        runCommand();
                    }
                    else
                    {
                        MessageBox.Show("This Command Tab Is Already Present");
                    }
                }
                else
                {
                    MessageBox.Show("No Offset Input");
                }
            }
            else if (ComboBoxOffset.IsEnabled == true)
            {
                if (ComboBoxOffset.SelectedItem != null)
                {
                    //string tempOffset = ComboBoxOffset.SelectedItem.ToString();
                    offset = ComboBoxOffset.SelectedItem.ToString().Split(' ')[0];
                    //mudar a verificacao para fazer o especifico
                    if ((RichTextBox)this.FindName("txtR" + valueCommand + "_" + offset) == null)
                    {
                        commandCB = valueCommand.ToLower();
                        runCommand();
                    }
                    else
                    {
                        MessageBox.Show("This Command Tab Is Already Present");
                    }
                }
                else
                {
                    MessageBox.Show("No Offset Selected");
                }
            }
        }

        private string BetweenStringsWithInput(string before, string after, string stringAux)
        {
            int pFrom = stringAux.IndexOf(before) + before.Length;
            int pTo = stringAux.LastIndexOf(after);
            string outputAUX = stringAux.Substring(pFrom, pTo - pFrom);
            return outputAUX;
        }

        void disableAdvanceStuff()
        {
            advanceCommandButton.Visibility = Visibility.Hidden;
            advanceCommandInput.Visibility = Visibility.Hidden;
            advanceCommandLabel.Visibility = Visibility.Hidden;
            advanceCommandButton.IsEnabled = false;
            advanceCommandInput.IsEnabled = false;
            advanceCommandLabel.IsEnabled = false;
            lbl_PID.Visibility = Visibility.Hidden;
            lbl_Type.Visibility = Visibility.Hidden;
            lbl_Offset.Visibility = Visibility.Hidden;
            lbl_CBOffset.Visibility = Visibility.Hidden;
            ComboBoxPID.Visibility = Visibility.Hidden;
            ComboBoxType.Visibility = Visibility.Hidden;
            TextBoxOffset.Visibility = Visibility.Hidden;
            ComboBoxOffset.Visibility = Visibility.Hidden;
            ComboBoxPID.IsEnabled = false;
            ComboBoxType.IsEnabled = false;
            TextBoxOffset.IsEnabled = false;
            ComboBoxOffset.IsEnabled = false;
            sandImg.Visibility = Visibility.Hidden;
            lbl_About.BorderBrush = Brushes.Black;
            //RunCommand.Margin = new Thickness(593, 85, 1212, 956);

            /*
            //cenas de mover
            Grid.SetRowSpan(lbl_About, 2);
            Grid.SetRow(Keyword, 1);
            Grid.SetRow(txtFind, 1);
            Grid.SetRow(FindCommand, 1);
            Grid.SetRow(Result, 1);
            Grid.SetRow(lbl_Status, 1);
            Grid.SetRow(tabDynamic, 2);
            Grid.SetRowSpan(tabDynamic, 9);
            g10.Visibility = Visibility.Hidden;
            g11.Visibility = Visibility.Hidden;
            g12.Visibility = Visibility.Hidden;
            g20.Visibility = Visibility.Hidden;
            g21.Visibility = Visibility.Hidden;
            g22.Visibility = Visibility.Hidden;
            g30.Visibility = Visibility.Hidden;
            g31.Visibility = Visibility.Hidden;
            g32.Visibility = Visibility.Hidden;
            */
        }

        private void OnKeyDownCommand(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                RunCommand.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
            }
        }

        private void OnKeyDownHandlerManual(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                advanceCommandButton.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
            }
        }

        private void OnKeyDownFind(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                FindCommand.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
            }
        }

        private void OnKeyDownSave(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                if (keyBtn.Visibility == Visibility.Visible)
                {
                    keyBtn.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
                }
                else
                {
                    keyBtn2.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
                }

            }
        }

        private void OnKeyDownAdvance(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                RunCommand.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
            }
        }
    }

}


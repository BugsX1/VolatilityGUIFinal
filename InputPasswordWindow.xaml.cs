using Microsoft.Win32;
using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace VolatilityGUI
{
    /// <summary>
    /// Interaction logic for InputPasswordWindow.xaml
    /// </summary>
    public partial class InputPasswordWindow : Window
    {
        private string loadTempEncrytep;
        private string varFilename;
        private bool closeUser = false;
        public InputPasswordWindow()
        {
            InitializeComponent();
            keyPasswordbtn.IsEnabled = false;
        }

        private void keyPasswordbtn_Click(object sender, RoutedEventArgs e)
        {
            if (keyPasswordbox.Text.Trim().Length < 1)
            {
                MessageBox.Show("No key provided");
            }
            else
            {
                backlabel.Visibility = Visibility.Visible;
                backmsg.Visibility = Visibility.Visible;
                pbStatus.Visibility = Visibility.Visible;
                bool exception = false;
                try
                {
                    string loadTemp = StringCipher.Decrypt(loadTempEncrytep, keyPasswordbox.Text);
                }
                catch (Exception ex)
                {
                    backlabel.Visibility = Visibility.Hidden;
                    backmsg.Visibility = Visibility.Hidden;
                    pbStatus.Visibility = Visibility.Hidden;
                    exception = true;
                    keyPasswordbox.Text = "";
                    MessageBox.Show("Wrong Key");
                }
                if (!exception)
                {
                    string loadTemp = StringCipher.Decrypt(loadTempEncrytep, keyPasswordbox.Text);
                    if (loadTemp.Contains("FileVolatilityGUI"))
                    {
                        closeUser = true;
                        string ramAux = BetweenStringsWithInput("Ram:", "EndRam", loadTemp);
                        if (File.Exists(ramAux))
                        {
                            CentralWindow.keyCons = keyPasswordbox.Text;
                            CentralWindow.imageRamCons = ramAux;
                            CentralWindow objSecondWindow = new CentralWindow();
                            //objSecondWindow.Owner = this;
                            objSecondWindow.Show();
                            CentralWindow.loadedFileCons = varFilename;
                            CentralWindow.loadAuxCons = loadTemp;
                            objSecondWindow.LoadFunction();
                            App.Current.Windows[0].Close();
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("The RAM Image file has been moved from its original folder. Please put the file in its original location.");
                            this.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Something went wrong. Please close and reopen the software");
                    }
                }
            }
        }

        private void filebtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog selectItem = new OpenFileDialog
            {
                Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*"
            };
            if (selectItem.ShowDialog() == true)
            {
                CentralWindow.historyListCons.Add(DateTime.UtcNow.ToString("HH:mm") + " | File" + selectItem.SafeFileName + " Opened");
                filebox.Text = selectItem.FileName;
                keyPasswordbtn.IsEnabled = true;
                keyPasswordbox.BorderBrush = System.Windows.Media.Brushes.Red;
                varFilename = selectItem.FileName;
                loadTempEncrytep = File.ReadAllText(selectItem.FileName, Encoding.UTF8);
            }

        }

        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                keyPasswordbtn.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
            }
        }

        private void keyPasswordbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (keyPasswordbox.Text.Trim().Length < 1)
            {
                keyPasswordbox.BorderBrush = System.Windows.Media.Brushes.Red;
            }
            else
            {
                keyPasswordbox.BorderBrush = System.Windows.Media.Brushes.Black;
            }
        }

        private string BetweenStringsWithInput(string before, string after, string stringAux)
        {
            int pFrom = stringAux.IndexOf(before) + before.Length;
            int pTo = stringAux.LastIndexOf(after);
            string outputAUX = stringAux.Substring(pFrom, pTo - pFrom);
            return outputAUX;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!closeUser)
            {
                mainwindow objSecondWindow = new mainwindow();
                //objSecondWindow.Owner = this;
                objSecondWindow.Show();
                try
                {
                    App.Current.Windows[0].Close();
                }
                catch (Exception ex)
                {

                }
                App.Current.Windows[1].Close();
            }
        }
    }
}

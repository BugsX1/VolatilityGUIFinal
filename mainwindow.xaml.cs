using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace VolatilityGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class mainwindow : Window
    {

        public mainwindow()
        {
            InitializeComponent();
            System.IO.Directory.CreateDirectory("Saves");
            if (!File.Exists(@Directory.GetCurrentDirectory() + "\\volatility.exe"))
            {
                MessageBox.Show("No Volatility Detected. Please make sure there is a Volatility Standalone in " + Directory.GetCurrentDirectory());
                this.Close();
            }
        }


        private void New_Project_Button_Click(object sender, RoutedEventArgs e)
        {
            //Apagar objSecondWindow = new Apagar();
            CentralWindow objSecondWindow = new CentralWindow();
            objSecondWindow.Show();
            this.Close();
        }

        private void Load_Project_Button_Click(object sender, RoutedEventArgs e)
        {

            InputPasswordWindow objSecondWindow = new InputPasswordWindow();
            objSecondWindow.Show();
            Newbtn.IsEnabled = false;
            Loadbtn.IsEnabled = false;
            Easybtn.IsEnabled = false;

            //////////////////////--------------
            /*
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                string loadTempEncrytep = File.ReadAllText(openFileDialog.FileName, Encoding.UTF8);
                //string loadTemp = StringCipher.Decrypt(encryptedstring, password);
                string loadTemp = Unprotect(loadTempEncrytep);
                string ramAux = BetweenStringsWithInput("Ram:", "EndRam", loadTemp);
                if (loadTemp.Contains("FileVolatilityGUI") == true)
                {
                    CentralWindow.imageRamCons = ramAux;
                    CentralWindow objSecondWindow = new CentralWindow();
                    //objSecondWindow.Owner = this;
                    objSecondWindow.Show();
                    CentralWindow.loadedFileCons = openFileDialog.FileName;
                    CentralWindow.loadAuxCons = loadTemp;
                    objSecondWindow.LoadFunction();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("No Save File from VolatilityGUI Detected");
                }
            }
            */
        }

        private void Easy_Project_Button_Click(object sender, RoutedEventArgs e)
        {
            EasyProject objSecondWindow = new EasyProject();
            objSecondWindow.Show();
            this.Close();

        }

        private string BetweenStringsWithInput(string before, string after, string stringAux)
        {
            int pFrom = stringAux.IndexOf(before) + before.Length;
            int pTo = stringAux.LastIndexOf(after);
            string outputAUX = stringAux.Substring(pFrom, pTo - pFrom);
            return outputAUX;
        }

        public static string Unprotect(string str)
        {
            byte[] protectedData = Convert.FromBase64String(str);
            byte[] entropy = Encoding.UTF8.GetBytes(Assembly.GetExecutingAssembly().FullName);
            string data = Encoding.UTF8.GetString(ProtectedData.Unprotect(protectedData, entropy, DataProtectionScope.CurrentUser));
            return data;
        }
    }
}

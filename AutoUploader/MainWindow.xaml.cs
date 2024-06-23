using AutoUploader.Selenium;
using AutoUploader.Selenium.ControlElem;
using AutoUploader.UI;
using Microsoft.Win32;
using OpenQA.Selenium;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AutoUploader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _driver;
        private Boolean _headless = true;

        private InfoStruct _istruct = InfoStruct.Instance;

        private FirefoxStart _firefox = new FirefoxStart();

        private string[] _selectedFiles;

        private string[] _firstLettersArray;

        public MainWindow()
        {
            InitializeComponent();
            _istruct.total = 0;
            _istruct.success = 0;
            _istruct.error = 0;
            TerminalTextBox.AppendText(@"
     _ _            _                             
 ___(_) | ___ _ __ | |___      ____ ___   _____   
/ __| | |/ _ \ '_ \| __\ \ /\ / / _` \ \ / / _ \  
\__ \ | |  __/ | | | |_ \ V  V / (_| |\ V /  __/_ 
|___/_|_|\___|_| |_|\__| \_/\_/ \__,_| \_/ \___(_)
--------------
AutoUploader
");
            this.Title = "silentwave. AutoUploader";
            this.ResizeMode = ResizeMode.NoResize;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            UploadBtn.Click += NewButton_Click;

            RadioButton3.Checked += Headless_Checked;
            RadioButton4.Checked += Headless_Checked;
        }

        private Boolean CorrectPageTitle(IWebDriver wb, string pt) => wb.Title == pt;

        private Boolean StationName(IWebDriver wb, string pt,string path) => wb.FindElement(By.XPath(path)).Text == pt;
        private async void NewButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TerminalTextBox.AppendText("[INFO] Choose the file(s) \n");
                OpenFileDialog openFileDialog = new OpenFileDialog();

                openFileDialog.Filter = "All Files (*.*)|*.*|Audio Files (*.mp3;*.wav;*.ogg)|*.mp3;*.wav;*.ogg";
                openFileDialog.FilterIndex = 1;

                openFileDialog.Multiselect = true;



                if (openFileDialog.ShowDialog() == true)
                {
                    _selectedFiles = openFileDialog.FileNames;

                    //getting array of first few characters of tracks name,for searching purposes.
                    _firstLettersArray = openFileDialog.FileNames
                        .Select(fileName => System.IO.Path.GetFileNameWithoutExtension(fileName))
                        .Select(fileName => fileName.Length >= 4 ? fileName.Substring(0, 4) : fileName.Substring(0, fileName.Length >= 3 ? 3 : fileName.Length)) // Get the first 4 or 3 letters, or the whole name if shorter
                        .ToArray();

                    _istruct.total = (Int16)_firstLettersArray.Length;

                    foreach (string file in _selectedFiles)
                    {
                        TerminalTextBox.AppendText($"File : {file}\n");
                    }
                }
                //updating ui
                UIUpdates.UpdateContent(Files, _istruct.total, _istruct.success, _istruct.error);
                if (openFileDialog.FileName.Length > 0)
                {
                    await Task.Run(() =>
                    {
                        IWebDriver driver = _firefox.Init(TerminalTextBox, _headless);
                        Login.NavigateToPage(driver, TerminalTextBox,Files);
                        Login.PutData(driver, TerminalTextBox,Files);
                        Login.PressBtn(driver, TerminalTextBox, Files);
                        if (CorrectPageTitle(driver, "Client Area - AZ-Network"))
                        {
                            TerminalTextBox.Dispatcher.Invoke(() => TerminalTextBox.AppendText("[INFO] Page Title is correct proceeding...\n"));
                            EnteringCCast.PressBtn(driver, TerminalTextBox);
                            if (!StationName(driver, "IceCast Unlimited", "/html/body/section/div/div[1]/div[2]/div/div[1]/div[1]/div/div/div/div[1]/div[1]/div[1]/h3"))
                            {
                                MessageBox.Show("Wasnt been able to find correct audio station!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                _firefox.Dispose(driver, TerminalTextBox);
                                Environment.Exit(0);
                            }
                            TerminalTextBox.Dispatcher.Invoke(() => TerminalTextBox.AppendText("[INFO] IceCast Unlimited confirmed!\n"));
                            string originalHandle = driver.CurrentWindowHandle;
                            EnteringCCast.EnterCCast(driver, TerminalTextBox);
                            var newHandle = driver.WindowHandles.First(handle => handle != originalHandle);
                            driver.SwitchTo().Window(newHandle);
                            System.Threading.Thread.Sleep(3000);
                            if (!CorrectPageTitle(driver, "Centova Cast"))
                            {
                                MessageBox.Show("Was not been able to login into Centova Cast.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                _firefox.Dispose(driver, TerminalTextBox);
                                Environment.Exit(0);
                            }
                            //uploading files
                            CCastInteration.PressFiles(driver, TerminalTextBox);
                            CCastInteration.Upload(driver, TerminalTextBox, _selectedFiles,Files);
                            TerminalTextBox.Dispatcher.Invoke(() =>
                            {
                                TerminalTextBox.AppendText($"[INFO] Waiting 10 secs for all files to be uploaded!\n");
                            });
                            System.Threading.Thread.Sleep(10000);

                            driver.Navigate().GoToUrl("https://cast.az-streamingserver.com:2199/client/index.php");
                            TerminalTextBox.Dispatcher.Invoke(() =>
                            {
                                TerminalTextBox.AppendText($"[INFO] Closing file storage...!\n");
                            });
                            //return button 
                            TerminalTextBox.Dispatcher.Invoke(() =>
                            {
                                TerminalTextBox.AppendText($"[INFO] Trying to enter media folder...!\n");
                            });
                            PressHandler.Interaction(driver, TerminalTextBox, "/html/body/div[5]/div/div/div[5]/div/div[2]/a[2]", "Media folder reached!");

                            //sending a first few chars of track's name
                            foreach (string x in _firstLettersArray)
                            {
                                MediaInteraction.FindTrack(driver, TerminalTextBox, x,Files);
                                MediaInteraction.SelectArts(driver, TerminalTextBox,Files);
                                PressHandler.Interaction(driver, TerminalTextBox, "/html/body/div[3]/form/div[1]/div[3]/div/div[2]/input[2]", "Searching...");
                                MediaInteraction.DragAndDrop(driver, TerminalTextBox,Files);
                            }
                        }
                        System.Threading.Thread.Sleep(5000);
                        _firefox.Dispose(driver, TerminalTextBox);
                    });
                }
                else
                {
                    TerminalTextBox.AppendText("[INFO] No files was selected.\n");
                }
            }
            catch (Exception ex)
            {
                _istruct.error++;
                UIUpdates.UpdateContent(Files, _istruct.total, _istruct.success, _istruct.error);
                TerminalTextBox.AppendText($"[ERR] {ex.ToString}\n");
            }

        }

        private void Headless_Checked(object sender, RoutedEventArgs e)
        {
            if (RadioButton3.IsChecked == true)
            {
                _headless = true;
               
            }
            else if (RadioButton4.IsChecked == true)
            {
                _headless = false;
            }
            HeadlessLabel.Content = _headless ? "True" : "False";
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {

        }
    }

}
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
using Microsoft.Win32;
using System.Xml.Linq;
using AutoUploader.Selenium.Suggestions;
using AutoUploader.Selenium.Suggestions.Structure;
using OpenQA.Selenium.DevTools.V124.DOMSnapshot;
using System.IO;
using AutoUploader.Track;

namespace AutoUploader
{
    public class FirefoxDriverInitializer : IDriverInitializer
    {
        private FirefoxStart _firefoxStart = new FirefoxStart();

        public IWebDriver Init(TextBox terminalTextBox, bool headless)
        {
            return _firefoxStart.Init(terminalTextBox, headless);
        }

        public void Dispose(IWebDriver driver, TextBox tb)
        {
            _firefoxStart.Dispose(driver, tb);
        }
    }

    public class FileHandler : IFileHandler
    {
        public string[] SelectFiles()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "All Files (*.*)|*.*|Audio Files (*.mp3;*.wav;*.ogg)|*.mp3;*.wav;*.ogg",
                FilterIndex = 1,
                Multiselect = true
            };

            return openFileDialog.ShowDialog() == true ? openFileDialog.FileNames : null;
        }

        public List<string> ExtractFirstLetters(string[] fileNames)
        {
            var firstLetters = new List<string>();

            foreach (var fileName in fileNames)
            {
                string nameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(fileName);
                string[] words = nameWithoutExtension.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (words.Length > 0)
                {
                    firstLetters.Add(words[0]);
                }
            }

            return firstLetters;
        }
    }

    public class PageValidator : IPageValidator
    {
        public bool CorrectPageTitle(IWebDriver wb, string pageTitle)
        {
            return wb.Title == pageTitle;
        }

        public bool ValidateStationName(IWebDriver wb, string stationName, string xpath)
        {
            return wb.FindElement(By.XPath(xpath)).Text == stationName;
        }
    }
    public partial class MainWindow : Window
    {
        private readonly IDriverInitializer _driverInitializer;
        private readonly IFileHandler _fileHandler;
        private readonly IPageValidator _pageValidator;

        private bool _headless = true;
        private bool _autoBehaviour = false;

        private readonly InfoStruct _istruct = InfoStruct.Instance;
        private string[] _selectedFiles;
        private List<string> _firstLettersArray;

        public MainWindow()
        {
            InitializeComponent();

            _driverInitializer = new FirefoxDriverInitializer();
            _fileHandler = new FileHandler();
            _pageValidator = new PageValidator();

            InitializeUI();
        }

        private void InitializeUI()
        {
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
            EnableSuggestionsCheckBox.Checked += AutoSuggest;
            EnableSuggestionsCheckBox.Unchecked += AutoSuggest;
        }

        private async void NewButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TerminalTextBox.AppendText($"[INFO] Auto Behaviour - {_autoBehaviour}\n");
                TerminalTextBox.AppendText("[INFO] Choose the file(s) \n");



                _selectedFiles = _autoBehaviour ? await AutoBehaviour() : _fileHandler.SelectFiles();


                if (_selectedFiles != null && _selectedFiles.Length > 0)
                {
                    _firstLettersArray = _fileHandler.ExtractFirstLetters(_selectedFiles);
                    _istruct.total = (short)_firstLettersArray.Count;

                    foreach (var file in _selectedFiles)
                    {
                        TerminalTextBox.AppendText($"File : {file}\n");
                    }

                    UIUpdates.UpdateContent(Files, _istruct.total, _istruct.success, _istruct.error);

                    await Task.Run(() => ProcessFiles());
                }
                else
                {
                    TerminalTextBox.AppendText("[INFO] No files were selected.\n");
                }
            }
            catch (Exception ex)
            {
                _istruct.error++;
                UIUpdates.UpdateContent(Files, _istruct.total, _istruct.success, _istruct.error);
                TerminalTextBox.AppendText($"[ERR] {ex.ToString()}\n");
            }
        }

        private void ProcessFiles()
        {
            try
            {
                IWebDriver driver = _driverInitializer.Init(TerminalTextBox, _headless);
                
                Login.NavigateToPage(driver, TerminalTextBox, Files);
                System.Threading.Thread.Sleep(5000);
                Login.PutData(driver, TerminalTextBox, Files);
                Login.PressBtn(driver, TerminalTextBox, Files);
                
                if (_pageValidator.CorrectPageTitle(driver, "Client Area - AZ-Network"))
                {
                    TerminalTextBox.Dispatcher.Invoke(() => TerminalTextBox.AppendText("[INFO] Page Title is correct proceeding...\n"));
                    EnteringCCast.PressBtn(driver, TerminalTextBox);
                    if (!_pageValidator.ValidateStationName(driver, "IceCast Unlimited", "/html/body/section/div/div[1]/div[2]/div/div[1]/div[1]/div/div/div/div[1]/div[1]/div[1]/h3"))
                    {
                        MessageBox.Show("Wasnt been able to find correct audio station!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        _driverInitializer.Dispose(driver, TerminalTextBox);
                        Environment.Exit(0);
                    }
                    TerminalTextBox.Dispatcher.Invoke(() => TerminalTextBox.AppendText("[INFO] IceCast Unlimited confirmed!\n"));
                    string originalHandle = driver.CurrentWindowHandle;
                    EnteringCCast.EnterCCast(driver, TerminalTextBox);
                    var newHandle = driver.WindowHandles.First(handle => handle != originalHandle);
                    driver.SwitchTo().Window(newHandle);
                    System.Threading.Thread.Sleep(3000);
                    if (!_pageValidator.CorrectPageTitle(driver, "Centova Cast"))
                    {
                        MessageBox.Show("Was not been able to login into Centova Cast.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        _driverInitializer.Dispose(driver, TerminalTextBox);
                        Environment.Exit(0);
                    }
                    //uploading files
                    CCastInteration.PressFiles(driver, TerminalTextBox);
                    CCastInteration.Upload(driver, TerminalTextBox, _selectedFiles, Files);
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
                        System.Threading.Thread.Sleep(4000);
                        driver.Navigate().GoToUrl("https://cast.az-streamingserver.com:2199/client/index.php?page=library");
                        System.Threading.Thread.Sleep(4000);
                        MediaInteraction.FindTrack(driver, TerminalTextBox, x, Files);
                        System.Threading.Thread.Sleep(4000);
                        MediaInteraction.SelectArts(driver, TerminalTextBox, Files);
                        System.Threading.Thread.Sleep(4000);
                        PressHandler.Interaction(driver, TerminalTextBox, "/html/body/div[3]/form/div[1]/div[3]/div/div[2]/input[2]", "Searching...");
                        System.Threading.Thread.Sleep(4000);
                        MediaInteraction.DragAndDrop(driver, TerminalTextBox, Files);
                    }
                }

                _driverInitializer.Dispose(driver, TerminalTextBox);
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
            _headless = RadioButton3.IsChecked == true;
            HeadlessLabel.Content = _headless ? "True" : "False";
        }

        private void AutoSuggest(object sender, RoutedEventArgs e)
        {
            _autoBehaviour = (bool)EnableSuggestionsCheckBox.IsChecked;
            TerminalTextBox.AppendText($"[INFO] Auto Behaviour - {_autoBehaviour}\n");
        }


        private async Task<string[]> AutoBehaviour()
        {
            string downloadPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "DownloadedFiles");

            IWebDriver driver = _driverInitializer.Init(TerminalTextBox, _headless);
            TerminalTextBox.AppendText($"[INFO] Auto Behaviour - Trying to pull data...\n");
            string linkToSong = LinkGenerator.GenerateLink();

            if (!linkToSong.Equals("Error generating link"))
            {
                SongResponse suggestStruct = await Task.Run(() => GetSuggestions.ByPayloadAsync(linkToSong, TerminalTextBox));
                Random rand = new Random();
                int randIndex = rand.Next(suggestStruct.Documents.Count);

                Document suggestedSong = suggestStruct.Documents[randIndex];
                TerminalTextBox.AppendText($"[INFO] Auto Behaviour - Suggested Song Name : {suggestedSong.SongName}\n");
                TerminalTextBox.AppendText($"[INFO] Auto Behaviour - Suggested Song Youtube link : {suggestedSong.YoutubeLink}\n");

                Downloader.NavigateToDownloader(driver, TerminalTextBox);
                System.Threading.Thread.Sleep(1000);
                Downloader.InputLink(driver, TerminalTextBox, suggestedSong.YoutubeLink);
                System.Threading.Thread.Sleep(1000);
                PressHandler.Interaction(driver, TerminalTextBox, "/html/body/form/div[2]/input[2]", "Converting...");

                System.Threading.Thread.Sleep(10000);
                PressHandler.Interaction(driver, TerminalTextBox, "/html/body/form/div[2]/a[1]", "Downloading...");

                System.Threading.Thread.Sleep(20000);

                _driverInitializer.Dispose(driver, TerminalTextBox);

                string searchText = suggestedSong.SongName;

                var files = Directory.EnumerateFiles(downloadPath, "*", SearchOption.AllDirectories)
                                     .Where(file => System.IO.Path.GetFileName(file).Contains(searchText))
                                     .ToArray();

                if (files.Any())
                {
                    foreach (var file in files)
                    {
                        MetaDataManipulator.FixMetaData(file, suggestedSong.SongName, suggestedSong.Artist);
                    }
                    return files; 
                }
                else
                {
                    TerminalTextBox.AppendText("[INFO] No files found with the specified text in their names.\n");
                    return null; 
                }
            }

            return null; 
        }

        private async void SuggestBtn_Click(object sender, RoutedEventArgs e)
        {
            string[] _testOutput = await AutoBehaviour();
            foreach(var file in _testOutput)
            {
                TerminalTextBox.AppendText($"[INFO] {file}\n");
            }
        }
    }

}
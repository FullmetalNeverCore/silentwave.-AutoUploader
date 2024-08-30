using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Windows;
using Microsoft.Win32;

namespace AutoUploader.Selenium
{
    public class FirefoxStart : iInitBehaviour
    {

        private static string _ff; //@"C:\Program Files\Mozilla Firefox\firefox.exe";
        public FirefoxStart(string ff= @"C:\Program Files\Mozilla Firefox\firefox.exe")
        {
            _ff = ff;
        }
        public void Dispose(IWebDriver driver,TextBox tb)
        {
            //disposing of the driver 
            tb.Dispatcher.Invoke(() =>
            {
            tb.AppendText("[INFO] Disposing of driver...\n");

            });
            driver.Quit();
        }

        public IWebDriver Init(TextBox tb,Boolean hd=false)
        {
            try
            {
                // Get the base directory of the application
                tb.Dispatcher.Invoke(() =>
                {
                    tb.AppendText("[INFO] Creating web driver...\n");
                });
                string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

                string driverDirectory = Path.Combine(baseDirectory, "Drivers");

                if (!Directory.Exists(driverDirectory))
                {
                    throw new DirectoryNotFoundException($"The directory '{driverDirectory}' does not exist.");
                }

                // Verify the geckodriver.exe exists in the specified directory
                string geckoDriverPath = Path.Combine(driverDirectory, "geckodriver.exe");
                if (!File.Exists(geckoDriverPath))
                {
                    throw new FileNotFoundException($"The file 'geckodriver.exe' was not found in directory '{driverDirectory}'.");
                }


                //stuff for downloading files from net

                var prof = new FirefoxProfile();

                string downloadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "DownloadedFiles");
                tb.Dispatcher.Invoke(() =>
                {
                    tb.AppendText($"[INFO] download path {downloadPath}\n");
                });
                if (!Path.Exists(downloadPath))
                {
                    tb.Dispatcher.Invoke(() =>
                    {
                        tb.AppendText($"[INFO] Creating download directory...{downloadPath}\n");
                    });
                    Directory.CreateDirectory(downloadPath);
                }

                prof.SetPreference("browser.download.folderList", 2);
                prof.SetPreference("browser.download.dir", downloadPath);
                prof.SetPreference("browser.helperApps.neverAsk.saveToDisk", "audio/mpeg,audio/mp3");  // MIME types

                //prof.SetPreference("browser.download.manager.showWhenStarting", false);
                prof.SetPreference("pdfjs.disabled", true);

                // Create FirefoxOptions object if you need to set any Firefox-specific options
                FirefoxOptions opts = new FirefoxOptions
                {
                    Profile = prof
                };
                if (hd)
                {
                    tb.Dispatcher.Invoke(() =>
                    {
                        tb.AppendText("[INFO] Making driver headless....\n");
                    });
                    opts.AddArgument("--headless");
                    opts.AddArgument("--disable-gpu");
                }
                //executable location 
                opts.BrowserExecutableLocation = _ff;

                // Create a new instance of the Firefox driver, specifying the directory containing geckodriver.exe
                IWebDriver driver = new FirefoxDriver(driverDirectory, opts);

                tb.Dispatcher.Invoke(() =>
                {
                    tb.AppendText("[OK] Driver was created successfully!\n");
                });
                return driver;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(0);
                return default;
            }
        }
    }
}

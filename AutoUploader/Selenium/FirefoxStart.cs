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

namespace AutoUploader.Selenium
{
    class FirefoxStart : iInitBehaviour
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

                // Create FirefoxOptions object if you need to set any Firefox-specific options
                FirefoxOptions opts = new FirefoxOptions();
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

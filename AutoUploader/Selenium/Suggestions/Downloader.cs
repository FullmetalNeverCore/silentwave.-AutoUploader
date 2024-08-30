using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using static System.Net.WebRequestMethods;

namespace AutoUploader.Selenium.Suggestions
{
    class Downloader
    {
        private static readonly string Url = "https://ytmp3s.nu/6ufl/";

        public static void NavigateToDownloader(IWebDriver wb, TextBox tb)
        {
            tb.Dispatcher.Invoke(() =>
            {
                tb.AppendText($"[INFO] Attempting to download track...\n");
            });

            wb.Navigate().GoToUrl(Url);

            //append logs

        }

        public static void InputLink(IWebDriver wb, TextBox tb,string url) 
        {
            try
            {
                IWebElement getUrlInput = wb.FindElement(By.Id("url"));
                tb.Dispatcher.Invoke(() =>
                {
                    tb.AppendText($"[INFO] entering link...\n");
                });

                getUrlInput.SendKeys(url);
            }
            catch(Exception ex) 
            {
                tb.Dispatcher.Invoke(() =>
                {
                    tb.AppendText($"[ERROR] {ex.Message}\n");
                });
            }

        }
    }
}

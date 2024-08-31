using AutoUploader.Selenium;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

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
}

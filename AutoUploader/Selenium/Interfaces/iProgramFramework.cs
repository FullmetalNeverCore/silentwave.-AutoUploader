using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AutoUploader.Selenium
{
    public interface IDriverInitializer
    {
        IWebDriver Init(TextBox terminalTextBox, bool headless);
        void Dispose(IWebDriver driver, TextBox tb);
    }

    public interface IFileHandler
    {
        string[] SelectFiles();
        List<string> ExtractFirstLetters(string[] fileNames);
    }

    public interface IPageValidator
    {
        bool CorrectPageTitle(IWebDriver wb, string pageTitle);
        bool ValidateStationName(IWebDriver wb, string stationName, string xpath);
    }

}

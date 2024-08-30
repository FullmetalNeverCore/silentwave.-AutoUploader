using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoUploader.Selenium.ControlElem
{
    class StateControl
    {
        public static Boolean StationName(IWebDriver wb, string pt, string path) => wb.FindElement(By.XPath(path)).Text == pt;

        public static Boolean CorrectPageTitle(IWebDriver wb, string pt) => wb.Title == pt;
    }
}

using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoUploader.Selenium.ControlElem
{
    static class Scroll
    {
        public static void ScrollDown(IWebDriver driver,int pixels)
        {
            var js = (IJavaScriptExecutor)driver;
            js.ExecuteScript($"window.scrollBy(0, {pixels});");
        }
    }
}

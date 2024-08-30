using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using OpenQA.Selenium;

namespace AutoUploader.Selenium
{
    interface iInitBehaviour
    {
        public IWebDriver Init(TextBox tb, Boolean hd = false);

        public void Dispose(IWebDriver driver,TextBox tb);


    }
}

using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using OpenQA.Selenium.Support.UI;
using AutoUploader.Selenium.ControlElem;
namespace AutoUploader.Selenium
{
    static class EnteringCCast
    {
        public static void PressBtn(IWebDriver wb,TextBox tb)
        {
            PressHandler.Interaction(wb, tb, "/html/body/section/div/div[1]/div[2]/div[2]/div/div[1]/div/div[2]/div[1]/div/div[2]/span[1]", "About to load interface!");
        }

        public static void EnterCCast(IWebDriver wb,TextBox tb)
        {
            Scroll.ScrollDown(wb, 500);
            PressHandler.Interaction(wb, tb, "/html/body/section/div/div[1]/div[2]/div/div[1]/div[3]/div[1]/div/form/input[4]", "In the interface!");
        }
    }
}

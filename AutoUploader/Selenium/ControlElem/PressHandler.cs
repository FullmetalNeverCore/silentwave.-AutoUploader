using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AutoUploader.Selenium.ControlElem
{
    static class PressHandler
    {
        public static void Interaction(IWebDriver wb, TextBox tb, string xpath, string msg)
        {
            try
            {
                IWebElement btn = wb.FindElement(By.XPath(xpath));
                btn.Click();
                tb.Dispatcher.Invoke(() =>
                {
                    tb.AppendText($"[INFO] {msg}!\n");

                });
            }
            catch (ElementNotVisibleException ex)
            {
                tb.Dispatcher.Invoke(() =>
                {
                    tb.AppendText($"[ERR] {ex.ToString}!\n");
                    tb.AppendText($"[INFO] Trying again in 5 secs!\n");

                });
                System.Threading.Thread.Sleep(5000);
                Interaction(wb, tb,xpath,msg);
            }
            catch (NoSuchElementException nse)
            {
                tb.Dispatcher.Invoke(() =>
                {
                    tb.AppendText($"[ERR] No Such Element!\n");

                });
            }
        }
    }
}

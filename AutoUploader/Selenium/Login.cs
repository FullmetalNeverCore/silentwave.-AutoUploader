using AutoUploader.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.TextFormatting;

namespace AutoUploader.Selenium
{
    static class Login
    {
        private static InfoStruct _istruct = InfoStruct.Instance;
        private static string _url = "https://az-billing.com/billing/login";
        private static string _login;
        private static string _passw;

        public static Boolean NavigateToPage(IWebDriver wb,TextBox tb,Label ll)
        {
            try
            {
                _login = File.ReadAllLines("login.txt")[0];
                _passw = File.ReadAllLines("login.txt")[1];

                tb.Dispatcher.Invoke(() =>
                {
                    tb.AppendText("[INFO] Entering the login page...\n");

                });
                wb.Navigate().GoToUrl(_url);
                return true;
            }

            catch(Exception ex) 
            {
                tb.Dispatcher.Invoke(() =>
                {
                    tb.AppendText($"[ERR] {ex.ToString}\n");
                    
                });
                _istruct.error++;
                ll.Dispatcher.Invoke(() => UIUpdates.UpdateContent(ll, _istruct.total, _istruct.success, _istruct.error));
                return false;
            }
        }

        public static Boolean PutData(IWebDriver wb,TextBox tb, Label ll)
        {
            try
            {
                IWebElement getEmailInput = wb.FindElement(By.Id("inputEmail"));
                IWebElement getPasswInput = wb.FindElement(By.Id("inputPassword"));

                getEmailInput.SendKeys(_login);
                getPasswInput.SendKeys(_passw);
                tb.Dispatcher.Invoke(() =>
                {
                    tb.AppendText($"[INFO] Login successful!\n");

                });
                return true;
            }
            catch(Exception ex)
            {
                tb.Dispatcher.Invoke(() =>
                {
                    tb.AppendText($"[ERR] Unable to log in!\n");

                });
                _istruct.error++;
                ll.Dispatcher.Invoke(() => UIUpdates.UpdateContent(ll, _istruct.total, _istruct.success, _istruct.error));
                return false;
            }

        }

        //login
        public static void PressBtn(IWebDriver wb,TextBox tb,Label ll)
        {
            try
            {
                //pressing login button
                IWebElement btn = wb.FindElement(By.Id("login"));
                btn.Click();
                tb.Dispatcher.Invoke(() =>
                {
                    tb.AppendText($"[INFO] Login btn was clicked!\n");

                });
            }
            catch(ElementNotVisibleException ex)
            {
                tb.Dispatcher.Invoke(() =>
                {
                    tb.AppendText($"[ERR] {ex.ToString}!\n");
                    tb.AppendText($"[INFO] Trying again in 5 secs!\n");

                });

                _istruct.error++;
                ll.Dispatcher.Invoke(()=>UIUpdates.UpdateContent(ll, _istruct.total, _istruct.success, _istruct.error));
                System.Threading.Thread.Sleep(5000);
                PressBtn(wb, tb,ll );
            }


        }
    }
}

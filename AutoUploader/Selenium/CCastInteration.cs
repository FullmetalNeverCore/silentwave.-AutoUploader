using AutoUploader.Selenium.ControlElem;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System.Windows.Input;
using AutoUploader.UI;

namespace AutoUploader.Selenium
{
    static class CCastInteration
    {
        private static InfoStruct _istruct = InfoStruct.Instance;
        public static void PressFiles(IWebDriver wb, TextBox tb)
        {
            PressHandler.Interaction(wb, tb, "/html/body/div[5]/div/div/div[5]/div/div[2]/a[3]", "Trying to enter file storage...");
            tb.Dispatcher.Invoke(() =>
            {
                tb.AppendText($"[INFO] Reached File Storage!\n");

            });
        }
        public static void Upload(IWebDriver wb, TextBox tb, string[] files,Label ll)
        {
            try
            {

                // Triggering the upload button click using JavaScript
                System.Threading.Thread.Sleep(2500);
                IWebElement uploadButton = wb.FindElement(By.Id("btn_upload"));
                IJavaScriptExecutor js = (IJavaScriptExecutor)wb;
                js.ExecuteScript("arguments[0].click();", uploadButton);

                tb.Dispatcher.Invoke(() =>
                {
                    tb.AppendText($"[INFO] Upload dialogue reached!\n");
                    tb.AppendText($"[INFO] Trying to upload the files\n");
                });

                System.Threading.Thread.Sleep(2500);

                tb.Dispatcher.Invoke(() =>
                {
                    tb.AppendText($"[INFO] Trying to upload files...\n");
                });

                System.Threading.Thread.Sleep(2500);

                // Upload the files
                foreach(string x in files)
                {
                    tb.Dispatcher.Invoke(() =>
                    {
                        tb.AppendText($"[INFO] Trying to upload file {x}\n");
                    });

                    //there always is input type file
                    wb.FindElement(By.XPath("//input[@type='file']")).SendKeys(x);
                    System.Threading.Thread.Sleep(5000);
                }
                

                tb.Dispatcher.Invoke(() =>
                {
                    tb.AppendText($"[INFO] Files uploaded successfully!\n");
                });

            }
            catch (NoSuchElementException ex)
            {
                _istruct.error++;
                UIUpdates.UpdateContent(ll, _istruct.total, _istruct.success, _istruct.error);
                tb.Dispatcher.Invoke(() =>
                {
                    tb.AppendText($"[ERROR] Element not found: {ex.Message}\n");
                });
            }
            catch (Exception ex)
            {
                _istruct.error++;
                UIUpdates.UpdateContent(ll, _istruct.total, _istruct.success, _istruct.error);
                tb.Dispatcher.Invoke(() =>
                {
                    tb.AppendText($"[ERROR] An error occurred: {ex.Message}\n");
                });
            }
        }
    }
}

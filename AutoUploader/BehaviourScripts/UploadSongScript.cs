using AutoUploader.Selenium.ControlElem;
using AutoUploader.Selenium;
using AutoUploader.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AutoUploader.BehaviourScripts
{
    class UploadSongScript
    {


        public static bool ValidateClientArea(IWebDriver driver,TextBox TerminalTextBox, IPageValidator _pageValidator)
        {
            if (_pageValidator.CorrectPageTitle(driver, "Client Area - AZ-Network"))
            {
                TerminalTextBox.Dispatcher.Invoke(() => TerminalTextBox.AppendText("[INFO] Page Title is correct proceeding...\n"));
                return true;
            }
            else
            {
                MessageBox.Show("Incorrect page title!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public static void ProcessIceCast(IDriverInitializer driverinit, IWebDriver driver, TextBox TerminalTextBox, IPageValidator _pageValidator)
        {
            EnteringCCast.PressBtn(driver, TerminalTextBox);
            if (!_pageValidator.ValidateStationName(driver, "IceCast Unlimited", "/html/body/section/div/div[1]/div[2]/div/div[1]/div[1]/div/div/div/div[1]/div[1]/div[1]/h3"))
            {
                MessageBox.Show("Wasn't able to find correct audio station!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                driverinit.Dispose(driver, TerminalTextBox);
                Environment.Exit(0);
            }
            TerminalTextBox.Dispatcher.Invoke(() => TerminalTextBox.AppendText("[INFO] IceCast Unlimited confirmed!\n"));

            EnterCCastAndSwitchWindow(driverinit,driver,TerminalTextBox,_pageValidator);
        }

        public static void EnterCCastAndSwitchWindow(IDriverInitializer driverinit, IWebDriver driver, TextBox TerminalTextBox, IPageValidator _pageValidator)
        {
            string originalHandle = driver.CurrentWindowHandle;
            EnteringCCast.EnterCCast(driver, TerminalTextBox);
            var newHandle = driver.WindowHandles.First(handle => handle != originalHandle);
            driver.SwitchTo().Window(newHandle);
            System.Threading.Thread.Sleep(3000);

            if (!_pageValidator.CorrectPageTitle(driver, "Centova Cast"))
            {
                MessageBox.Show("Was not able to login into Centova Cast.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                driverinit.Dispose(driver, TerminalTextBox);
                Environment.Exit(0);
            }
        }

        public static void NavigateToMediaFolder(IWebDriver driver, TextBox TerminalTextBox)
        {
            TerminalTextBox.Dispatcher.Invoke(() =>
            {
                TerminalTextBox.AppendText("[INFO] Trying to enter media folder...!\n");
            });
            PressHandler.Interaction(driver, TerminalTextBox, "/html/body/div[5]/div/div/div[5]/div/div[2]/a[2]", "Media folder reached!");
        }

        public static void DisposeDriver(IDriverInitializer driverinit, IWebDriver driver, TextBox TerminalTextBox)
        {
            driverinit.Dispose(driver, TerminalTextBox);
        }
    }
}

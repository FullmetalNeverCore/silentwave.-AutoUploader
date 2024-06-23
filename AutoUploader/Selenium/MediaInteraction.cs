using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using OpenQA.Selenium.Interactions;
using AutoUploader.UI;


namespace AutoUploader.Selenium
{
    static class MediaInteraction
    {

        private static InfoStruct _istruct = InfoStruct.Instance;
        public static void FindTrack(IWebDriver wb,TextBox tb,string tname,Label ll)
        {
            try
            {
                tb.Dispatcher.Invoke(() =>
                {
                    tb.AppendText($"[INFO] Entering track's name... {tname}\n");
                });
                IWebElement searchBar = new WebDriverWait(wb, TimeSpan.FromSeconds(10)).Until(wbw => wbw.FindElement(By.Id("search_keyword")));
                searchBar.Clear();
                searchBar.SendKeys(tname);
            }
            catch(Exception ex)
            {
                _istruct.error++;
                UIUpdates.UpdateContent(ll, _istruct.total, _istruct.success, _istruct.error);
                tb.Dispatcher.Invoke(() =>
                {
                    tb.AppendText($"[ERR] {ex.ToString}\n");
                });
            }
        }

        public static void SelectArts(IWebDriver wb,TextBox tb,Label ll)
        {
            try
            {
                tb.Dispatcher.Invoke(() =>
                {
                    tb.AppendText($"[INFO] Selecting option 'all'...\n");
                });
                IWebElement selElem = wb.FindElement(By.Id("search_type"));
                SelectElement sel = new SelectElement(selElem);
                sel.SelectByValue("all");
            }
            catch(NoSuchElementException nse)
            {
                _istruct.error++;
                UIUpdates.UpdateContent(ll, _istruct.total, _istruct.success, _istruct.error);
                tb.Dispatcher.Invoke(() =>
                {
                    tb.AppendText($"[ERR] {nse.ToString}\n");
                });
            }
        }

        public static void DragAndDrop(IWebDriver wb,TextBox tb,Label ll)
        {
            ////*[@id="tracks_row_1"]
            ///playlists_row_4
            ///
            try
            {
                IWebElement track = wb.FindElement(By.Id("tracks_row_1"));
                IWebElement target = wb.FindElement(By.Id("playlists_row_4"));

                //drag and drop
                Actions act = new Actions(wb);

                act.DragAndDrop(track, target).Perform();

                _istruct.success++;
                UIUpdates.UpdateContent(ll, _istruct.total, _istruct.success, _istruct.error);
                tb.Dispatcher.Invoke(() =>
                {
                    tb.AppendText($"[OK] File was sent to playlist!\n");
                });
            }
            catch(Exception ex)
            {
                _istruct.error++;
                UIUpdates.UpdateContent(ll, _istruct.total, _istruct.success, _istruct.error);
                tb.Dispatcher.Invoke(() =>
                {
                    tb.AppendText($"[ERR] {ex.ToString}\n");
                });
            }


        }

    }
}

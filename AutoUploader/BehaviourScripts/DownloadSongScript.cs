using AutoUploader.Selenium;
using AutoUploader.Selenium.ControlElem;
using AutoUploader.Selenium.Suggestions;
using AutoUploader.Selenium.Suggestions.Structure;
using AutoUploader.Track;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AutoUploader.BehaviourScripts
{
    class DownloadSongScript
    {
        public static void DownloadSong(IWebDriver driver,TextBox tb,Document suggestedSong) 
        {
            tb.AppendText($"[INFO] Auto Behaviour - No duplicates were found!\n");
            Downloader.NavigateToDownloader(driver, tb);
            System.Threading.Thread.Sleep(1000);
            Downloader.InputLink(driver, tb, suggestedSong.YoutubeLink);
            System.Threading.Thread.Sleep(1000);
            PressHandler.Interaction(driver, tb, "/html/body/form/div[2]/input[2]", "Converting...");

            System.Threading.Thread.Sleep(10000);
            PressHandler.Interaction(driver, tb, "/html/body/form/div[2]/a[1]", "Downloading...");

            tb.AppendText($"[INFO] Auto Behaviour - Sleep for 40 secs, while download is in progress\n");
            System.Threading.Thread.Sleep(40000);
        }

        public static string GetDownloadPath()
        {
            return System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "DownloadedFiles");
        }

        public static async Task<SongResponse> GetSongSuggestionsAsync(IDriverInitializer driverInit, string linkToSong, TextBox tb, bool headless)
        {
            IWebDriver driver = null;
            try
            {
                tb.AppendText($"DEBUG: Starting to fetch song suggestions for link: {linkToSong}\n");

                SongResponse result = await Task.Run(() => GetSuggestions.ByPayloadAsync(linkToSong, tb));

                tb.AppendText("DEBUG: Song suggestions fetched successfully.\n");
                return result;
            }
            catch (Exception ex)
            {
                tb.AppendText($"ERROR: An error occurred in GetSongSuggestionsAsync: {ex.Message}\n");
                tb.AppendText($"ERROR: Stack Trace: {ex.StackTrace}\n");

                throw;
            }
        }


        public static Document SelectRandomSong(SongResponse suggestStruct,TextBox tb)
        {
            Random rand = new Random();
            int randIndex = rand.Next(suggestStruct.Documents.Count);
            Document suggestedSong = suggestStruct.Documents[randIndex];

            tb.AppendText($"[INFO] Auto Behaviour - Suggested Song Name : {suggestedSong.SongName}\n");
            tb.AppendText($"[INFO] Auto Behaviour - Suggested Song Youtube link : {suggestedSong.YoutubeLink}\n");

            return suggestedSong;
        }

        public static bool IsSongAlreadyDownloaded(string downloadPath, string songName)
        {
            var files = Directory.EnumerateFiles(downloadPath, "*", SearchOption.AllDirectories)
                                 .Where(file => System.IO.Path.GetFileName(file).Contains(songName))
                                 .ToArray();

            return files.Any();
        }

        public static async Task<string[]> DownloadAndProcessSong(IDriverInitializer driverinit,string downloadPath, Document suggestedSong,TextBox tb, bool headless)
        {
            IWebDriver driver = driverinit.Init(tb, headless);
            try
            {
                DownloadSongScript.DownloadSong(driver, tb, suggestedSong);

                var latestFile = Directory.EnumerateFiles(downloadPath, "*", SearchOption.AllDirectories)
                                          .Select(f => new FileInfo(f))
                                          .OrderByDescending(f => f.LastWriteTime)
                                          .FirstOrDefault();

                if (latestFile != null)
                {
                    return new string[] { MetaDataManipulator.FixMetaData(latestFile.FullName, suggestedSong.SongName, suggestedSong.Artist) };
                }
                else
                {
                    tb.AppendText("[INFO] No new files found.\n");
                    return null;
                }
            }
            finally
            {
                driverinit.Dispose(driver, tb);
            }
        }
    }
}

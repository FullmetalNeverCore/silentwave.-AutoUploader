using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoUploader.Selenium.Suggestions
{
    class LinkGenerator
    {
        public static Dictionary<string, string> songs = new Dictionary<string, string>
        {
            { "Heaven's Night","\"/music/Akira Yamaoka/_/Heaven's Night\"" },
            { "The Day of Night","\"/music/Akira Yamaoka/_/The Day of Night\"" },
            { "Stray Child","\"/music/Akira Yamaoka/_/A Stray Child\"" }
        };

        private static string GetRandomKey()
        {
            Random rand = new Random();
            int index = rand.Next(songs.Count); 
            return songs.Keys.ElementAt(index);
        }

        public static string GenerateLink()
        {
            try
            {
                string randomKey = GetRandomKey();
                string songValue = songs[randomKey];
                return songValue;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return "Error generating link";
            }
        }
    }

}

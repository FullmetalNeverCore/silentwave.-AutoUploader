using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoUploader.Track
{
    class MetaDataManipulator
    {
        public static string FixMetaData(string filePath, string newSongName, string artistName)
        {
            try
            {
                var tfile = TagLib.File.Create(filePath);

                tfile.Tag.Title = newSongName; 
                tfile.Tag.Performers = new[] { artistName };
                tfile.Save(); 

                string newFileName = Path.Combine(Path.GetDirectoryName(filePath), $"{newSongName}.mp3");
                File.Move(filePath, newFileName);

                Console.WriteLine($"[INFO] File renamed to: {newFileName}");
                return newFileName; 
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Failed to update metadata or rename file: {ex.Message}");
                return null;
            }
        }
    }
}

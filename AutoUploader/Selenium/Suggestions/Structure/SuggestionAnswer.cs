using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoUploader.Selenium.Suggestions.Structure
{
    public class Featured
    {
        public string FeaturedVideo { get; set; }
        public string SelectedArtist { get; set; }
        public string SelectedSong { get; set; }
    }

    public class Document
    {
        public string SongName { get; set; }
        public string Img { get; set; }
        public string Artist { get; set; }
        public string YoutubeLink { get; set; }
    }

    public class SongResponse
    {
        public Featured Featured { get; set; }
        public List<Document> Documents { get; set; }
    }
}

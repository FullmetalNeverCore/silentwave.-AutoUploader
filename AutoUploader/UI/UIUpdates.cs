using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AutoUploader.UI
{
    static class UIUpdates
    {
        //count label update
        //
        public static void UpdateContent(Label Files,Int16 total,Int16 success,Int16 fails) => Files.Content = $"Total : {total} Success : {success} Errors : {fails}";
    }
}

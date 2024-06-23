using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoUploader.UI
{
    public class InfoStruct
    {
        private static InfoStruct instance;
        private static readonly object lockObj = new object();

        public InfoStruct() {}

        public static InfoStruct Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObj)
                    {
                        if (instance == null)
                        {
                            instance = new InfoStruct();
                        }
                    }
                }
                return instance;
            }
        }
            public Int16 total { get; set; }
            public Int16 success { get; set; }
            public Int16 error { get; set; }
     
        }
}


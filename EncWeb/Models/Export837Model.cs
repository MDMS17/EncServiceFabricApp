using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EncWeb.Models
{
    public class Export837Model
    {
        public string sourcePath { get; set; }
        public string ProdTestFlag { get; set; }
        public List<string> newFiles { get; set; }
        public bool ExportCMSP { get; set; }
        public bool ExportCMSI { get; set; }
        public bool ExportCMSE { get; set; }
        public bool ExportDualP { get; set; }
        public bool ExportDualI { get; set; }
        public bool ExportDualE { get; set; }
        public bool ExportStateP { get; set; }
        public bool ExportStateI { get; set; }
        public bool ExportStateE { get; set; }
        public string totalDatabaseRecords { get; set; }
        public string goodDatabaseRecords { get; set; }
    }
}

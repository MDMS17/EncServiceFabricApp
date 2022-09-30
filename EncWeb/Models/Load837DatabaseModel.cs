using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EncWeb.Models
{
    public class Load837DatabaseModel
    {
        public string totalDatabaseRecords { get; set; }
        public string goodDatabaseRecords { get; set; }
        public string sourcePath { get; set; }
        public string archivePath { get; set; }
        public string totalFiles { get; set; }
        public string goodFiles { get; set; }
        public string ProdTestFlag { get; set; }
        public List<string> newFiles { get; set; }
        public string lastRunStartDate { get; set; }
        public string lastRunEndDate { get; set; }
        public string lastRunDate { get; set; }
        public string lastRunUser { get; set; }
        public string lastRunStatus { get; set; }
        public string currentRunStartDate { get; set; }
        public string currentRunEndDate { get; set; }
        public string message { get; set; }
        public bool enableLoadButton { get; set; }
        public bool LoadCMSP { get; set; }
        public bool LoadCMSI { get; set; }
        public bool LoadDualP { get; set; }
        public bool LoadDualI { get; set; }
        public bool LoadStateP { get; set; }
        public bool LoadStateI { get; set; }
        public bool DateRange { get; set; }
        public bool Include { get; set; }
        public bool Exclude { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EncWeb.Models
{
    public class ExportFilesModel
    {
        public string totalDatabaseRecords { get; set; }
        public string goodDatabaseRecords { get; set; }
        public string sourcePath { get; set; }
        public string archivePath { get; set; }
        public string totalFiles { get; set; }
        public string goodFiles { get; set; }
        public string ProdTestFlag { get; set; }
        public List<string> newFiles { get; set; }
        public string displayMessage { get; set; }
        public bool enableSubmit { get; set; }
    }
}

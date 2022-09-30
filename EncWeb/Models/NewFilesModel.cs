using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EncWeb.Models
{
    public class NewFilesModel
    {
        public string sourcePath { get; set; }
        public string archivePath { get; set; }
        public List<string> newFiles { get; set; }
        public int totalFiles { get; set; }
        public int goodFiles { get; set; }
    }
}

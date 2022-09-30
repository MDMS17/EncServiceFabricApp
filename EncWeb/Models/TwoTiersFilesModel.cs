using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EncWeb.Models
{
    public class TwoTiersFilesModel
    {
        public string sourcePath { get; set; }
        public string archivePath { get; set; }
        public List<Tuple<string, string, string, string, string>> twoTiersFiles { get; set; }
        public string totalFiles { get; set; }
        public string goodFiles { get; set; }
    }
}

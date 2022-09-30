using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EncWeb.Models
{
    public class LoadReconModel
    {
        public string totalFiles { get; set; }
        public string goodFiles { get; set; }
        public List<Tuple<string,string>> newFiles { get; set; }
        public string displayMessage { get; set; }
        public bool enableSubmit { get; set; }
    }
}

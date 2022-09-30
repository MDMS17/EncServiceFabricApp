using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EncWeb.Models
{
    public class MCPDIPJsonFileValidationModel
    {
        public string sourcePath { get; set; }
        public List<string> newFiles { get; set; }
        public List<Tuple<bool, string, string, string, string>> SelectedFiles { get; set; }
        public string AllFiles { get; set; }
        public string SelectedSequences { get; set; }
    }
}

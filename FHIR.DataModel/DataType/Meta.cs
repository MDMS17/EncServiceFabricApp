using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class Meta : Element
    {
        public string versionId { get; set; }
        public DateTime? lastUpdated { get; set; }
        public string source { get; set; }
        public List<string> profile { get; set; }
        public List<Coding> security { get; set; }
        public List<Coding> tag { get; set; }
    }
}

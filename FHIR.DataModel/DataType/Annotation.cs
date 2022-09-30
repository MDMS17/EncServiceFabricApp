using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class Annotation : Element
    {
        public Author author { get; set; }
        public DateTime time { get; set; }
        public string text { get; set; }
    }
}

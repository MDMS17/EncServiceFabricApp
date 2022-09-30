using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class ContactPoint : Element
    {
        public string system { get; set; }
        public string value { get; set; }
        public string use { get; set; }
        public uint? rank { get; set; }
        public Period period { get; set; }
    }
}

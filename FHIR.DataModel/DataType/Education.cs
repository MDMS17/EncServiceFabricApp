using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class Education : BackboneElement
    {
        public string documentType { get; set; }
        public string reference { get; set; }
        public DateTime? publicationDate { get; set; }
        public DateTime? presentationDate { get; set; }
    }
}

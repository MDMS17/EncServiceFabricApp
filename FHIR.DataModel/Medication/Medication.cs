using FHIR.DataModel.DataType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.Medication
{
    public class Medication : DomainResource
    {
        public List<Identifier> identifier { get; set; }
        public CodeableConcept code { get; set; }
        public string status { get; set; }
        public Reference manufacturer { get; set; }
        public CodeableConcept form { get; set; }
        public Ratio amount { get; set; }
        public List<Ingredient> ingredient { get; set; }
        public Batch batch { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class CareTeamClaim:BackboneElement
    {
        public uint sequence { get; set; }
        public Reference provider { get; set; }
        public bool? responsible { get; set; }
        public CodeableConcept role { get; set; }
        public CodeableConcept qualification { get; set; }
    }
}

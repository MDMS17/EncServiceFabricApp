using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class SubDetailExplanationOfBenefitAdd : BackboneElement 
    {
        public CodeableConcept productOrService { get; set; }
        public List<CodeableConcept> modifier { get; set; }
        public decimal? quantity { get; set; }
        public Money unitPrice { get; set; }
        public decimal? factor { get; set; }
        public Money net { get; set; }
        public List<uint> noteNumber { get; set; }
        public List<Adjudication> adjudication { get; set; }
    }
}

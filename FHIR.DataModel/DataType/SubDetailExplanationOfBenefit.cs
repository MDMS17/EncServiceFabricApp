using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class SubDetailExplanationOfBenefit : SubDetail
    {
        public List<uint> noteNumber { get; set; }
        public List<Adjudication> adjudication { get; set; }
    }
}

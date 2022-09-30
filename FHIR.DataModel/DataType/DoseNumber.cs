using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class DoseNumber
    {
        public uint? doseNumberPositiveInt { get; set; }
        public string doseNumberString { get; set; }
    }
}

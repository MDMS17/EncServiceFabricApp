using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class Location
    {
        public Address locationAddress { get; set; }
        public Reference locationReference { get; set; }
    }
}

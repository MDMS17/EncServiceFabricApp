using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class Ingredient : BackboneElement
    {
        public Item item { get; set; }
        public bool? isActive { get; set; }
        public Ratio strength { get; set; }
    }
}

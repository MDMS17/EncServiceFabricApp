using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class Attachment : Element
    {
        public string contentType { get; set; }
        public string language { get; set; }
        public byte[] data { get; set; }
        public string url { get; set; }
        public uint? size { get; set; }
        public byte[] hash { get; set; }
        public string title { get; set; }
        public DateTime? creation { get; set; }
    }
}

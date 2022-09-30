using System;
using System.Collections.Generic;
using System.Text;

namespace EncDataModel.Meditrac
{
    public class MeditracCode
    {
        public int ClaimId { get; set; }
        public int Sequence { get; set; }
        public string CodeType { get; set; }
        public string Code { get; set; }
        public string DateRecorded { get; set; }
        public string DateThrough { get; set; }
        public decimal? Amount { get; set; }
        public string DiagnosisCodeQualifier { get; set; }
        public string POAIndicator { get; set; }
    }
}

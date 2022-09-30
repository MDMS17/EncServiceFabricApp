using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EncDataModel.Vega
{
    public class NPIVega
    {
        [Key]
        public string NPI { get; set; }
        public string EntityType { get; set; }
        public string EIN { get; set; }
        public string ProvOrgName { get; set; }
        public string ProvLastName { get; set; }
        public string ProvFirstName { get; set; }
        public string ProvMidName { get; set; }
        public string Prov1stPracticeAddress { get; set; }
        public string Prov2ndPracticeAddress { get; set; }
        public string ProvPracticeCity { get; set; }
        public string ProvPracticeState { get; set; }
        public string ProvPracticeZip { get; set; }
        public string ProvPracticeCountry { get; set; }
        public string ProvPracticeTelephone { get; set; }
        public string ProvPracticeFax { get; set; }
        public string HPTaxCode1 { get; set; }
        public string ProvOtherOrgName { get; set; }
        public string ProvOtherOrgType { get; set; }
    }
}

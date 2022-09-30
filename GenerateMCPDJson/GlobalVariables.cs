using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenerateMCPDJson
{
    public class GlobalVariables
    {
        public static List<string> TradingPartners { get; set; } = new List<string> {"IEHP","Kaiser" };
        public static List<string> TestCin305 { get; set; } = new List<string> { "32001378A", "32001379A", "32001380A", "32001381A", "32001382A", "32001383A", "32001384A", "32001385A", "32001386A", "32001387A" };
        public static List<string> TestCin306 { get; set; } = new List<string> { "32001388A", "32001389A", "32001390A", "32001391A", "32001392A", "32001393A", "32001394A", "32001395A", "32001396A", "32001397A" };
        public static List<string> TestCocMer305 { get; set; } = new List<string> { "292092", "292093", "292094", "292095", "292096", "292097", "292098", "292099", "292100", "292101" };
        public static List<string> TestCocMer306 { get; set; } = new List<string> { "293093", "293094", "293095", "293096", "293097", "293098", "293099", "293100", "293101", "293102" };
    }
}

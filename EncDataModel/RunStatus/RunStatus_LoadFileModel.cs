using System;
using System.Collections.Generic;
using System.Text;

namespace EncDataModel.RunStatus
{
    public class RunStatus_LoadFileModel
    {
        public string CurrentRunDate { get; set; }
        public string CurrentRunUser { get; set; }
        public string CurrentRunStatus { get; set; }
        public List<string> FileNames { get; set; }
    }
}

using EncDataModel.MCPDIP;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonLib
{
    public static class JsonOperations
    {
        public static string GetResponseJson(ref ResponseFile responseFile) 
        {
            return JsonConvert.SerializeObject(responseFile, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Include });
        }
        public static string GetTestJson(ref ResponseFile responseFile)
        {
            return JsonConvert.SerializeObject(responseFile, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Include });
        }
        public static string GetMcpdJson(JsonMcpd jsonMcpd)
        {
            return JsonConvert.SerializeObject(
                jsonMcpd,
                Formatting.Indented,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
        }
        public static string GetPcpaJson(JsonPcpa jsonPcpa)
        {
            return JsonConvert.SerializeObject(
                jsonPcpa,
                Formatting.Indented,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
        }
        public static string GetPcpaHeaderJson(PcpHeader pcpHeader)
        {
            return JsonConvert.SerializeObject(
                pcpHeader,
                Formatting.Indented,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
        }

        public static string GetPcpaDetailJson(List<PcpAssignment> pcpAssignments)
        {
            return JsonConvert.SerializeObject(
                pcpAssignments,
                Formatting.Indented,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
        }
        public static string GetMcpdHeaderJson(McpdHeader mcpdHeader)
        {
            return JsonConvert.SerializeObject(
                mcpdHeader,
                Formatting.Indented,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
        }
        public static string GetGrievanceJson(List<McpdGrievance> grievances)
        {
            return JsonConvert.SerializeObject(
                grievances,
                Formatting.Indented,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
        }
        public static string GetAppealJson(List<McpdAppeal> appeals)
        {
            return JsonConvert.SerializeObject(
                appeals,
                Formatting.Indented,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
        }
        public static string GetCocJson(List<McpdContinuityOfCare> Cocs)
        {
            return JsonConvert.SerializeObject(
                Cocs,
                Formatting.Indented,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
        }
        public static string GetOonJson(List<McpdOutOfNetwork> Oons)
        {
            return JsonConvert.SerializeObject(
                Oons,
                Formatting.Indented,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
        }
        public static string GetResponseDetailJson(List<McpdipDetail> Details)
        {
            return JsonConvert.SerializeObject(
                Details,
                Formatting.Indented,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
        }
    }
}

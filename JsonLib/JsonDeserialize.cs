using EncDataModel.MCPDIP;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonLib
{
    public static class JsonDeserialize
    {
        public static ResponseFile DeserializeReponseFile(ref string responseFileString)
        {
            return JsonConvert.DeserializeObject<ResponseFile>(responseFileString);
        }
        public static TestJsonCoc DeserializeCoc(string jsonString)
        {
            return JsonConvert.DeserializeObject<TestJsonCoc>(jsonString);
        }
        public static List<TestJsonCoc> DeserializeListOfCoc(string jsonString)
        {
            return JsonConvert.DeserializeObject<List<TestJsonCoc>>(jsonString);
        }
        public static TestJsonOon DeserializeOon(string jsonString)
        {
            return JsonConvert.DeserializeObject<TestJsonOon>(jsonString);
        }
        public static List<TestJsonOon> DeserializeListOfOon(string jsonString)
        {
            return JsonConvert.DeserializeObject<List<TestJsonOon>>(jsonString);
        }
        public static JsonMcpd DeserializeJsonMcpd(ref string jsonString)
        {
            return JsonConvert.DeserializeObject<JsonMcpd>(jsonString);
        }
        public static JsonPcpa DeserializeJsonPcpa(ref string jsonString)
        {
            return JsonConvert.DeserializeObject<JsonPcpa>(jsonString);
        }
    }
}

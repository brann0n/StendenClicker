using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StendenClicker.Library
{
    public class RestHelper
    {
        private static readonly string BaseUrl = "https://stendenclicker.serverict.nl/";

        public static async Task<IRestResponse> GetRequestAsync(string url)
        {
            return await GetRequestAsync(url, null);
        }

        public static async Task<IRestResponse> GetRequestAsync(string url, Dictionary<string, string> parameters)
        {
            RestClient client = new RestClient(BaseUrl);
            var request = new RestRequest(url, Method.GET);
            foreach(var parameter in parameters)
            {
                request.AddParameter(parameter.Key, parameter.Value);
            }

            return await client.ExecuteAsync(request);
        }

        public static async Task<IRestResponse> PostRequestAsync(string url, object obj)
        {
            RestClient client = new RestClient(BaseUrl);
            var request = new RestRequest(url, Method.POST);

            request.AddParameter("application/json", ConvertObjectToJson(obj), ParameterType.RequestBody);
            request.RequestFormat = DataFormat.Json;

            return await client.ExecuteAsync(request);
        }

        public static string ConvertObjectToJson(object arg)
        {
            return JsonConvert.SerializeObject(arg);
        }

        public static T ConvertJsonToObject<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}

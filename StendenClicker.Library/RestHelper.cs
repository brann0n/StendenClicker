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

#if !DEBUG
		private const string BaseUrl = "http://localhost:50420/";
#else
        private const string BaseUrl = "https://stendenclicker.serverict.nl/";
#endif

        public static async Task<IRestResponse> GetRequestAsync(string url)
        {
            return await GetRequestAsync(url, new Dictionary<string, string>());
        }

        public static async Task<IRestResponse> GetRequestAsync(string url, Dictionary<string, string> parameters)
        {
            RestClient client = new RestClient(BaseUrl);
            var request = new RestRequest(url, Method.GET);
            request.AddHeader("API_KEY", "1D4AB4D5-2A21-4437-B11D-ED7874A4AB21");
            foreach (var parameter in parameters)
            {
                request.AddParameter(parameter.Key, parameter.Value);
            }

            return await client.ExecuteAsync(request);
        }

        public static async Task<IRestResponse> PostRequestAsync(string url, object obj)
        {
            RestClient client = new RestClient(BaseUrl);
            var request = new RestRequest(url, Method.POST);

            request.AddHeader("API_KEY", "1D4AB4D5-2A21-4437-B11D-ED7874A4AB21");
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
            if (string.IsNullOrEmpty(json)) return default;
            if (json[0] == '<')
            {             
                return default;
            }

            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}

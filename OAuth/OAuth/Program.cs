using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace OAuth
{
    class Program
    {
        private static HttpClient _httpClient = new HttpClient(){BaseAddress =new Uri("http://localhost:53003") };

        static  void autTask()
        {
            //var clientId = "1234";
            //var clientSecret = "5678";
            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            //        "Basic",
            //        Convert.ToBase64String(Encoding.ASCII.GetBytes(clientId + ":" + clientSecret)));

            //var parameters = new Dictionary<string, string>();
            //parameters.Add("grant_type", "client_credentials");

            //var a = _httpClient.PostAsync("/token", new FormUrlEncodedContent(parameters))
            //    .Result.Content.ReadAsStringAsync().Result;
            //Console.WriteLine(a);
            //Console.Read();
            var token =  GetAccessToken();
            _httpClient.DefaultRequestHeaders.Authorization=new AuthenticationHeaderValue("Bearer",token);
            var result = _httpClient.GetAsync("/api/values").Result.Content.ReadAsStringAsync();
            Console.WriteLine(result.Result);
        }

        static void Main(string[] args)
        {
             autTask();
        }
        public static string GetAccessToken()
        {
            var clientId = "1234";var clientSecret = "5678";
            _httpClient.DefaultRequestHeaders.Authorization=new AuthenticationHeaderValue("Basic",Convert.ToBase64String(Encoding.ASCII.GetBytes($"{clientId}:{clientSecret}")));
            var parameters=new Dictionary<string,string>();
            parameters.Add("grant_type", "client_credentials");
            var responseValue = _httpClient.PostAsync("/token", new FormUrlEncodedContent(parameters)).Result.Content.ReadAsStringAsync();
            return JObject.Parse(responseValue.Result)["access_token"].Value<string>();
        }
    }
}

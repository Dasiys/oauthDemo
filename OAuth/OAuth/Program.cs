﻿using System;
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
        private static HttpClient _httpClient = new HttpClient() { BaseAddress = new Uri("http://localhost:53003") };

        #region client_credentials验证OAuth
        //static void autTask()
        //{
        //    //var clientId = "1234";
        //    //var clientSecret = "5678";
        //    //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
        //    //        "Basic",
        //    //        Convert.ToBase64String(Encoding.ASCII.GetBytes(clientId + ":" + clientSecret)));

        //    //var parameters = new Dictionary<string, string>();
        //    //parameters.Add("grant_type", "client_credentials");

        //    //var a = _httpClient.PostAsync("/token", new FormUrlEncodedContent(parameters))
        //    //    .Result.Content.ReadAsStringAsync().Result;
        //    //Console.WriteLine(a);
        //    //Console.Read();
        //    var token = GetAccessToken();
        //    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        //    var result = _httpClient.GetAsync("/api/values").Result.Content.ReadAsStringAsync();
        //    Console.WriteLine(result.Result);
        //}

        //static void Main(string[] args)
        //{
        //    autTask();
        //}
        //public static string GetAccessToken()
        //{
        //    var clientId = "1234"; var clientSecret = "5678";
        //    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($"{clientId}:{clientSecret}")));
        //    var parameters = new Dictionary<string, string>();
        //    parameters.Add("grant_type", "client_credentials");
        //    var responseValue = _httpClient.PostAsync("/token", new FormUrlEncodedContent(parameters)).Result.Content.ReadAsStringAsync();
        //    return JObject.Parse(responseValue.Result)["access_token"].Value<string>();
        //}
        #endregion

        #region 账号和密码验证

        static void Main(string[] args)
        {
            var token = GetAccessToken();
            _httpClient.DefaultRequestHeaders.Authorization=new AuthenticationHeaderValue("Bearer",token);
            var result = _httpClient.GetAsync("/api/User").Result.Content.ReadAsStringAsync();
            Console.WriteLine(result.Result);
        }



        #endregion

        /// <summary>
        /// 获取Token
        /// </summary>
        /// <returns></returns>
        public static string GetAccessToken()
        {
            var clientId = "1234";
            var clientSecret = "5678";
            var parameters=new Dictionary<string,string>();
            parameters.Add("grant_type","password");
            parameters.Add("username","yeweimi");
            parameters.Add("password","123456");
            _httpClient.DefaultRequestHeaders.Authorization=new AuthenticationHeaderValue("Basic",Convert.ToBase64String(Encoding.ASCII.GetBytes($"{clientId}:{clientSecret}")));
            var response =
                _httpClient.PostAsync("/token", new FormUrlEncodedContent(parameters)).Result;
            var responseValue = response.Content.ReadAsStringAsync().Result;
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return JObject.Parse(responseValue)["access_token"].Value<string>();
            }
            else
            {
                Console.WriteLine(responseValue);
                return string.Empty;
            }
        }
    }
}

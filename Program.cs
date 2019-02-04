using System;
using System.IO;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using RestSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GetLimits
{
    class Program
    {
        public static String apiVersion;
        public static String baseUrl;
        public static String grantType;
        public static String clientId;
        public static String clientSecret;
        public static String userName;
        public static String password;
        public static String securityToken;
        public static String oauthUrl;
        public static String basePath;
        public static String accessToken;
        public static String tokenType;
        public static String instanceUrl;
        private static String orgAlias;

        public static IConfigurationRoot configuration { get; set; }

        static void Main(string[] args)
        {
            Setup();
            Authenticate();
            if (accessToken != null)
                GetLimits();
        }

        private static void GetLimits()
        {
            var client = new RestClient(instanceUrl);

            var request = new RestRequest("/services/data/" + apiVersion +"/limits/", Method.GET);
            request.AddHeader("Content-Type", "application/json; charset=utf-8");
            request.AddHeader("Authorization", tokenType + " " + accessToken);

            IRestResponse response = client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var content = response.Content;
                JObject limits = JObject.Parse(content);
                SaveFile(limits.ToString());
            }
            else 
            {
                Console.WriteLine(response.StatusCode + ": " + response.ErrorMessage);
            }
        }

        private static void Authenticate()
        {
            var client = new RestClient(baseUrl);

            var request = new RestRequest(oauthUrl, Method.POST);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded; charset=utf-8");
            request.AddParameter("client_id", clientId);
            request.AddParameter("client_secret", clientSecret);
            request.AddParameter("username", userName);
            request.AddParameter("password", password);
            request.AddParameter("security_token", securityToken);
            request.AddParameter("grant_type", grantType);

            IRestResponse response = client.Execute(request);
            var content = response.Content;
            JObject auth = JObject.Parse(content);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                accessToken = auth["access_token"].ToString();
                instanceUrl = auth["instance_url"].ToString();
                tokenType = auth["token_type"].ToString();
            }
            else
            {
                Console.WriteLine(response.StatusCode);
                Console.WriteLine(auth["error_description"]);
            }
        }

        private static void Setup()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            configuration = builder.Build();

            apiVersion = configuration["AppSettings:apiVersion"];
            baseUrl = configuration["AppSettings:baseUrl"];
            grantType = configuration["AppSettings:grantType"];
            clientId = configuration["AppSettings:cliendId"];
            clientSecret = configuration["AppSettings:clientSecret"];
            userName = configuration["AppSettings:userName"];
            password = configuration["AppSettings:password"];
            securityToken = configuration["AppSettings:securityToken"];
            oauthUrl = configuration["AppSettings:oauthUrl"];
            basePath = configuration["AppSettings:basePath"];
            orgAlias = configuration["AppSettings:orgAlias"];
        }

        public static void SaveFile(string contents)
        {
            String filePath = basePath + "\\" + DateTime.Now.ToString("yyyyMMddHHmm-") + orgAlias  + "-limits.json";
            try
            {
                System.IO.File.WriteAllText ( filePath, contents);
            }
            catch(IOException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}

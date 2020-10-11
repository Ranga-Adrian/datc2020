using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Newtonsoft.Json.Linq;

namespace L03
{
    class Program
    {
        private static DriveService _service;
         private static string _token;
        private static IEnumerable<string> scopes = null;

        static void Main(string[] args)
        {
            init();
        }
        static void init()
        {
            string[] scop=new string[]
            {
                DriveService.Scope.Drive,
                 DriveService.Scope.DriveFile
            };
            var clientID="302057956761-rm4n14jqi0i41cmgrhms875ntj7efajm.apps.googleusercontent.com";
            var clientSecret="1TeDEbzy3UY59iLYgwOBnQ-H";

             var credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                 new ClientSecrets
                 {
                     ClientId=clientID,
                     ClientSecret=clientSecret
                 },
                 scopes,
                 Environment.UserName,
                 CancellationToken.None,
                null
             ).Result;
            _service = new DriveService(new Google.Apis.Services.BaseClientService.Initializer()
            {
                HttpClientInitializer = credential
            
            });
            _token=credential.Token.AccessToken;
            Console.Write("Token:" + credential.Token.AccessToken);
            GetMyFiles();
                 
        }

        static void GetMyFiles()
        {
            var request = (HttpWebRequest)WebRequest.Create("http://www.googleapis.com/drive/v3/files?q='root'%20in%20parents");
            request.Headers.Add(HttpRequestHeader.Authorization, "Bearer" + _token);

            using (var response = request.GetResponse())
            {
            
                using (Stream data = request.GetRequestStream())
                using (var reader = new StreamReader(data))
                {
                    string text = reader.ReadToEnd();
                    var myData = JObject.Parse(text);
                    foreach (var file in myData["files"])
                    {
                        if (file["mineType"].ToString() != "application/vnd.google-apps.folder")
                        {
                            Console.WriteLine("File name: " + file["name"]);
                        }
                    }
                }
            }
        }

    }
}

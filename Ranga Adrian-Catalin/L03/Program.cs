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

        static void Main(string[] args)
        {
            Program.init();
        }
        static void init()
        {
            string[] scop=new string[]
            {
                DriveService.Scope.Drive,
                 DriveService.Scope.DriveFile
            };
            var clientID="302057956761-32cv4knupjnjj1iuvopcp76e2glqkkm9.apps.googleusercontent.com";
            var clientSecret="1hXajdrDOJoe2teHQ_z3G0PA";

             var credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                 new ClientSecrets
                 {
                     ClientId=clientID,
                     ClientSecret=clientSecret
                 },
                 scop,
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
        
                 
        }

      

    }
}

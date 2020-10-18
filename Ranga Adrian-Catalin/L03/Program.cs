using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Newtonsoft.Json.Linq;

namespace L03
{
    class Program
    {
        private static DriveService _service;
       // private static string _token;

        private static string[]Scopes = {DriveService.Scope.Drive};
        private static string ApplicationName = "Drive API RDATC ";

        private static UserCredential credential;

        static void Main(string[] args)
        {
            Program.init();
            Program.GetMyFiles();
            Program.Upload().GetAwaiter().GetResult();
        }
        static void init()
        {
        
            using (var stream = 
                new FileStream("client_datc2020.json", FileMode.Open, FileAccess.Read))
                {
                    string credPath = "token"; 
                    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.Load(stream).Secrets,
                        Scopes,
                        Environment.UserName,
                        CancellationToken.None,
                        new FileDataStore(credPath, true)).Result;
                    Console.WriteLine("Credential path" + credPath);

                }
            
            _service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            
            });
            Console.Write("Token:" + credential.Token.AccessToken);
        
        }

        static void GetMyFiles()
        {
            var request = (HttpWebRequest)WebRequest.Create("http://www.googleapis.com/drive/v3/files?q='root'%20in%20parents");
            request.Headers.Add(HttpRequestHeader.Authorization, "Bearer " + credential.Token.AccessToken);

            using (var response = request.GetResponse())
            {
            
                using (Stream data = request.GetRequestStream())
                using (var reader = new StreamReader(data))
                {
                    string text = reader.ReadToEnd();
                    var myData = JObject.Parse(text);
                    foreach (var file in myData["files"])
                    {
                        if (file["mimeType"].ToString() != "application/vnd.google-apps.folder")
                        {
                            Console.WriteLine("File name: " + file["name"]);
                        }
                    }
                }
            }
        }

         public static async Task<Google.Apis.Drive.v3.Data.File> Upload(string documentId="root")
        {
            var name = ($"{DateTime.UtcNow.ToString()}.txt");
            var mimeType = "text/plain";

            var fileMetadata = new Google.Apis.Drive.v3.Data.File()
            {
                Name = name,
                MimeType = mimeType,
                Parents = new[] { documentId }
            };

            FilesResource.CreateMediaUpload request;

            FileStream mystream = new FileStream("DATC_Tema_L03.txt", FileMode.Open, FileAccess.Read);
                request = _service.Files.Create(
                    fileMetadata, mystream, mimeType
                );
                request.Fields = "id, name, parents, createdTime, modifiedTime, mimeType, thumbnailLink";
                await request.UploadAsync();

                return request.ResponseBody;
        }
    }


}

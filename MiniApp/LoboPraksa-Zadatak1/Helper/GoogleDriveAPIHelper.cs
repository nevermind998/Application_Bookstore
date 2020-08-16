using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using LoboPraksa_Zadatak1.Model;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LoboPraksa_Zadatak1.Helper
{
    public class GoogleDriveAPIHelper : IGoogleDriveAPIHelper
    {
        private static DriveService driveService;

        public DriveService GetDriveService()
        {
            if (driveService == null)
            {
                return GetDriveServiceSingleton();
            }
            return driveService;
        }

        private DriveService GetDriveServiceSingleton()
        {
            string[] scope = { DriveService.Scope.Drive };
            UserCredential credentials;


            using (FileStream stream = new FileStream(Path.Combine("credentials.json"), FileMode.Open, FileAccess.Read))
            {
                ClientSecrets secrets = GoogleClientSecrets.Load(stream).Secrets;

                credentials = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    secrets, scope, "mojgrad2020@gmail.com", CancellationToken.None, null).Result;
            }

            driveService = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credentials,
                ApplicationName = "GoogleDriveASPNETCore"
            });

            return driveService;
        }

        public IList<FileModel> GetFiles()
        {
            FilesResource.ListRequest listRequest = GetDriveService().Files.List();
            listRequest.PageSize = 10;
            listRequest.Fields = "nextPageToken, files(id, name, mimeType)";

            IList<Google.Apis.Drive.v3.Data.File> files = listRequest.Execute().Files.ToList();


            List<FileModel> newFiles = new List<FileModel>();

            foreach (var item in files)
            {
                newFiles.Add(new FileModel()
                {
                    Id = item.Id,
                    Name = item.Name
                });
            }

            return newFiles;

        }

        public string GetFileName(string id)
        {
            //Google.Apis.Drive.v3.Data.File file = new Google.Apis.Drive.v3.Data.File();
            //file.Id = id;
            FilesResource.GetRequest getRequest = GetDriveService().Files.Get(id);

            return getRequest.Execute().Name;
        }

         
        public FileModel UploadFile(string path)
        {
            Google.Apis.Drive.v3.Data.File file = new Google.Apis.Drive.v3.Data.File();
            file.Name = Path.GetFileName(path);
            file.MimeType = MimeKit.MimeTypes.GetMimeType(file.Name); // moze i u argumentima koji faj hocemo 

            FilesResource.CreateMediaUpload createMediaUpload;

            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                createMediaUpload = GetDriveService().Files.Create(file, stream, file.MimeType);
                createMediaUpload.Fields = "id, name, mimeType, parents";
                createMediaUpload.Upload();
            }

            FileModel fileModel = new FileModel()
            {
                Id = createMediaUpload.ResponseBody.Id,
                Name = createMediaUpload.ResponseBody.Name,
                MimeType = createMediaUpload.ResponseBody.MimeType,
                DrivePath = createMediaUpload.ResponseBody.Parents != null ? GetFileName(createMediaUpload.ResponseBody.Parents.ElementAt(0)) : null
            };

            return fileModel;
        }

        public void DownloadFile(string id, string savePath)
        {
            FilesResource.GetRequest getRequest = GetDriveService().Files.Get(id);
            MemoryStream stream = new MemoryStream();
            getRequest.MediaDownloader.ProgressChanged += (Google.Apis.Download.IDownloadProgress downloadProgress) =>
            {
                switch (downloadProgress.Status)
                {
                    case Google.Apis.Download.DownloadStatus.Completed:
                        {
                            using (FileStream fileStream = new FileStream(savePath, FileMode.Create, FileAccess.Write))
                            {
                                stream.WriteTo(fileStream);
                                break;
                            }
                        }
                }
            };

            getRequest.Download(stream);
        }

        public string ShareFile(string id, string user)
        {
            Permission permission = new Permission()
            {
                EmailAddress = user,
                Role = "reader",
                Type = "user"
            };

            PermissionsResource.CreateRequest createRequest = GetDriveService().Permissions.Create(permission, id);
            return createRequest.Execute().Id;
        }
    }
}

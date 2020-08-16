using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LoboPraksa_Zadatak1.Helper;
using LoboPraksa_Zadatak1.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LoboPraksa_Zadatak1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GoogleDriveController : Controller
    {
        private readonly IGoogleDriveAPIHelper _googleDriveApiHelper;
        readonly ILogger<GoogleDriveController> _log;

        public GoogleDriveController(IGoogleDriveAPIHelper googleDriveApiHelper, ILogger<GoogleDriveController> log)
        {
            _googleDriveApiHelper = googleDriveApiHelper;
            _log = log;
        }
        [HttpGet]
        [Route("listFiles")]
        public IList<FileModel> ListFiles()
        {
            return _googleDriveApiHelper.GetFiles();
        }

        [HttpGet]
        [Route("getFileName")]
        public string GetFileName(string id)
        {
            return _googleDriveApiHelper.GetFileName(id);
           

        }

        [HttpGet]
        [Route("uploadFile")]
        public string UploadFile(string path)
        {
            FileModel file = _googleDriveApiHelper.UploadFile(path);
            if (file == null)
            {
                    _log.LogInformation("Nije lepo uploadovan fajl");
             }
            else
            {
                _log.LogInformation("Fajl je uspesno uplodovan");
            }
            return file.Id;
        }

        [HttpGet]
        [Route("downloadFile")]
        public string DownloadFile(string id, string savePath)
        {
            _googleDriveApiHelper.DownloadFile(id, savePath);
            return "Downloaded";

           // return _googleDriveBLL.GetDocumentData(id); //driveId
        }

        [HttpPost]
        [Route("shareFile")]
        public string ShareFile(string id, string user)
        {
            string id1 = _googleDriveApiHelper.ShareFile(id, user);
           // _googleDriveBLL.CreateShare(id1, id, user);
            return id1;
        }

    }
}

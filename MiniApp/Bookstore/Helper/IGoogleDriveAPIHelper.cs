using LoboPraksa_Zadatak1.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoboPraksa_Zadatak1.Helper
{
    public interface IGoogleDriveAPIHelper
    {
        public IList<FileModel> GetFiles();
        public string GetFileName(string id);
        public FileModel UploadFile(string path);
        public void DownloadFile(string id, string savePath);
        public string ShareFile(string id, string user);
       
    }
}

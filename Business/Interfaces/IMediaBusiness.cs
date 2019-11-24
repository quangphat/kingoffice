using Entity.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IMediaBusiness
    {
        Task<string> Upload(Stream stream, string key, string name, string webRootPath);
        Task<MediaUploadConfig> UploadSingle(Stream stream, string key,int fileId, string name, string webRootPath);
    }
}

using Business.Infrastructures;
using Business.Interfaces;
using Entity.Infrastructures;
using Entity.ViewModels;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Business.Classes
{
    public class MediaBusiness : BaseBusiness, IMediaBusiness
    {
        protected readonly ITailieuRepository _rpTailieu;
        public MediaBusiness(CurrentProcess process,ITailieuRepository tailieuRepository) : base(null, process)
        {
            _rpTailieu = tailieuRepository;
        }
        public async Task<string> Upload(Stream stream, string key, string name, string webRootPath)
        {
            stream.Position = 0;
            string fileUrl = string.Empty;
            var file = BusinessExtension.GetFileUploadUrl(name, webRootPath);
            using (var fileStream = System.IO.File.Create(file.FullPath))
            {
                await stream.CopyToAsync(fileStream);
                fileStream.Close();
                return file.FileUrl;
            }
        }
        public async Task<MediaUploadConfig> UploadSingle(Stream stream, string key,int fileId, string name, string webRootPath)
        {
            stream.Position = 0;
            string fileUrl = string.Empty;
            var file = BusinessExtension.GetFileUploadUrl(name, webRootPath);
            using (var fileStream = System.IO.File.Create(file.FullPath))
            {
                await stream.CopyToAsync(fileStream);
                fileStream.Close();
                fileUrl = file.FileUrl;
            }
            string deleteURL = fileId <=0 ? $"/media/delete?key={key}": $"/hoso/tailieu/delete/0/{fileId}";
            var update = await _rpTailieu.UpdateExistingFile(fileId, file.Name, file.FileUrl);
            if(update==true)
            {
                var _type = System.IO.Path.GetExtension(name);
                var config = new MediaUploadConfig
                {
                    initialPreview = fileUrl,
                    initialPreviewConfig = new PreviewConfig[] {
                                    new  PreviewConfig{
                                        caption = file.Name,
                                        url = deleteURL,
                                        key =key,
                                        type=_type.IndexOf("pdf") > 0 ?"pdf" : null,
                                        width ="120px"
                                        }
                                },
                    append = false,
                    Id = Guid.NewGuid()
                };
                return config;
            }
            return new MediaUploadConfig();
        }
    }
}

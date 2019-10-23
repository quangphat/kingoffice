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
    public class MediaBusiness :BaseBusiness, IMediaBusiness
    {
        
        public MediaBusiness(CurrentProcess process):base(null,process)
        {

        }
        public async Task<MediaUploadConfig> UploadSingle(Stream stream, string key, string name, string webRootPath)
        {
            stream.Position = 0;
            string fileUrl = string.Empty;
            var file = BusinessExtension.GetFileUploadUrl(name, webRootPath);
            using (var fileStream = System.IO.File.Create(file.FullPath))
            {
                await stream.CopyToAsync(fileStream);
                fileStream.Close();
                fileUrl =  file.FileUrl;
            }
            string deleteURL = $"/media/delete?key={key}";
            var _type = System.IO.Path.GetExtension(name);
            if (_type.IndexOf("pdf") > 0)
            {
                var config = new MediaUploadConfig
                {
                    initialPreview = fileUrl,
                    initialPreviewConfig = new PreviewConfig[] {
                                    new  PreviewConfig{
                                        caption = file.Name,
                                        url = deleteURL,
                                        key =key,
                                        type="pdf",
                                        width ="120px"
                                        }
                                },
                    append = false
                };
                return config;
            }
            else
            {
                var config = new MediaUploadConfig
                {
                    initialPreview = fileUrl,
                    initialPreviewConfig = new PreviewConfig[] {
                                    new PreviewConfig{
                                        caption = file.Name,
                                        url = deleteURL,
                                        key =key,
                                        width ="120px"
                                    }
                                },
                    append = false
                };
                return config;
            }
        }
    }
}
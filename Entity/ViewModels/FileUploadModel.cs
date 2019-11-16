using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entity.ViewModels
{
    public class FileUploadModel
    {
        public string Key { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }
    }

    public class HosoFilesModel
    {
        public int key { get; set; }
        public IFormFile file { get; set; }
    }
}

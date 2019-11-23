using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entity.ViewModels
{
    public class FileUploadModel
    {
        public int Key { get; set; }
        public string KeyName { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }
        public int FileId { get; set; }
        public string Id { get; set; }
        public bool? IsRequire { get; set; }
    }
    public class FileUploadModelGroupByKey
    {
        public int key { get; set; }
        public List<FileUploadModel> files { get; set; }
    }
    public class HosoFilesModel
    {
        public int key { get; set; }
        public IFormFile file { get; set; }
    }
}

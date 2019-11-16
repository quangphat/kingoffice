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
        public int Id { get; set; }
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

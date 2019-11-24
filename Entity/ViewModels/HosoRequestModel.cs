using Entity.DatabaseModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entity.ViewModels
{
    public class HosoRequestModel:HosoModel
    {
        public string Ghichu { get; set; }
        public DateTime BirthDay { get; set; }
        public DateTime CmndDay { get; set; }
        public int Doitac { get; set; }
        public  List<FileUploadModel> files { get; set; }
    }
    public class TestModel
    {
        public string Ghichu { get; set; }
    }
}

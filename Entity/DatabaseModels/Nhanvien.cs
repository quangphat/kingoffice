using System;
using System.Collections.Generic;
using System.Text;

namespace Entity.DatanbaseModels
{
    public class Nhanvien : BaseSqlEntity
    {
        public int ID { get; set; }
        public string Ten_Dang_Nhap { get; set; }
        public string Ma { get; set; }
        public string Mat_Khau { get; set; }
        public string Ho_Ten { get; set; }
        public string Dien_Thoai { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public int ProvinceId { get; set; }
        public int DistrictId { get; set; }
        public DateTime WorkDate { get; set; }
    }
}

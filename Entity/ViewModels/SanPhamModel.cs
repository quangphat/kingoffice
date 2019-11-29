using System;
using System.Collections.Generic;
using System.Text;

namespace Entity.ViewModels
{
    public class ProductViewModel
    {
        public int ID { get; set; }
        public string Ten { get; set; }
        public int TrangThai { get; set; }
    }
    public class ProductDetailViewModel
    {
        public int ID { get; set; }
        public string Ma { get; set; }
        public string Ten { get; set; }
        public DateTime NgayTao { get; set; }
        public string NguoiTao { get; set; }
    }
}

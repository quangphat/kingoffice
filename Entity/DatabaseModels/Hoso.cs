using System;
using System.Collections.Generic;
using System.Text;

namespace Entity.DatabaseModels
{
    public class HosoModel
    {
        public int ID { get; set; }
        public string MaHoSo { get; set; }
        public int CourierCode { get; set; }
        public string TenKhachHang { get; set; }
        public string CMND { get; set; }
        public string DiaChi { get; set; }
        public int MaKhuVuc { get; set; }
        public string SDT { get; set; }
        public string SDT2 { get; set; }
        public int GioTinh { get; set; }
        public DateTime NgayTao { get; set; }
        public DateTime NgayCapnhat { get; set; }
        public int MaNguoiTao { get; set; }
        public int MaNguoiCapnhat { get; set; }
        public int HoSoCuaAi { get; set; }
        public int? KetQuaHS { get; set; }
        public DateTime? NgayNhanDon { get; set; }
        public int MaTrangThai { get; set; }
        public int Sanphamvay { get; set; }
        public int CoBaoHiem { get; set; }
        public decimal SoTienVay { get; set; }
        public int HanVay { get; set; }
        public string TenCuaHang { get; }
       
    }

}

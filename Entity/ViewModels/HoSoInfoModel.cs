using System;
using System.Collections.Generic;
using System.Text;

namespace Entity.ViewModels
{
    public class HoSoInfoModel
    {
        public int ID { get; set; }
        

        public string MaHoSo { get; set; }

        public string MaKhachHang { get; set; }
        public string TenKhachHang { get; set; }

        public string CMND { get; set; }
        public string DiaChi { get; set; }
        public int MaKhuVuc { get; set; }

        public string SDT { get; set; }
        public int GioiTinh { get; set; }

        public DateTime NgayTao { get; set; }

        public int MaNguoiTao { get; set; }

        public DateTime NgayCapNhat { get; set; }

        public int MaNguoiCapNhat { get; set; }

        public DateTime NgayNhanDon { get; set; }

        public int MaTrangThai { get; set; }
        public int MaKetQua { get; set; }
        public int CoBaoHiem { get; set; }

        public int HanVay { get; set; }

        public string TenCuaHang { get; set; }

        public string GhiChu { get; set; }
        public decimal SoTienVay { get; set; }
        public string SDT2 { get; set; }

        public int HoSoCuaAi { get; set; }

        public int SanPhamVay { get; set; }

        public int CourierCode { get; set; }

        public string TenTrangThai { get; set; }
        public string KetQuaText { get; set; }
    }
}

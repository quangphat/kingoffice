using System;
using System.Collections.Generic;
using System.Text;

namespace Entity.Enums
{
    public enum TrangThaiHoSo
    {
        Nhap = 0,
        NhapLieu = 1,// Nhập liệu
        ThamDinh = 2,// Thẩm định
        TuChoi = 3,// Từ chối
        BoSungHoSo = 4,//
        GiaiNgan = 5,
        DaDoiChieu = 6,
        Cancel = 7,
        PCB = 8
    }
    public enum AutoID
    {
        HoSo = 1
    }
    public enum KetQuaHoSo
    {
        Trong = 1,
        GoiKhachHang = 2,
        ThamDinhDiaBan = 3,
        ChoKhoanVay = 4

    }
}

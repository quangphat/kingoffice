using System;
using System.Collections.Generic;
using System.Text;

namespace Entity.Enums
{
    public enum RoleType
    {
        Admin = 1,
        Leader = 5,
        Sale = 2
    }
    public enum ExportType
    {
        DanhsachHoso = 1,
        DuyetHoso = 2
    }
    public enum CICStatus
    {
        NotDebt = 0,// không nợ xấu
        Warning = 1, //nợ chú ý
        Debt = 2 //nợ xấu
    }
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
    public enum DateTimeFilterType
    {
        CreateDate = 1,
        UpdateDate = 2,
        WorkDate =3
    }
}

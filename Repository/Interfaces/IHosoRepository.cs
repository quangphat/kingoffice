﻿using Entity.ViewModels;
using Repository.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IHosoRepository
    {
        Task<List<Hoso>> Gets();
        Task<int> CountHosoDuyet(int maNVDangNhap,
            int maNhom,
            int maThanhVien,
            DateTime tuNgay,
            DateTime denNgay,
            string maHS,
            string cmnd,
            int loaiNgay,
            string trangThai,
            string freeText = null);
        Task<List<HosoDuyet>> GetHosoDuyet(int maNVDangNhap,
            int maNhom,
            int maThanhVien,
            DateTime tuNgay,
            DateTime denNgay,
            string maHS,
            string cmnd,
            int loaiNgay,
            string trangThai,
            string freeText,
            int offset,
            int limit,
            bool isDowload = false);
    }
}

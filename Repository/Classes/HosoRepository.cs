using Dapper;
using Entity.ViewModels;
using Microsoft.Extensions.Configuration;
using Repository.Entities;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Classes
{
    public class HosoRepository : BaseRepository, IHosoRepository
    {
        public HosoRepository(IConfiguration configuration) : base(configuration)
        {

        }
        public async Task<List<Hoso>> Gets()
        {
            var result = await connection.QueryAsync<Hoso>("select * from HO_SO");
            return result.ToList();
        }
        public async Task<int> CountHosoDuyet(int maNVDangNhap,
            int maNhom,
            int maThanhVien,
            DateTime tuNgay,
            DateTime denNgay,
            string maHS,
            string cmnd,
            int loaiNgay,
            string trangThai,
            string freeText = null)
        {
            string query = string.IsNullOrWhiteSpace(freeText) ? string.Empty : freeText;
            int totalRecord = 0;
            totalRecord = await connection.ExecuteScalarAsync<int>("sp_HO_SO_Count_TimHoSoDuyet",
                new
                {
                    @MaNVDangNhap = maNVDangNhap,
                    @MaNhom = maNhom,
                    @MaThanhVien = maThanhVien,
                    @TuNgay = tuNgay,
                    @DenNgay = denNgay,
                    @MaHS = maHS,
                    @CMND = cmnd,
                    @LoaiNgay = loaiNgay,
                    @TrangThai = trangThai,
                    @freeText = query
                },
                commandType: CommandType.StoredProcedure);
            return totalRecord;
        }
        public async Task<List<HosoDuyet>> GetHosoDuyet(int maNVDangNhap,
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
            bool isDowload = false)
        {
            var result = await connection.QueryAsync<HosoDuyet>("sp_HO_SO_TimHoSoDuyet",
                new
                {
                    @MaNVDangNhap = maNVDangNhap,
                    @MaNhom = maNhom,
                    @MaThanhVien = maThanhVien,
                    @TuNgay = tuNgay,
                    @DenNgay = denNgay,
                    @MaHS = maHS,
                    @CMND = cmnd,
                    @LoaiNgay = loaiNgay,
                    @TrangThai = trangThai,
                    @offset = offset,
                    @limit = limit,
                    @freeText = freeText
                },
                commandType: CommandType.StoredProcedure);
            return result.ToList();
        }
    }
}

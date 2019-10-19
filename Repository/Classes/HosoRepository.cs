using Dapper;
using Entity.Enums;
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
            string freeText = "")
        {

            return  await connection.ExecuteScalarAsync<int>("sp_HO_SO_Count_TimHoSoDuyet",
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
                    @freeText = freeText
                },
                commandType: CommandType.StoredProcedure);
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
        public async Task<AutoIDModel> GetAutoId()
        {
            var result = await connection.QueryFirstOrDefaultAsync("sp_AUTOID_GetID", new { ID = (int)AutoID.HoSo }, commandType: CommandType.StoredProcedure);
        }
        public async Task<List<HosoDuyet>> GetHosoNotApprove(int userId,
            int maNhom,
            int maThanhvien,
            DateTime fromDate,
            DateTime toDate,
            string maHs,
            string cmnd,
            int dateType,
            string status
            )
        {
            var result = await connection.QueryAsync<HosoDuyet>("sp_HO_SO_TimHoSoDuyetChuaXem",
                new
                {
                    @MaNVDangNhap = userId,
                    @MaNhom = maNhom,
                    @MaThanhVien = maThanhvien,
                    @TuNgay = fromDate,
                    @DenNgay = toDate,
                    @MaHS = maHs,
                    @CMND = cmnd,
                    @LoaiNgay = dateType,
                    @TrangThai = status
                },
                commandType: CommandType.StoredProcedure);
            return result.ToList();
        }
    }
}

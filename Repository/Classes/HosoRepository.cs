using Dapper;
using Entity.DatabaseModels;
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
        
        public async Task<int> Create(HosoModel model)
        {
            model.NgayTao = DateTime.Now;
            var p = generateHosoParameter(model);
            await connection.ExecuteAsync("sp_HO_SO_Them", p,
                commandType: CommandType.StoredProcedure);
            var result = p.Get<int>("ID");
            return result;
        }
        public async Task<bool> Update(HosoModel model)
        {
            model.NgayTao = DateTime.Now;
            var p = generateHosoParameter(model);
            await connection.ExecuteAsync("sp_HO_SO_CapNhat", p,
                commandType: CommandType.StoredProcedure);
            return true;
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
        private DynamicParameters generateHosoParameter(HosoModel model)
        {
            var p = new DynamicParameters();
            if(model.ID >0)
            {
                p.Add("ID", model.ID);
            }
            else
            {
                p.Add("MaHoSo", model.MaHoSo);
                p.Add("ID", dbType: DbType.Int32, direction: ParameterDirection.Output);
            }
            
            p.Add("CourierCode", model.CourierCode);
            p.Add("TenKhachHang", model.TenKhachHang);
            p.Add("CMND", model.CMND);
            p.Add("DiaChi", model.DiaChi);
            p.Add("MaKhuVuc", model.MaKhuVuc);
            p.Add("SDT", model.SDT);
            p.Add("SDT2", model.SDT2);
            p.Add("GioiTinh", model.GioTinh);
            p.Add("NgayTao", model.NgayTao);
            p.Add("MaNguoiTao", model.MaNguoiTao);
            p.Add("HoSoCuaAi", model.HoSoCuaAi);
            p.Add("KetQuaHS", model.KetQuaHS);
            p.Add("NgayNhanDon", model.NgayNhanDon);
            p.Add("MaTrangThai", model.MaTrangThai);
            p.Add("SanPhamVay", model.Sanphamvay);
            p.Add("CoBaoHiem", model.CoBaoHiem);
            p.Add("SoTienVay", model.SoTienVay);
            p.Add("HanVay", model.HanVay);
            p.Add("TenCuaHang", model.TenCuaHang);
            return p;
        }
    }
}

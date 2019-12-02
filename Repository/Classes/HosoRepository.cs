using Dapper;
using Entity.DatabaseModels;
using Entity.Enums;
using Entity.Infrastructures;
using Entity.PostModel;
using Entity.ViewModels;
using Microsoft.Extensions.Configuration;

using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
        public async Task<bool> DuyetHoso(HosoModel model)
        {
            var p = new DynamicParameters();

            p.Add("ID", model.Id);
            p.Add("MaNguoiCapNhat", model.UpdateBy);
            p.Add("NgayCapNhat", DateTime.Now);
            p.Add("CourierCode", model.CourierId);
            p.Add("TenKhachHang", model.CustomerName);
            p.Add("CMND", model.Cmnd);
            p.Add("DiaChi", model.Address);
            p.Add("MaKhuVuc", model.DistrictId);
            p.Add("SDT", model.Phone);
            p.Add("SDT2", model.Phone2);
            p.Add("GioiTinh", model.Gender);
            p.Add("HoSoCuaAi", model.CreatedBy);
            p.Add("KetQuaHS", model.Result);
            p.Add("NgayNhanDon", model.NgayNhandon);
            p.Add("MaTrangThai", model.Status);
            p.Add("SanPhamVay", model.ProductId);
            p.Add("CoBaoHiem", model.IsBaohiem);
            p.Add("SoTienVay", model.BorrowAmount);
            p.Add("HanVay", model.ThoihanVay);
            p.Add("birthDay", DateTime.Now);
            p.Add("cmndDay", DateTime.Now);
            using (var con = GetConnection())
            {
                await con.ExecuteAsync("sp_HO_SO_CapNhatHoSo", p, commandType: CommandType.StoredProcedure);
                return true;
            }
        }
        public async Task<List<OptionSimpleModelOld>> GetResultList()
        {
            using (var con = GetConnection())
            {
                var result = await con.QueryAsync<OptionSimpleModelOld>("sp_KET_QUA_HS_LayDSKetQua",

               commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
        }
        public async Task<List<OptionSimpleModelOld>> GetStatusList(bool isTeamlead)
        {
            using (var con = GetConnection())
            {
                var result = await con.QueryAsync<OptionSimpleModelOld>("sp_TRANG_THAI_HS_LayDSTrangThai",
              new
              {
                  isTeamlead = isTeamlead
              },
              commandType: CommandType.StoredProcedure);
                return result.ToList();
            }

        }
        public async Task<List<GhichuViewModel>> GetComments(int hosoId)
        {
            using (var con = GetConnection())
            {
                var result = await con.QueryAsync<GhichuViewModel>("sp_GetGhichuByHosoId",
               new
               {
                   hosoId
               },
               commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
        }
        public async Task Daxem(int hosoId)
        {
            using (var con = GetConnection())
            {
                await con.ExecuteAsync("sp_HO_SO_XEM_DaXem",
                  new
                  {
                      ID = hosoId
                  },
                  commandType: CommandType.StoredProcedure);
            }
               
        }
        public async Task AddHosoDaxem(int hosoId)
        {
            using (var con = GetConnection())
            {
                await con.ExecuteAsync("sp_HO_SO_XEM_Them",
                   new
                   {
                       ID = hosoId
                   },
                   commandType: CommandType.StoredProcedure);
            }
                
        }
        public async Task<bool> UpdateStatus(int hosoId, int userId, int status, int result, string comment)
        {
            using (var con = GetConnection())
            {
                await con.ExecuteAsync("sp_HO_SO_CapNhatTrangThaiHS",
                  new
                  {
                      ID = hosoId,
                      MaNguoiThaoTac = userId,
                      NgayThaoTac = DateTime.Now,
                      MaTrangThai = status,
                      MaKetQua = result,
                      GhiChu = ""
                  },
                  commandType: CommandType.StoredProcedure);
                return true;
            }
               
        }
        public async Task<HoSoInfoModel> GetHosoById(int hosoId)
        {
            try
            {
                using (var con = GetConnection())
                {
                    var result = await con.QueryFirstOrDefaultAsync<HoSoInfoModel>("sp_HO_SO_LayChiTiet",
                       new
                       {
                           ID = hosoId
                       },
                       commandType: CommandType.StoredProcedure);
                    return result;
                }
            }
            catch(Exception e)
            {
                return null;
            }
               
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

            using (var con = GetConnection())
            {
                return await con.ExecuteScalarAsync<int>("sp_HO_SO_Count_TimHoSoDuyet",
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
                
        }
        public async Task<List<HoSoQuanLyModel>> GetHosoDuyet(int maNVDangNhap,
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
            using (var con = GetConnection())
            {
                var result = await con.QueryAsync<HoSoQuanLyModel>("sp_HO_SO_TimHoSoDuyet",
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

        public async Task<int> Create(HosoModel model)
        {
            model.CreatedDate = DateTime.Now;
            var p = generateHosoParameter(model);
            using (var con = GetConnection())
            {
                await con.ExecuteAsync("sp_HO_SO_Them", p,
                    commandType: CommandType.StoredProcedure);
                var result = p.Get<int>("ID");
                return result;
            }
                
        }
        public async Task<bool> Update(HosoModel model)
        {
            model.UpdatedDate = DateTime.Now;
            var p = generateHosoParameter(model);
            using (var con = GetConnection())
            {
                await con.ExecuteAsync("sp_HO_SO_CapNhat", p,
                   commandType: CommandType.StoredProcedure);
                return true;
            }
               
        }
        public async Task<bool> CreateHosoDuyetXem(int hosoId)
        {
            using (var con = GetConnection())
            {
                await con.ExecuteAsync("sp_HO_SO_DUYET_XEM_Them",
                    new
                    {
                        ID = hosoId
                    },
                    commandType: CommandType.StoredProcedure);
                return true;
            }
                
        }

        public async Task<List<HoSoQuanLyModel>> GetHosoNotApprove(int userId,
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
            using (var con = GetConnection())
            {
                var result = await con.QueryAsync<HoSoQuanLyModel>("sp_HO_SO_TimHoSoDuyetChuaXem",
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
        public async Task<int> CountDanhsachHoso(int loginUserId,
            int maNhom,
            int userId,
            DateTime fromDate,
            DateTime toDate,
            string maHs,
            string cmnd,
            string trangthai,
            int loaiNgay,
            string freeText = null)
        {
            //var p = new DynamicParameters();
            //p.Add("MaNVDangNhap", loginUserId);
            //p.Add("MaNhom", maNhom);
            //p.Add("MaThanhVien", userId);
            //p.Add("TuNgay", fromDate);
            //p.Add("DenNgay", toDate);
            //p.Add("MaHS", maHs);
            //p.Add("CMND", cmnd);
            //p.Add("LoaiNgay", loaiNgay);
            //p.Add("TrangThai", trangthai);
            //p.Add("freeText", fromDate);
            using (var con = GetConnection())
            {
                return await con.ExecuteScalarAsync<int>("sp_HO_SO_Count_TimHoSoQuanLy",
                   new
                   {
                       MaNVDangNhap = loginUserId,
                       MaNhom = maNhom,
                       MaThanhVien = userId,
                       TuNgay = fromDate,
                       DenNgay = toDate,
                       MaHS = new DbString() { IsAnsi = true, Value = maHs },
                       CMND = new DbString() { IsAnsi = true, Value = cmnd },
                       LoaiNgay = loaiNgay,
                       TrangThai = trangthai,
                       freeText = freeText
                   },
                   commandType: CommandType.StoredProcedure);
            }
                
        }
        public async Task<List<HoSoQuanLyModel>> GetDanhsachHoso(int loginUserId,
                int maNhom,
                int userId,
                DateTime fromDate,
                DateTime toDate,
                string maHs,
                string cmnd,
                string trangthai,
                int loaiNgay,
                string freeText = null,
                int page = 0,
                int limit = 10,
                bool isDownload = false)
        {
            using (var con = GetConnection())
            {
                var result = await con.QueryAsync<HoSoQuanLyModel>("sp_HO_SO_TimHoSoQuanLy",
                   new
                   {
                       MaNVDangNhap = loginUserId,
                       MaNhom = maNhom,
                       MaThanhVien = userId,
                       TuNgay = fromDate,
                       DenNgay = toDate,
                       MaHS = new DbString() { IsAnsi = true, Value = maHs },
                       CMND = new DbString() { IsAnsi = true, Value = cmnd },
                       LoaiNgay = loaiNgay,
                       TrangThai = trangthai,
                       offset = (page - 1) * limit,
                       limit = limit,
                       freeText = freeText
                   },
                   commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
                
        }
        private DynamicParameters generateHosoParameter(HosoModel model)
        {
            var p = new DynamicParameters();
            if (model.Id > 0)
            {
                p.Add("ID", model.Id);
                p.Add("UpdatedUserId", model.UpdateBy);
                p.Add("UpdatedDate", DateTime.Now);
            }
            else
            {
                p.Add("MaHoSo", model.Code);
                p.Add("ID", dbType: DbType.Int32, direction: ParameterDirection.Output);
                p.Add("NgayTao", model.CreatedDate);
                p.Add("MaNguoiTao", model.CreatedBy);
                p.Add("HoSoCuaAi", model.HoSoCuaAi);
            }

            p.Add("CourierCode", model.CourierId);
            p.Add("TenKhachHang", model.CustomerName);
            p.Add("CMND", model.Cmnd);
            p.Add("DiaChi", model.Address);
            p.Add("MaKhuVuc", model.DistrictId);
            p.Add("SDT", model.Phone);
            p.Add("SDT2", model.Phone2);
            p.Add("GioiTinh", model.Gender);
            
            p.Add("KetQuaHS", model.Result);
            p.Add("NgayNhanDon", model.NgayNhandon);
            p.Add("MaTrangThai", model.Status);
            p.Add("SanPhamVay", model.ProductId);
            p.Add("CoBaoHiem", model.IsBaohiem);
            p.Add("SoTienVay", model.BorrowAmount);
            p.Add("HanVay", model.ThoihanVay);
            p.Add("TenCuaHang", "");
            p.Add("birthDay", model.BirthDay);
            p.Add("cmndDay", model.CmndDay);
            return p;
        }
    }
}

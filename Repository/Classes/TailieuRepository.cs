using Dapper;
using Entity.Infrastructures;
using Entity.ViewModels;
using Microsoft.Extensions.Configuration;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Classes
{
    public class TailieuRepository : BaseRepository, ITailieuRepository
    {
        public TailieuRepository(IConfiguration configuration) : base(configuration)
        {

        }
        public async Task<bool> RemoveAllTailieu(int hosoId)
        {
            using (var con = GetConnection())
            {
                var p = new DynamicParameters();
                p.Add("MaHS", hosoId);
                await con.ExecuteAsync("sp_TAI_LIEU_HS_XoaTatCa", p,
                    commandType: CommandType.StoredProcedure);
                return true;
            }
               
        }
        public async Task<bool> Add(TaiLieu model)
        {
            using (var con = GetConnection())
            {
                var p = new DynamicParameters();
                p.Add("Maloai", model.TypeId);
                p.Add("DuongDan", model.FilePath);
                p.Add("Ten", model.FileName);
                p.Add("MaHS", model.HosoId);
                await con.ExecuteAsync("sp_TAI_LIEU_HS_Them", p,
                    commandType: CommandType.StoredProcedure);
                return true;
            }
                
        }

        public async Task<List<LoaiTaiLieuModel>> GetLoaiTailieuList()
        {
            using (var con = GetConnection())
            {
                var result = await con.QueryAsync<LoaiTaiLieuModel>("sp_LOAI_TAI_LIEU_LayDS", null, commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
                
        }
        public async Task<List<FileUploadModel>> GetTailieuByHosoId(int hosoId)
        {
            using (var con = GetConnection())
            {
                var p = new DynamicParameters();
                p.Add("hosoId", hosoId);

                var result = await con.QueryAsync<FileUploadModel>("getTailieuByHosoId", p,
                    commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
                
        }
        public async Task<bool> RemoveTailieu(int hosoId, int tailieuId)
        {
            var p = new DynamicParameters();
            p.Add("hosoId", hosoId);
            p.Add("tailieuId", tailieuId);
            using (var con = GetConnection())
            {
                var result = await con.ExecuteAsync("removeTailieu", p,
                    commandType: CommandType.StoredProcedure);
                return true;
            }
                
        }
        public async Task<bool> UpdateExistingFile(int fileId, string name, string url)
        {
            var p = new DynamicParameters();
            p.Add("fileId", fileId);
            p.Add("name", name);
            p.Add("url", url);
            using (var con = GetConnection())
            {
                var result = await con.ExecuteAsync("updateExistingFile", p,
                    commandType: CommandType.StoredProcedure);
                return true;
            }
        }
    }
}


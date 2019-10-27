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
            var p = new DynamicParameters();
            p.Add("MaHS", hosoId);
            await connection.ExecuteAsync("sp_TAI_LIEU_HS_XoaTatCa", p,
                commandType: CommandType.StoredProcedure);
            return true;
        }
        public async Task<bool> Add(TaiLieu model)
        {
            var p = new DynamicParameters();
            p.Add("Maloai", model.TypeId);
            p.Add("DuongDan", model.FilePath);
            p.Add("Ten", model.FileName);
            p.Add("MaHS", model.HosoId);
            await connection.ExecuteAsync("sp_TAI_LIEU_HS_Them", p,
                commandType: CommandType.StoredProcedure);
            return true;
        }

        public async Task<List<LoaiTaiLieuModel>> GetLoaiTailieuList()
        {
            var result = await connection.QueryAsync<LoaiTaiLieuModel>("sp_LOAI_TAI_LIEU_LayDS",null,commandType: CommandType.StoredProcedure);
            return result.ToList();
        }
    }
}


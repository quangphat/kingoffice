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
    public class LoaiTailieuRepository : BaseRepository, ILoaiTailieuRepository
    {
        public LoaiTailieuRepository(IConfiguration configuration) : base(configuration)
        {

        }
        public async Task<List<LoaiTaiLieuModel>> GetList()
        {
            var result = await connection.QueryAsync<LoaiTaiLieuModel>("sp_LOAI_TAI_LIEU_LayDS",null,commandType: CommandType.StoredProcedure);
            return result.ToList();
        }
    }
}


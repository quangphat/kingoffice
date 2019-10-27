using Dapper;
using Entity.DatabaseModels;
using Entity.Infrastructures;
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
    public class NotesRepository : BaseRepository, INotesRepository
    {
        public NotesRepository(IConfiguration configuration) : base(configuration)
        {

        }
        public async Task<bool> Add(GhichuModel model)
        {
            IDbCommand command = new SqlCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = "insert into Ghichu (UserId,Noidung,HosoId, CommentTime) values(@UserId,@Noidung,@HosoId,@CommentTime)";
            var p = new DynamicParameters();
            p.Add("UserId", model.UserId);
            p.Add("HosoId", model.HosoId);
            p.Add("Noidung", model.Noidung);
            p.Add("CommentTime", model.CommentTime);
            await connection.ExecuteAsync("insert into Ghichu (UserId,Noidung,HosoId, CommentTime) values(@UserId,@Noidung,@HosoId,@CommentTime)"
                , p, commandType: CommandType.Text);
            return true;
        }
    }
}


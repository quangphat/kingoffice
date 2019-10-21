using System;
using System.Collections.Generic;
using System.Text;

namespace Entity.DatabaseModels
{
    public class GhichuModel
    {
        public long Id { get; set; }
        public int UserId { get; set; }
        public int HosoId { get; set; }
        public string Noidung { get; set; }
        public DateTime CommentTime { get; set; }
    }
}

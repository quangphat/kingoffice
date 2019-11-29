using System;
using System.Collections.Generic;
using System.Text;

namespace Entity.DatabaseModels
{
    public class Customer : BaseSqlEntity
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public DateTime CheckDate { get; set; }
        public string Cmnd { get; set; }
        public int CICStatus { get; set; }
        public bool Gender { get; set; }
        public string LastNote { get; set; }
        public string MatchCondition { get; set; }
        public string NotMatch { get; set; }
    }
}

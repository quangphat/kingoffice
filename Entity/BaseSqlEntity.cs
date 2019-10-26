using System;
using System.Collections.Generic;
using System.Text;

namespace Entity
{
    public class BaseSqlEntity
    {
        public DateTime CreatedTime { get; set; }
        public int CreatedBy { get; set; }
        public DateTime UpdatedTime { get; set; }
        public int UpdatedBy { get; set; }
    }
}

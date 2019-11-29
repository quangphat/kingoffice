using System;
using System.Collections.Generic;
using System.Text;

namespace Entity.ViewModels
{
    public class CustomerNote : BaseSqlEntity
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string Note { get; set; }
    }
}

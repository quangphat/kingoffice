using System;
using System.Collections.Generic;
using System.Text;

namespace Entity.ViewModels
{
    public class CustomerCheck : BaseSqlEntity
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int PartnerId { get; set; }
        public int Status { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Entity.DatabaseModels
{
    public class Product
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int PartnerId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int Type { get; set; }
    }
}

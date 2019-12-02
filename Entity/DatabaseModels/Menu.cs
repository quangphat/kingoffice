using System;
using System.Collections.Generic;
using System.Text;

namespace Entity.DatabaseModels
{
    public class Menu
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int Value { get; set; }
        public int ParentId { get; set; }
        public bool Deleted { get; set; }
    }
}

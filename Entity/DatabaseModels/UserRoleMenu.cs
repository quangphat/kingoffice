using System;
using System.Collections.Generic;
using System.Text;

namespace Entity.DatabaseModels
{
    public class UserRoleMenu
    {
        public int Id { get; set; }
        public int MenuId { get; set; }
        public string UserRole { get; set; }
    }
}

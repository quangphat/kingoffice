using System;
using System.Collections.Generic;
using System.Text;

namespace Entity.PostModel
{
    public class UpdateUserRole
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public string secret { get; set; }
    }
    public class UpdateUserRoleForMultipleUser
    {
        public List<int> UserIds { get; set; }
        public int RoleId { get; set; }
        public string secret { get; set; }
    }
}

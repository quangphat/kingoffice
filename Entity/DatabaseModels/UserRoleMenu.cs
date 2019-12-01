using System;
using System.Collections.Generic;
using System.Text;

namespace Entity.DatabaseModels
{
    public class UserRoleMenu
    {
        public int Id { get; set; }
        public int MenuId { get; set; }
        public string RoleCode { get; set; }
        public int RoleId { get; set; }
        public string MenuName { get; set; }
    }
    public class UserRoleMenuForMultipleMenu
    {
        public int Id { get; set; }
        public List<int> MenuIds { get; set; }
        public string RoleCode { get; set; }
        public int RoleId { get; set; }
        public string MenuName { get; set; }
    }
}

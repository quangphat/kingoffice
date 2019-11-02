using System;
using System.Collections.Generic;
using System.Text;

namespace Entity.ViewModels
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Code { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public DateTime? WorkDate { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Location { get; set; }
        public string Phone { get; set; }
        public DateTime? CreatedTime { get; set; }
    }
}

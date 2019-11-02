using System;
using System.Collections.Generic;
using System.Text;

namespace Entity.ViewModels
{
    public class UserModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public int ProvinceId { get; set; }
        public int DistrictId { get; set; }
        public int RoleId { get; set; }
        public DateTime WorkDate { get; set; }
    }
}

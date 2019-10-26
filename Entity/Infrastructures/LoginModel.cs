using System;
using System.Collections.Generic;
using System.Text;

namespace Entity.Infrastructures
{
    public class LoginModel
    {
        public string userName { get; set; }
        public string password { get; set; }
        public bool remember { get; set; }
    }
}

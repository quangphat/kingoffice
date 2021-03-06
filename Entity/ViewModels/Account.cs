﻿using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entity.ViewModels
{
    public class Account
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Code { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }
        public string Email { get; set; }
        public string Fullname { get; set; }
        public string Scope { get; set; }
    }
    
}

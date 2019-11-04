using System;
using System.Collections.Generic;
using System.Text;

namespace Entity.ViewModels
{
    public class ResetPassModel
    {
        public int Id { get; set; }
        public string OldPass { get; set; }
        public string NewPass { get; set; }
        public string ConfirmPass { get; set; }
    }
}

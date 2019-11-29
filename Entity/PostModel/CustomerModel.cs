using System;
using System.Collections.Generic;
using System.Text;

namespace Entity.PostModel
{
    public class CustomerModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public DateTime? CheckDate { get; set; }
        public string Cmnd { get; set; }
        public int CICStatus { get; set; }
        public bool Gender { get; set; }
        public string Note { get; set; }
    }
}

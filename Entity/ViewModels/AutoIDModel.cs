using System;
using System.Collections.Generic;
using System.Text;

namespace Entity.ViewModels
{
    public class AutoIDModel
    {
        public int ID { get; set; }
        public string NameID { get; set; }
        public string Prefix { get; set; }
        public string Suffixes { get; set; }
        public int Value { get; set; }
    }
}

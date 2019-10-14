using System;
using System.Collections.Generic;
using System.Text;

namespace Entity.ViewModels
{
    public class OptionSimpleModel
    {
        public int ID { get; set; }
        public string Ten { get; set; }
    }
    public class OptionSimpleModelV2:OptionSimpleModel
    {
        public int MaNguoiQL { get; set; }
        public string ChuoiMaCha { get; set; }
        public string TenQL { get; set; }
    }
}

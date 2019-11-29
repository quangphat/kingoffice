using System;
using System.Collections.Generic;
using System.Text;

namespace Entity.ViewModels
{
    public class OptionSimpleModelOld
    {
        public int ID { get; set; }
        public string Ten { get; set; }
    }
    public class OptionSimple
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }
    public class OptionSimpleWithIsSelect
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsSelect { get; set; }
    }
    public class OptionSimpleExtendForProduct:OptionSimple
    {
        public string CreatedName { get; set; }
    }
    public class OptionSimpleModelV2 : OptionSimpleModelOld
    {
        public int MaNguoiQL { get; set; }
        public string ChuoiMaCha { get; set; }
        public string TenQL { get; set; }
    }
}

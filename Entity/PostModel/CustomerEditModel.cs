using Entity.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entity.PostModel
{
    public class CustomerEditModel
    {
        public CustomerModel Customer { get; set; }
        public List<OptionSimpleWithIsSelect> Partners { get; set; }
    }
}

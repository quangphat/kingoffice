using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Options;
using Entity.F88;
using Entity.Infrastructures;

namespace F88ServiceApi
{
    public abstract class F88ServiceBase
    {
        protected readonly string _basePath;
        public F88ServiceBase(IOptions<F88Api> option)
        {
            _basePath = option.Value.LadiPage;
        }
    }
}

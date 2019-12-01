using Entity.F88;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace F88ServiceApi.Interface
{
    public interface IF88Service
    {
        Task<F88ResponseModel> LadipageReturnID(LadipageModel model);
    }
}

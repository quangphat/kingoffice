using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Options;
using Entity.F88;
using Entity.Infrastructures;
using System.Linq;

namespace F88ServiceApi
{
    public abstract class F88ServiceBase
    {
        protected readonly string _basePath;
        public F88ServiceBase(IOptions<F88Api> option)
        {
            _basePath = option.Value.LadiPage;
        }
        protected F88ResponseModel ToF88Response(ResponseModel model)
        {
            var result = new F88ResponseModel();
            result.Success = false;
            result.Message = "";
            if (model.Code == 200 && model.Data != null)
            {
                if (model.Data.Count() == 2)
                {
                    result.Success = false;
                    result.Message = "Số điện thoại đã tồn tại";
                    return result;
                }
                if (model.Data.Count() == 4)
                {
                    result.Success = true;
                    result.Message = "Thành công";
                    return result;
                }
                return result;
            }
            else
            {
                result.Success = false;
                result.Message = $"Code:{model.Code} . Message: {model.Message}";
                return result;
            }
        }
        protected F88ResponseModel ToF88Response(bool success, string message = null)
        {
            return new F88ResponseModel
            {
                Success = success,
                Message = message
            };
        }
    }
}

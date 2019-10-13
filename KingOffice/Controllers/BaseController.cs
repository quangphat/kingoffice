using Entity.Infrastructures;
using KingOffice.Infrastructures;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KingOffice.Controllers
{
    public class BaseController : Controller
    {
        public readonly CurrentProcess _process;
        public BaseController(CurrentProcess process)
        {
            _process = process;
        }
        private JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Include,
            ContractResolver = new DefaultContractResolver()
        };

        public IActionResult ToResponse()
        {
            var model = new ResponseJsonModel();
            bool hasError = _checkHasError(model);
            return Json(model, _jsonSerializerSettings);
        }
        public IActionResult ToResponse<T>(T data) where T : class
        {
            var model = new ResponseJsonModel<T>();
            if (!_checkHasError(model))
                model.data = data;

            return Json(model, _jsonSerializerSettings);
        }
        public IActionResult ToResponse(bool isSuccess)
        {
            var model = new ResponseJsonModel();

            if (!_checkHasError(model))
                model.success = isSuccess;

            return Json(model, _jsonSerializerSettings);
        }
        public IActionResult ToFailResponse(string message = null)
        {
            var errorMessage = _process.ToError();
            var model = new ErrorRMessage()
            {
                Result = false,
                ErrorMessage = message,
                Code = errorMessage?.Message
            };
            return Json(new { Message = model, newurl = string.Empty }, _jsonSerializerSettings);
        }
        private bool _checkHasError(ResponseJsonModel model)
        {
            var hasError = _process.HasError;
            if (hasError)
            {
                var errorMessage = _process.ToError();

                model.error = new ErrorJsonModel()
                {
                    Result = false,
                    code = errorMessage.Message,
                    ErrorMessage = errorMessage.Message,
                    trace_keys = errorMessage.TraceKeys
                };
            }
            model.success = !hasError;
            return hasError;
        }

    }
    public class ResponseJsonModel
    {
        public ErrorJsonModel error { get; set; }
        public bool success { get; set; }
    }

    public class ResponseJsonModel<T> : ResponseJsonModel where T : class
    {
        public T data { get; set; }
    }
    public class ErrorJsonModel
    {
        public string code { get; set; }
        public List<object> trace_keys { get; set; }
        public bool Result { get; set; }
        public string MessageId { get; set; }
        public string ErrorMessage { get; set; }
        public string SystemMessage { get; set; }
    }
    public class ErrorRMessage
    {
        public bool Result { get; set; }
        public string MessageId { get; set; }
        public string ErrorMessage { get; set; }
        public string SystemMessage { get; set; }
        public string Code { get; set; }
    }
}

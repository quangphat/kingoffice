using System;
using System.Collections.Generic;
using System.Text;

namespace Entity.F88
{
    public class ResponseModel
    {
        public List<string> Data { get; set; }
        public int Code { get; set; }
        public string Message { get; set; }
        public string SystemMessage { get; set; }
    }
}

using Entity.F88;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using HttpHelper;
using Newtonsoft.Json;
using System.Linq;
using Microsoft.Extensions.Options;
using Entity.Infrastructures;
using F88ServiceApi.Interface;

namespace F88ServiceApi
{
    public class F88Service : F88ServiceBase, IF88Service
    {
        public F88Service(IOptions<F88Api> option) : base(option)
        {

        }


        public async Task<F88ResponseModel> LadipageReturnID(LadipageModel model)
        {
            if (model == null)
                return ToF88Response(false);
            if (string.IsNullOrWhiteSpace(model.Name) || string.IsNullOrWhiteSpace(model.Phone))
                return ToF88Response(false, "Tên hoặc số điện thoại không được bỏ trống");
            try
            {
                var result = await PostLadipageReturnID(model);
                return ToF88Response(result);
            }
            catch (Exception e)
            {
                return ToF88Response(false, e.Message);
            }
        }
        private async Task<ResponseModel> PostLadipageReturnID(LadipageModel model)
        {
            model.Select1 = "";
            model.ReferenceType = 10;
            HttpClient _httpClient = new HttpClient();
            var response = await _httpClient.Post(_basePath, "/LadipageReturnID", null, model);
            if (response == null || response.Content == null)
                return null;
            var json = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<ResponseModel>(json);
        }

    }
}

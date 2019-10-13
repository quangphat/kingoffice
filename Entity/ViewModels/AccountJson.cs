using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entity.ViewModels
{
    public class AccountJson
    {
        public AccountJson(Account account)
        {
            this.Account = account;
        }
        public Account Account { get; set; }
        public string ToJson
        {
            get
            {
                return PrepareAccountJson(Account);
            }
        }
        private string PrepareAccountJson(Account account)
        {
            if (account == null) return null;
            return JsonConvert.SerializeObject(Account, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
        }
    }
}

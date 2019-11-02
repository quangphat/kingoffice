using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Interfaces;
using Entity.Infrastructures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KingOffice.Controllers
{
    [Route("roles")]
    public class RoleController : BaseController
    {
        protected readonly IRoleBusiness _bizRole;
        public RoleController(CurrentProcess process, IRoleBusiness roleBusiness) : base(process)
        {
            _bizRole = roleBusiness;
        }
        [HttpGet]
        public async Task<IActionResult> Gets()
        {
            var result = await _bizRole.GetList();
            return ToResponse(result);
        }
    }
}
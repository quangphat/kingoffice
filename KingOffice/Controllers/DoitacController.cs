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
    [Route("Partners")]
    public class DoitacController : BaseController
    {
        protected readonly IDoitacBusiness _bizDoitac;
        public DoitacController(CurrentProcess process, IDoitacBusiness doitacBusiness) : base(process)
        {
            _bizDoitac = doitacBusiness;
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> LayDSDoiTac()
        {
            var result = await _bizDoitac.GetListSimple();
            return ToResponse(result);
        }
    }
}
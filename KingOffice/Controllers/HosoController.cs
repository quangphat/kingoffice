using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Interfaces;
using Entity.Infrastructures;
using Microsoft.AspNetCore.Mvc;

namespace KingOffice.Controllers
{
    public class HosoController : BaseController
    {
        protected readonly IHosoBusiness _bizHoso;
        public HosoController(CurrentProcess process, IHosoBusiness hosoBusiness) : base(process)
        {
            _bizHoso = hosoBusiness;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> GetNotificationHosoNotApprove()
        {
            var result = await _bizHoso.GetHosoNotApprove();
            return ToResponse(result);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Interfaces;
using Entity.Infrastructures;
using KingOffice.Infrastructures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KingOffice.Controllers
{
    [Route("employee")]
    public class NhanvienController : BaseController
    {
        protected readonly INhanvienBusiness _bizNhanvien;
        public NhanvienController(CurrentProcess process,INhanvienBusiness nhanvienBusiness) : base(process)
        {
            _bizNhanvien = nhanvienBusiness;
        }
        [MyAuthorize]
        [HttpPost]
        public async Task<IActionResult> CreateUser()
        {
            return ToResponse(true);
        }
        [Authorize]
        [HttpGet("courier")]
        public async Task<IActionResult> GetCourierSimpleList()
        {
            var result = await _bizNhanvien.GetCourierList();
            return ToResponse(result);
        }
        [Authorize]
        [HttpGet("simple/{userId}")]
        public async Task<IActionResult> GetSimpleListByUserId(int userId)
        {
            var result = await _bizNhanvien.GetListByUserId(userId);
            return ToResponse(result);
        }
    }
}
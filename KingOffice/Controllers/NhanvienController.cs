using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Interfaces;
using Entity.Infrastructures;
using Microsoft.AspNetCore.Mvc;

namespace KingOffice.Controllers
{
    public class NhanvienController : BaseController
    {
        protected readonly INhanvienBusiness _bizNhanvien;
        public NhanvienController(CurrentProcess process,INhanvienBusiness nhanvienBusiness) : base(process)
        {
            _bizNhanvien = nhanvienBusiness;
        }
        public async Task<IActionResult> GetCourierSimpleList()
        {
            var result = await _bizNhanvien.GetCourierList();
            return ToResponse(result);
        }
        public async Task<IActionResult> GetSimpleListByUserId(int userId)
        {
            var result = await _bizNhanvien.GetListByUserId(userId);
            return ToResponse(result);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Interfaces;
using Entity.Infrastructures;
using Entity.ViewModels;
using KingOffice.Infrastructures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KingOffice.Controllers
{
    [Route("employee")]
    public class EmployeeController : BaseController
    {
        protected readonly INhanvienBusiness _bizNhanvien;
        public EmployeeController(CurrentProcess process,INhanvienBusiness nhanvienBusiness) : base(process)
        {
            _bizNhanvien = nhanvienBusiness;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View();
        }
        [HttpGet("add")]
        public async Task<IActionResult> Add()
        {
            return View();
        }
        //[MyAuthorize]
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserModel model)
        {
            var result = await _bizNhanvien.Create(model);
            return ToResponse(result);
        }
        [HttpGet]
        [Route("list")]
        public async Task<IActionResult> Gets(DateTime? workFromDate, DateTime? workToDate,
            string freetext = "",
            int roleId =0,
            int page = 1, int limit = 10)
        {
            var result = await _bizNhanvien.Gets(workFromDate, workToDate, freetext,roleId, page, limit);
            return ToResponse(DataPaging.Create(result.datas, result.totalRecord));
        }
        [HttpGet]
        [Route("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var result = await _bizNhanvien.GetById(id);
            ViewBag.employee = result;
            ViewBag.account = _process.User;
            return View();
        }
        [HttpGet]
        [Route("getuser/{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var result = await _bizNhanvien.GetById(id);
            return ToResponse(result);
        }
        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> Update([FromBody] EmployeeEditModel model)
        {
            var result = await _bizNhanvien.Update(model);
            return ToResponse(result);
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
        [Authorize]
        [HttpGet("sale")]
        public async Task<IActionResult> GetSaleList()
        {
            var result = await _bizNhanvien.GetSaleList();
            return ToResponse(result);
        }
        [Authorize]
        [HttpGet("all/simplelist")]
        public async Task<IActionResult> GetAllSimpleList()
        {
            var result = await _bizNhanvien.GetAllEmployeeSimpleList();
            return ToResponse(result);
        }
        
    }
}
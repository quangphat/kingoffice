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
    [Route("teams")]
    public class TeamController : BaseController
    {
        protected readonly INhanvienBusiness _bizNhanvien;
        public TeamController(CurrentProcess process, INhanvienBusiness nhanvienBusiness) : base(process)
        {
            _bizNhanvien = nhanvienBusiness;
        }
        [Authorize]
        [HttpGet("Taomoi")]
        public async Task<IActionResult> Taomoi()
        {
            return View();
        }
        [Authorize]
        [HttpGet("simplelist")]
        public async Task<IActionResult> GetAllTeamSimpleList()
        {
            var result = await _bizNhanvien.GetAllTeamSimpleList();
            return ToResponse(result);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateTeam([FromBody] Team model)
        {
            var result = await _bizNhanvien.CreateTeam(model);
            return ToResponse(result);
        }
        [Authorize]
        [HttpGet("Danhsach")]
        public async Task<IActionResult> Danhsach()
        {
            return View();
        }
        [Authorize]
        [HttpGet("list")]
        public async Task<IActionResult> GetChildTeam(int parentId, int page =1, int limit = 10)
        {
            var result = await _bizNhanvien.GetTeamsByParentId(parentId, page, limit);
            return ToResponse(DataPaging.Create(result.datas,result.totalRecord));
        }
        [Authorize]
        [HttpGet("capnhat/{id}")]
        public async Task<IActionResult> Capnhat(int id)
        {
            ViewBag.team = await _bizNhanvien.GetTeamById(id);
            return View();
        }
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id,[FromBody] Team model)
        {
            var result = await _bizNhanvien.UpdateTeam(model);
            return ToResponse(result);
        }
        [Authorize]
        [HttpGet("member/{id}")]
        public async Task<IActionResult> GetTeamMember(int id)
        {
            var members = await _bizNhanvien.GetTeamMember(id);
            var notMembers = await _bizNhanvien.GetUserNotInTeam(id);
            return Json(new { members, notMembers });
        }
        [Authorize]
        [HttpGet("usernotinteam/{id}")]
        public async Task<IActionResult> GetUserNotInTeam(int id)
        {
            ViewBag.team = await _bizNhanvien.GetUserNotInTeam(id);
            return View();
        }
        [Authorize]
        [HttpGet("Chitiet/{id}")]
        public async Task<IActionResult> Chitiet(int id)
        {
            ViewBag.team = await _bizNhanvien.GetTeamById(id, true);
            return View();
        }
        [Authorize]
        [HttpGet("members")]
        public async Task<IActionResult> GetTeamMemberDetail(int teamId, int page = 1, int limit = 10)
        {
            var result = await _bizNhanvien.GetTeamMembers(teamId, page, limit);
            return ToResponse(DataPaging.Create(result.datas, result.totalRecord));
        }
    }
}
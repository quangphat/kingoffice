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
    public class DuyetHoSoController : BaseController
    {
        protected readonly IHosoBusiness _bizHoso;
        public DuyetHoSoController(IHosoBusiness hosoBusiness, CurrentProcess process) : base(process)
        {
            _bizHoso = hosoBusiness;
        }
        public static Dictionary<string, ActionInfo> LstRole
        {
            get
            {
                Dictionary<string, ActionInfo> _lstRole = new Dictionary<string, ActionInfo>();
                _lstRole.Add("Index", new ActionInfo() { _formindex = IndexMenu.M_2_3, _href = "DuyetHoSo/Index", _mangChucNang = new int[] { (int)QuyenIndex.DuyetHoSo } });
                return _lstRole;
            }
        }
        [MyAuthorize(Roles = "admin")]
        public IActionResult Index()
        {
            ViewBag.formindex = LstRole[RouteData.Values["action"].ToString()]._formindex;
            ViewBag.account = _process.User;
            return View();
        }
        [Authorize]
        public async Task<IActionResult> TimHS(
            DateTime? fromDate,
            DateTime? toDate,
            string maHS,
            string cmnd,
            int loaiNgay,
            int maNhom = 0,
            string freetext = null,
            string status = null,
            int page = 1, int limit = 10,
            int maThanhVien = 0)
        {
            var data = await _bizHoso.GetHosoDuyet(fromDate, toDate, maHS,
                cmnd, loaiNgay, maNhom, status, freetext, page, limit, maThanhVien);
            var result = DataPaging.Create(data.datas, data.TotalRecord);
            return ToResponse(result);
        }
    }
}
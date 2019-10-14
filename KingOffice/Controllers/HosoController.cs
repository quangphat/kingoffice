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
    public class HosoController : BaseController
    {
        protected readonly IHosoBusiness _bizHoso;
        protected readonly ILoaiTailieuBusiness _bizLoaiTl;
        protected readonly IDoitacBusiness _bizDoitac;
        public HosoController(CurrentProcess process, 
            ILoaiTailieuBusiness loaiTailieuBusiness,
            IDoitacBusiness doitacBusiness,
            IHosoBusiness hosoBusiness) : base(process)
        {
            _bizHoso = hosoBusiness;
            _bizLoaiTl = loaiTailieuBusiness;
            _bizDoitac = doitacBusiness;
        }
        public static Dictionary<string, ActionInfo> LstRole
        {
            get
            {
                Dictionary<string, ActionInfo> _lstRole = new Dictionary<string, ActionInfo>();
                _lstRole.Add("AddNew", new ActionInfo() { _formindex = IndexMenu.M_1_1, _href = "HoSo/AddNew", _mangChucNang = new int[] { (int)QuyenIndex.Public } });
                _lstRole.Add("Index", new ActionInfo() { _formindex = IndexMenu.M_1_2, _href = "HoSo/Index", _mangChucNang = new int[] { (int)QuyenIndex.Public } });
                return _lstRole;
            }

        }
        public IActionResult Index()
        {
            return View();
        }
        [Authorize]
        public async Task<IActionResult> GetNotificationHosoNotApprove()
        {
            var result = await _bizHoso.GetHosoNotApprove();
            return ToResponse(result);
        }
        [Authorize]
        public ActionResult AddNew()
        {
            ViewBag.formindex = LstRole[RouteData.Values["action"].ToString()]._formindex;
            ViewBag.MaNV = _process.User.Id;
            return View();
        }
        [Authorize]
        public async Task<IActionResult> LayDSTaiLieu()
        {
            var result = await _bizLoaiTl.GetList();
            return ToResponse(result);
        }
        [Authorize]
        public async Task<IActionResult> LayDSDoiTac()
        {
            var result = await _bizDoitac.GetListSimple();
            return ToResponse(result);
        }
    }
}
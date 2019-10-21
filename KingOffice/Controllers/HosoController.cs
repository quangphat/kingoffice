using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Business.Interfaces;
using Entity.Infrastructures;
using Entity.ViewModels;
using KingOffice.Infrastructures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace KingOffice.Controllers
{
    [Route("hoso")]
    public class HosoController : BaseController
    {
        protected readonly IHosoBusiness _bizHoso;
        protected readonly ITailieuBusiness _bizLoaiTl;
        protected readonly IHostingEnvironment _hosting;
        public HosoController(CurrentProcess process,
            ITailieuBusiness loaiTailieuBusiness,
            IHostingEnvironment hosting,
        IHosoBusiness hosoBusiness) : base(process)
        {
            _bizHoso = hosoBusiness;
            _bizLoaiTl = loaiTailieuBusiness;
            _hosting = hosting;
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
        [Authorize]
        [HttpGet("notification")]
        public async Task<IActionResult> GetNotificationHosoNotApprove()
        {
            var result = await _bizHoso.GetHosoNotApprove();
            return ToResponse(result);
        }
        [Authorize]
        [HttpGet("AddNew")]
        public ActionResult AddNew()
        {
            ViewBag.formindex = LstRole[RouteData.Values["action"].ToString()]._formindex;
            ViewBag.MaNV = _process.User.Id;
            var hoso = new HosoRequestModel();
            ViewBag.hoso = Utils.ConvertToJson(hoso);
            return View();
        }
        [Authorize]
        [HttpGet("tailieu")]
        public async Task<IActionResult> LayDSTaiLieu()
        {
            var result = await _bizLoaiTl.GetList();
            return ToResponse(result);
        }

        //public async  Task<IActionResult> SaveDaft(string hoten, string phone, string phone2, string ngayNhanDon, int hoSoCuaAi, string cmnd, int gioiTinh
        //   , int maKhuVuc, string diaChi, int courier, int sanPhamVay, string tenCuaHang, int baoHiem, int thoiHanVay, string soTienVay, string ghiChu)
        //{
        //    return ToResponse(true);
        //}
        [Authorize]
        [HttpPost("draft")]
        public async Task<IActionResult> SaveDaft([FromBody]HosoRequestModel model)
        {
            var result = await _bizHoso.Save(model, true);
            return ToResponse(result);
        }
        [Authorize]
        [HttpPost("test")]
        public async Task<IActionResult> SaveDaft([FromBody]TestModel model)
        {
            //var result = await _bizHoso.Save(model, true);
            return ToResponse(true);
        }
        [Authorize]
        [HttpPost("UploadHoso/{hosoId}")]
        public async Task<IActionResult> UploadHoso(int hosoId, [FromBody] List<FileUploadModel> files)
        {
            var result = await _bizHoso.UploadHoso(hosoId, files, _hosting.WebRootPath);
            return ToResponse(result);
        }

    }
}
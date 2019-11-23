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
        protected readonly ITailieuBusiness _bizTailieu;
        protected readonly IHostingEnvironment _hosting;
        
        public HosoController(CurrentProcess process,
            ITailieuBusiness loaiTailieuBusiness,
            IHostingEnvironment hosting,
            
        IHosoBusiness hosoBusiness) : base(process)
        {
            _bizHoso = hosoBusiness;
            _bizTailieu = loaiTailieuBusiness;
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
        [MyAuthorize(Roles = RoleConsts.hoso_read)]
        [HttpGet("notification")]
        public async Task<IActionResult> GetNotificationHosoNotApprove()
        {
            var result = await _bizHoso.GetHosoNotApprove();
            return ToResponse(result);
        }
        [MyAuthorize(Roles = RoleConsts.hoso_write)]
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
            var result = await _bizTailieu.GetList();
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
        [HttpPost("submit")]
        public async Task<IActionResult> Submit([FromBody]HosoRequestModel model)
        {
            var result = await _bizHoso.Save(model, false);
            return ToResponse(result);
        }
        [Authorize]
        [HttpPost("UploadHoso/{hosoId}")]
        public async Task<IActionResult> UploadHoso(int hosoId, [FromBody] List<FileUploadModelGroupByKey> files)
        {
            var result = await _bizHoso.UploadHoso(hosoId, files, _hosting.WebRootPath);
            return ToResponse(result);
        }
        [Authorize]
        [HttpPost("uploadhoso/{hosoId}/{key}/{isDraft}")]
        public async Task<IActionResult> UploadFile(int hosoId,int key,bool isDraft, List<IFormFile> files)
        {
            var result = await _bizHoso.UploadHoso(hosoId, key, files, _hosting.WebRootPath, !isDraft);
            return ToResponse(result);
        }
        [Authorize]
        [HttpPost("uploadhoso/{hosoId}/test")]
        public async Task<IActionResult> UploadFile(int hosoId, HosoFilesModel files)
        {
            var result = true;// await _bizHoso.UploadHoso(hosoId, key, files, _hosting.WebRootPath, !isDraft);
            return ToResponse(result);
        }
        [Authorize]
        [HttpGet("DanhsachHoso")]
        public async Task<IActionResult> DanhsachHoso()
        {
            ViewBag.formindex = "2_2";
            return View();
        }
        [Authorize]
        [HttpGet("search")]
        public async Task<IActionResult> Search(string maHs,
            string cmnd,
            DateTime? fromDate,
            DateTime? toDate,
            int loaiNgay = 1,
            int nhomId = 0,
            int userId = 0,
            string freetext = null,
            string status = null,
            int page = 1, int limit = 10)
        {
            var data = await _bizHoso.GetDanhsachHoso(maHs, cmnd, fromDate, toDate, loaiNgay, nhomId, userId, freetext, status, page, limit);
            var result = DataPaging.Create(data.datas, data.totalRecord);
            return ToResponse(result);
        }
        [Authorize]
        [HttpGet("chitiethoso/{hosoId}")]
        public async Task<IActionResult> ChitietHoso(int hosoId)
        {
            var result = await _bizHoso.GetById(hosoId);
            ViewBag.hoso = result;
            return View();
        }
        [Authorize]
        [HttpGet("tailieu/{hosoId}")]
        public async Task<IActionResult> GetTailieu(int hosoId)
        {
            var result = await _bizTailieu.GetTailieuByHosoId(hosoId);
            return ToResponse(result);
        }
        [Authorize]
        [HttpGet("DuyetHoso/{hosoId}")]
        public async Task<IActionResult> DuyetHoso(int hosoId)
        {
            var result = await _bizHoso.GetById(hosoId);
            ViewBag.hoso = result;
            return View();
        }
        [Authorize]
        [HttpGet("comments/{hosoId}")]
        public async Task<IActionResult> GetComment(int hosoId)
        {
            var result = await _bizHoso.GetComments(hosoId);
            return ToResponse(result);
        }
        [Authorize]
        [HttpGet("statuses")]
        public async Task<IActionResult> GetStatusList()
        {
            var result = await _bizHoso.GetStatusList();
            return ToResponse(result);
        }
        [Authorize]
        [HttpGet("results")]
        public async Task<IActionResult> GetResultList()
        {
            var result = await _bizHoso.GetResultList();
            return ToResponse(result);
        }
    }
}
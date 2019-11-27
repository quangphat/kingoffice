using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using KingOffice.Models;
using Repository.Classes;
using Repository.Interfaces;
using KingOffice.Infrastructures;
using Entity.Infrastructures;
using Microsoft.AspNetCore.Authorization;
using Entity.ViewModels;

namespace KingOffice.Controllers
{

    
    public class HomeController : BaseController
    {
        protected readonly IHosoRepository _rpHoso;
        protected readonly F88ServiceApi.F88Service _f88;
        public HomeController(IHosoRepository hosoRepository, 
            F88ServiceApi.F88Service f88,
            CurrentProcess process) : base(process)
        {
            _rpHoso = hosoRepository;
            _f88 = f88;
        }
        public static Dictionary<string, ActionInfo> LstRole
        {
            get
            {
                Dictionary<string, ActionInfo> _lstRole = new Dictionary<string, ActionInfo>();
                _lstRole.Add("Index", new ActionInfo() { _formindex = IndexMenu.M_0_1, _href = "Home/Index", _mangChucNang = new int[] { (int)QuyenIndex.Public } });
                _lstRole.Add("HuongDanSuDung", new ActionInfo() { _formindex = IndexMenu.M_0_2, _href = "Home/HuongDanSuDung", _mangChucNang = new int[] { (int)QuyenIndex.Public } });
                _lstRole.Add("PhienBan", new ActionInfo() { _formindex = IndexMenu.M_0_3, _href = "Home/PhienBan", _mangChucNang = new int[] { (int)QuyenIndex.Public } });
                return _lstRole;
            }

        }
        [HttpPost("home/f88")]
        public async Task<IActionResult> TestF88()
        {
            _f88.LadipageReturnID(new Entity.F88.LadipageModel());
            return ToResponse(true);
        }
        [Authorize]
        public async Task<IActionResult> Index()
        {
            //var account = new AccountJson(_process.User);
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [Authorize]
        public ActionResult PhienBan()
        {
            ViewBag.formindex = LstRole[RouteData.Values["action"].ToString()]._formindex;
            return View();
        }

    }
}

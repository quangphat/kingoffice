using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Interfaces;
using Entity.Infrastructures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KingOffice.Controllers
{
    public class TeamController : BaseController
    {
        public TeamController(CurrentProcess process) : base(process)
        {
            
        }
        public IActionResult Taomoi()
        {
            return View();
        }

    }
}
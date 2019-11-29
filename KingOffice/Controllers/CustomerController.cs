using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Interfaces;
using Entity.Infrastructures;
using Entity.PostModel;
using KingOffice.Infrastructures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KingOffice.Controllers
{
    [Route("Customers")]
    public class CustomerController : BaseController
    {
        protected readonly ICustomerBusiness _bizCustomer;
        public CustomerController(CurrentProcess process,ICustomerBusiness customerBusiness) : base(process)
        {
            _bizCustomer = customerBusiness;
        }
        [Authorize]
        [HttpGet("Index")]
        public IActionResult Index()
        {
            return View();
        }
        [Authorize]
        [HttpGet("Search")]
        public async Task<IActionResult> Search(string freeText = null, int page = 1, int limit = 10)
        {
            var result = await _bizCustomer.Gets(freeText, page, limit);
            return ToResponse(DataPaging.Create(result.datas,result.totalRecord));
        }
        [Authorize]
        [HttpGet("AddNew")]
        public ActionResult AddNew()
        {
            ViewBag.MaNV = _process.User.Id;
            return View();
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody]CustomerModel model)
        {
            var result = await _bizCustomer.Create(model);
            return ToResponse(result);

        }
        [Authorize]
        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var customer = await _bizCustomer.GetById(id);
            ViewBag.customer = customer;
            return View();
        }
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] CustomerEditModel model)
        {
            var result = await _bizCustomer.Update(model);
            return ToResponse(result);
        }
        [Authorize]
        [HttpGet("partner/{id}")]
        public async Task<IActionResult> GetPartner(int id)
        {
            var result = await _bizCustomer.GetPartner(id);
            return ToResponse(result);
        }
        [Authorize]
        [HttpGet("notes/{id}")]
        public async Task<IActionResult> GetNotes(int id)
        {
            var datas = await _bizCustomer.GetNoteByCustomerId(id);
            return ToResponse(datas);
        }
    }
}
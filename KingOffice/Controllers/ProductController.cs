using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Interfaces;
using Entity.Infrastructures;
using Entity.PostModel;
using Entity.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KingOffice.Controllers
{
    public class ProductController : BaseController
    {
        protected readonly IProductBusiness _bizProduct;
        public ProductController(CurrentProcess process, IProductBusiness productBusiness) : base(process)
        {
            _bizProduct = productBusiness;
        }

        [Authorize]
        [HttpGet("products/{doitacId}")]
        public async Task<IActionResult> LayDSSanPham(int doitacId)
        {
            var result = await _bizProduct.GetListByDoitacId(doitacId);
            return ToResponse(result);
        }
        [Authorize]
        public async Task<IActionResult> Import()
        {
            return View();
        }
        [Authorize]
        [HttpPost("product/import")]
        public async Task<IActionResult> UploadFile([FromBody] ProductImport model)
        {
            var result = await _bizProduct.Import(model);
            return ToResponse(result);
        }
        [Authorize]
        [HttpPost("product/saveimport")]
        public async Task<IActionResult> Save([FromBody] ImportSaveModel model)
        {
            var result = await _bizProduct.SaveImport(model);
            return ToResponse(result);
        }
    }
}
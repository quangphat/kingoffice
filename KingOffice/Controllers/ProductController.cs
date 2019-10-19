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
    [Route("products")]
    public class ProductController : BaseController
    {
        protected readonly IProductBusiness _bizProduct;
        public ProductController(CurrentProcess process, IProductBusiness productBusiness) : base(process)
        {
            _bizProduct = productBusiness;
        }

        [Authorize]
        [HttpGet("{doitacId}")]
        public async Task<IActionResult> LayDSSanPham(int doitacId)
        {
            var result = await _bizProduct.GetListByDoitacId(doitacId);
            return ToResponse(result);
        }
    }
}
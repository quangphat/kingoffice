using AutoMapper;
using Business.Interfaces;
using Entity;
using Entity.DatabaseModels;
using Entity.Infrastructures;
using Entity.PostModel;
using Entity.ViewModels;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Classes
{
    public class ProductBusiness : BaseBusiness, IProductBusiness
    {
        IProductRepository _rpProduct;

        public ProductBusiness(CurrentProcess process,
            IProductRepository productRepository, IMapper mapper) : base(mapper, process)
        {
            _rpProduct = productRepository;
        }
        public async Task<List<OptionSimple>> GetListByDoitacId(int doitacId)
        {
            if (doitacId <= 0)
            {
                AddError(errors.invalid_id);
                return null;
            }
            var datas = await _rpProduct.GetListByDoitacId(doitacId);
            return _mapper.Map<List<OptionSimple>>(datas);
        }
        public async Task<ProductImportResponse> Import(ProductImport model)
        {
            if (model == null || model.param == null)
            {
                AddError(errors.invalid_data);
                return null;
            }
            var displayedCompanies = model.lstSanPham
                               .Skip(model.param.iDisplayStart)
                               .Take(model.param.iDisplayLength);
            var result = new ProductImportResponse
            {
                aaData = displayedCompanies.ToList(),
                sEcho = model.param.sEcho,
                iTotalDisplayRecords = model.lstSanPham.Count(),
                iTotalRecords = model.lstSanPham.Count()
            };
            return result;
        }
        public async Task<List<ProductViewModel>> SaveImport(ImportSaveModel model)
        {
            if (model == null || model.lstSanPham == null || !model.lstSanPham.Any() || model.ImportDate ==null)
            {
                AddError(errors.invalid_data);
                return null;
            }
            var existings = await _rpProduct.GetAllList();
            foreach(var item in model.lstSanPham)
            {
                var exist = existings.Where(p => (!string.IsNullOrWhiteSpace(p.Code) && p.Code.ToLower().Trim() == item.Ten.ToLower().Trim())).FirstOrDefault();
                if(exist!=null)
                {
                    item.TrangThai = 2;
                }
                else
                {
                    Product sp = new Product {
                        Code = item.Ten,
                        Name = item.Ten,
                        CreatedBy = _process.User.Id,
                        CreatedDate = model.ImportDate.Value,
                        PartnerId = 3,
                        Type = 1
                    };
                    var id = await _rpProduct.Insert(sp);
                    item.TrangThai = id > 0 ? 1 : 3;
                }
            }
            return model.lstSanPham;
        }
    }
}

using Business.Interfaces;
using Entity;
using Entity.Infrastructures;
using Entity.ViewModels;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business.Classes
{
    public class ProductBusiness : BaseBusiness, IProductBusiness
    {
        IProductRepository _rpProduct;
        public ProductBusiness(CurrentProcess process,
            IProductRepository productRepository) : base(null, process)
        {
            _rpProduct = productRepository;
        }
        public async Task<List<ProductViewModel>> GetListByDoitacId(int doitacId)
        {
            if (doitacId <= 0)
            {
                AddError(errors.invalid_id);
                return null;
            }
            return await _rpProduct.GetListByDoitacId(doitacId);
        }
        public async Task<List<ProductViewModel>> GetListByHosoId(int doitacId, int hosoId)
        {
            if (doitacId <= 0 || hosoId <= 0)
            {
                AddError(errors.invalid_id);
                return null;
            }
            return await _rpProduct.GetListByHosoId(doitacId, hosoId);
        }
    }
}

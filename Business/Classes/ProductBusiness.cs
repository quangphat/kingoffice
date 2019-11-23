using AutoMapper;
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
        
    }
}

using Entity.PostModel;
using Entity.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IProductBusiness
    {
        Task<List<ProductViewModel>> SaveImport(ImportSaveModel model);
        Task<ProductImportResponse> Import(ProductImport model);
        Task<List<OptionSimple>> GetListByDoitacId(int doitacId);
        
    }
}

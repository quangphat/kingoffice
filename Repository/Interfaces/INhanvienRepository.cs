using Entity.DatanbaseModels;
using Entity.ViewModels;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface INhanvienRepository
    {
        Task<int> Count(
            DateTime fromDate,
            DateTime toDate,
            int dateFilterType,
            string freeText);
        Task<List<Nhanvien>> Gets(
            DateTime fromDate,
            DateTime toDate,
            int dateFilterType,
            string freeText,
            int offset,
            int limit);
        Task<Nhanvien> GetByUserName(string userName, int id = 0);
        Task<int> Create(Nhanvien entity);
        Task<List<OptionSimpleModel>> GetListCourierSimple();
        Task<List<OptionSimpleModelV2>> GetListByUserId(int userId);
    }
}


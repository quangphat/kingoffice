using Business.Interfaces;
using Entity.Infrastructures;
using Entity.ViewModels;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business.Classes
{
    public class NhanvienBusiness : BaseBusiness,INhanvienBusiness
    {
        INhanvienRepository _rpNhanvien;
        public NhanvienBusiness(CurrentProcess process, INhanvienRepository nhanvienRepository):base(null,process)
        {
            _rpNhanvien = nhanvienRepository;
        }
        public async Task<List<OptionSimpleModel>> GetCourierList()
        {
            return await _rpNhanvien.GetListCourierSimple();
        }
        public async Task<List<OptionSimpleModelV2>> GetListByUserId(int userId)
        {
            return await _rpNhanvien.GetListByUserId(userId);
        }
    }
}

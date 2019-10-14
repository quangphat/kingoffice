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
    public class LoaiTailieuBusiness :BaseBusiness, ILoaiTailieuBusiness
    {
        protected readonly ILoaiTailieuRepository _rpLoaiTailieu;
        public LoaiTailieuBusiness(CurrentProcess process, ILoaiTailieuRepository loaiTailieuRepository):base(null,process)
        {
            _rpLoaiTailieu = loaiTailieuRepository;
        }
        public async Task<List<LoaiTaiLieuModel>> GetList()
        {
            return await _rpLoaiTailieu.GetList();
        }
    }
}

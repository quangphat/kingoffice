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
    public class TailieuBusiness :BaseBusiness, ITailieuBusiness
    {
        protected readonly ITailieuRepository _rpTailieu;
        public TailieuBusiness(CurrentProcess process, ITailieuRepository loaiTailieuRepository):base(null,process)
        {
            _rpTailieu = loaiTailieuRepository;
        }
        public async Task<List<LoaiTaiLieuModel>> GetList()
        {
            return await _rpTailieu.GetLoaiTailieuList();
        }
        public async Task<List<FileUploadModel>> GetTailieuByHosoId(int hosoId)
        {
            return await _rpTailieu.GetTailieuByHosoId(hosoId);
        }
    }
}

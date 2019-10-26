using AutoMapper;
using Business.Infrastructures;
using Business.Interfaces;
using Entity;
using Entity.DatanbaseModels;
using Entity.Infrastructures;
using Entity.ViewModels;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business.Classes
{
    public class NhanvienBusiness : BaseBusiness, INhanvienBusiness
    {
        INhanvienRepository _rpNhanvien;
        IMapper _mapper;
        public NhanvienBusiness(CurrentProcess process, INhanvienRepository nhanvienRepository, IMapper mapper) : base(mapper, process)
        {
            _rpNhanvien = nhanvienRepository;
        }
        public async Task<int> Create(UserModel entity)
        {
            if (entity == null)
            {
                AddError(errors.invalid_data);
                return 0;
            }
            if(string.IsNullOrWhiteSpace(entity.UserName))
            {
                AddError(errors.username_or_password_must_not_be_empty);
                return 0;
            }
            if (string.IsNullOrWhiteSpace(entity.Password))
            {
                AddError(errors.username_or_password_must_not_be_empty);
                return 0;
            }
            if (string.IsNullOrWhiteSpace(entity.PasswordConfirm))
            {
                AddError(errors.password_not_match);
                return 0;
            }
            if (entity.Password != entity.PasswordConfirm)
            {
                AddError(errors.password_not_match);
                return 0;
            }
            if (string.IsNullOrWhiteSpace(entity.Email))
            {
                AddError(errors.email_cannot_be_null);
                return 0;
            }
            if (!BusinessExtension.IsValidEmail(entity.Email,50))
            {
                AddError(errors.invalid_email);
                return 0;
            }
            var user = _mapper.Map<Nhanvien>(entity);
            user.Mat_Khau = Utils.getMD5(entity.Password);
            var result = await _rpNhanvien.Create(user);
            return result;
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

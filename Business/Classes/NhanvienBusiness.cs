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
        public NhanvienBusiness(CurrentProcess process, INhanvienRepository nhanvienRepository, IMapper mapper) : base(mapper, process)
        {
            _rpNhanvien = nhanvienRepository;
        }
        public async Task<bool> Update(EmployeeEditModel model)
        {
            if (model == null || model.Id <= 0)
            {
                AddError(errors.invalid_data);
                return false;
            }
            var user = _mapper.Map<Nhanvien>(model);
            user.UpdatedBy = _process.User.Id;
            return await _rpNhanvien.Update(user);
        }
        public async Task<EmployeeEditModel> GetById(int id)
        {
            if (id <= 0)
            {
                AddError(errors.invalid_data);
                return null;
            }
            var user = await _rpNhanvien.GetById(id);
            if (user == null)
            {
                AddError(errors.not_found_user);
                return null;
            }
            var result = _mapper.Map<EmployeeEditModel>(user);
            return result;
        }
        public async Task<(int totalRecord, List<EmployeeViewModel> datas)> Gets(DateTime? fromDate, DateTime? toDate,
            string freetext = "",
            int roleId = 0,
            int page = 1, int limit = 10)
        {
            if (!string.IsNullOrWhiteSpace(freetext) && freetext.Length > 30)
            {
                AddError(errors.freetext_length_cannot_lagger_30);
                return (0, null);
            }
            var fDate = fromDate == null ? DateTime.Now : fromDate.Value;
            var tDate = toDate == null ? DateTime.Now : toDate.Value;
            BusinessExtension.ProcessPaging(page, ref limit);
            freetext = string.IsNullOrWhiteSpace(freetext) ? string.Empty : freetext.Trim();
            var totalRecord = await _rpNhanvien.Count(fDate, tDate, roleId, freetext);
            if (totalRecord == 0)
            {
                return (0, null);
            }
            var nhanviens = await _rpNhanvien.Gets(fDate, tDate, roleId, freetext, page, limit);

            return (totalRecord, nhanviens);
        }
        public async Task<int> Create(UserModel entity)
        {
            if (entity == null)
            {
                AddError(errors.invalid_data);
                return 0;
            }
            if (string.IsNullOrWhiteSpace(entity.UserName))
            {
                AddError(errors.username_or_password_must_not_be_empty);
                return 0;
            }
            if (string.IsNullOrWhiteSpace(entity.Password))
            {
                AddError(errors.username_or_password_must_not_be_empty);
                return 0;
            }
            if (entity.Password.Trim().Length < Constanst.PasswordMinLengthRequire)
            {
                AddError(errors.password_not_match_min_length);
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
            if (!BusinessExtension.IsValidEmail(entity.Email, 50))
            {
                AddError(errors.invalid_email);
                return 0;
            }
            if (entity.ProvinceId <= 0)
            {
                AddError(errors.missing_province);
                return 0;
            }
            if (entity.DistrictId <= 0)
            {
                AddError(errors.missing_district);
                return 0;
            }
            var existUserName = await _rpNhanvien.GetByUserName(entity.UserName.Trim(), 0);
            if (existUserName != null)
            {
                AddError(errors.username_has_exist);
                return 0;
            }
            entity.UserName = entity.UserName.Trim();
            entity.Password = entity.Password.Trim();
            var user = _mapper.Map<Nhanvien>(entity);
            user.CreatedBy = _process.User.Id;
            user.Mat_Khau = Utils.getMD5(entity.Password);
            var result = await _rpNhanvien.Create(user);
            return result;
        }
        
        public async Task<List<OptionSimpleModelOld>> GetCourierList()
        {
            return await _rpNhanvien.GetListCourierSimple();
        }
        public async Task<List<OptionSimpleModelV2>> GetListByUserId(int userId)
        {
            return await _rpNhanvien.GetListByUserId(userId);
        }
    }
}

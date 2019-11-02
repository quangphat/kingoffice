using AutoMapper;
using Entity.DatabaseModels;
using Entity.DatanbaseModels;
using Entity.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Infrastructures
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Add as many of these lines as you need to map your objects
            CreateMap<Nhanvien, Account>()
                .ForMember(a => a.Id, b => b.MapFrom(c => c.ID))
                .ForMember(a => a.UserName, b => b.MapFrom(c => c.Ten_Dang_Nhap))
                .ForMember(a => a.Code, b => b.MapFrom(c => c.Ma))
                .ForMember(a => a.Email, b => b.MapFrom(c => c.Email))
                .ForMember(a => a.FullName, b => b.MapFrom(c => c.Ho_Ten));
            CreateMap<UserModel, Nhanvien>()              
                .ForMember(a => a.Ten_Dang_Nhap, b => b.MapFrom(c => c.UserName))
                .ForMember(a => a.Dien_Thoai, b => b.MapFrom(c => c.Phone))
                .ForMember(a => a.Email, b => b.MapFrom(c => c.Email))
                .ForMember(a => a.Ho_Ten, b => b.MapFrom(c => c.FullName));
            CreateMap<EmployeeEditModel, Nhanvien>()
                .ForMember(a => a.Dien_Thoai, b => b.MapFrom(c => c.Phone))
                .ForMember(a => a.Email, b => b.MapFrom(c => c.Email))
                .ForMember(a => a.Ten_Dang_Nhap, b => b.MapFrom(c => c.UserName))
                .ForMember(a => a.Ho_Ten, b => b.MapFrom(c => c.FullName));
            CreateMap<Nhanvien, EmployeeEditModel>()
               .ForMember(a => a.Phone, b => b.MapFrom(c => c.Dien_Thoai))
               .ForMember(a => a.UserName, b => b.MapFrom(c => c.Ten_Dang_Nhap))
               .ForMember(a => a.Email, b => b.MapFrom(c => c.Email))
               .ForMember(a => a.FullName, b => b.MapFrom(c => c.Ho_Ten));
            CreateMap<HosoRequestModel, HosoModel>()
                .ForMember(d => d.NgayNhanDon, map => map.MapFrom((s, d) => {
                    return BusinessExtension.ConvertddMMyyyyToDateTime(s.ngaynhandon);
                }));
        }
    }
}

using AutoMapper;
using BL.Dtos;
using DAL;
using DAL.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Configurations
{
    class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<DAL.ApplicationUsersIdentity, LoginDto>().ReverseMap();
            CreateMap<ApplicationUsersIdentity, RegisterDto>().ReverseMap();
            CreateMap<IdentityRole, RoleDto>().ReverseMap();
            CreateMap<EmployeeLogs, EmployeeLogsDto>().ReverseMap();
            CreateMap<Setting, SettingDto>().ReverseMap();

        }
    }
}

using AutoMapper;
using BL.Bases;
using BL.Dtos;
using BL.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.AppServices
{
    public class RoleAppService : AppServiceBase
    {
        public RoleAppService(IUnitOfWork theUnitOfWork, IMapper mapper) : base(theUnitOfWork, mapper)
        {

        }
        public async Task CreateRoles()
        {
            await TheUnitOfWork.role.CreateRoles();
        }
        public RoleDto GetRoleById(string id)
        {
            if (id == null || id == "")
                throw new ArgumentNullException();

            return Mapper.Map<RoleDto>(TheUnitOfWork.role.GetRoleByID(id));
        }
        public IdentityResult Create(string rolename)
        {
            return TheUnitOfWork.role.Create(rolename);
        }
        public async Task<IdentityResult> Update(RoleDto RoleDto)
        {
            if (RoleDto == null)
                throw new ArgumentNullException();
            if (RoleDto.Id == null || RoleDto.Id == string.Empty)
                throw new ArgumentException();

            var role = Mapper.Map<IdentityRole>(RoleDto);
            return await TheUnitOfWork.role.UpdateRole(role);
        }

        public bool DeleteRole(string id)
        {
            if (id == null || id == "")
                throw new ArgumentNullException();
            bool result = false;

            TheUnitOfWork.role.DeleteRole(id);
            result = TheUnitOfWork.Commit() > new int();

            return result;
        }      
    }
}

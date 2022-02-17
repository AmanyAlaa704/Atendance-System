using BL.Bases;
using BL.Dtos;
using DAL;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Repositories
{
    public class AccountRepository : BaseRepository<ApplicationUsersIdentity>
    {
        private readonly UserManager<ApplicationUsersIdentity> manager;
        private readonly RoleManager<IdentityRole> roleManager;

        public AccountRepository(DbContext db, UserManager<ApplicationUsersIdentity> manager, RoleManager<IdentityRole> roleManager) : base(db)
        {
            this.manager = manager;
            this.roleManager = roleManager;

        }

        public ApplicationUsersIdentity GetAccountById(string id)
        {
            return GetFirstOrDefault(l => l.Id == id);
        }

        public async Task<bool> UpdateUserInfo(string id, RegisterDto accountInfo)
        {
            ApplicationUsersIdentity user = GetFirstOrDefault(l => l.Id == id);
            if (user != null)
            {
                user.UserName = accountInfo.UserName;           

                IdentityResult result = await manager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return true;
                }
            }

            return false;
        }


        public List<ApplicationUsersIdentity> GetAllAccounts()
        {
            return GetAll().ToList();
        }
        public async Task<ApplicationUsersIdentity> FindByName(string userName)
        {
            ApplicationUsersIdentity result = await manager.FindByNameAsync(userName);
            return result;
        }
        public async Task<IEnumerable<string>> GetUserRoles(ApplicationUsersIdentity user)
        {
            var userRoles = await manager.GetRolesAsync(user);
            return userRoles;
        }

        public async Task<ApplicationUsersIdentity> FindById(string id)
        {
            ApplicationUsersIdentity result = await manager.FindByIdAsync(id);

            return result;

        }
        public async Task<ApplicationUsersIdentity> Find(string username, string password)
        {

            var user = await manager.FindByNameAsync(username);
            if (user != null && await manager.CheckPasswordAsync(user, password))
            {
                return user;
            }

            return null;
        }

        public async Task<IdentityResult> Register(ApplicationUsersIdentity user)
        {
            user.Id = Guid.NewGuid().ToString();
            IdentityResult result;
            result = await manager.CreateAsync(user, user.PasswordHash);

            return result;
        }
        public async Task<IdentityResult> AssignToRole(string userid, string rolename)
        {
            var user = await manager.FindByIdAsync(userid);
            if (user != null && await roleManager.RoleExistsAsync(rolename))
            {
                IdentityResult result = await manager.AddToRoleAsync(user, rolename);
                return result;
            }
            return null;
        }
        public async Task<bool> updatePassword(string stid, RegisterDto accountInfo, string oldPassword)
        {
            ApplicationUsersIdentity user = GetFirstOrDefault(l => l.Id == stid);
            var newPasswordHash = manager.PasswordHasher.HashPassword(user, accountInfo.Password);

            if (await manager.CheckPasswordAsync(user, oldPassword))
            {
                user.PasswordHash = newPasswordHash;
                IdentityResult result = await manager.UpdateAsync(user);
                if (result != null && result.Succeeded)
                {
                    return true;
                }
            }

            return false;

        }

        public async Task<bool> UpdateAccount(ApplicationUsersIdentity user)
        {
            IdentityResult result = await manager.UpdateAsync(user);
            return true;
        }

        public bool CheckAccountExistsByData(ApplicationUsersIdentity user)
        {
            return GetAny(std => std.UserName == user.UserName && std.Email == user.Email);
        }
    }
}


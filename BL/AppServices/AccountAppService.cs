using AutoMapper;
using BL.Bases;
using BL.Dtos;
using BL.Interfaces;
using BL.StaticClasses;
using DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BL.AppServices
{
    public class AccountAppService : AppServiceBase
    {
        IConfiguration _configuration;
        IHttpContextAccessor _httpContextAccessor;


        public AccountAppService(IUnitOfWork theUnitOfWork, IConfiguration configuration,
             IMapper mapper) : base(theUnitOfWork, mapper)
        {
            this._configuration = configuration;

        }

        public List<RegisterDto> GetAllAccounts()
        {
            return Mapper.Map<List<RegisterDto>>(TheUnitOfWork.account.GetAllAccounts());
        }
        public RegisterDto GetAccountById(string id)
        {
            if (id == null)
                throw new ArgumentNullException();
            return Mapper.Map<RegisterDto>(TheUnitOfWork.account.GetAccountById(id));

        }
        public async Task<ApplicationUsersIdentity> Find(string Email, string password)
        {
            ApplicationUsersIdentity user = await TheUnitOfWork.account.Find(Email, password);

            if (user != null)
                return user;
            return null;
        }

        public async Task<ApplicationUsersIdentity> FindByName(string userName)
        {
            ApplicationUsersIdentity user = await TheUnitOfWork.account.FindByName(userName);

            if (user != null)
                return user;
            return null;
        }

        public async Task<IdentityResult> Register(RegisterDto user)
        {
            bool isExist = await checkUserNameExist(user.UserName);
            if (isExist)
                return IdentityResult.Failed(new IdentityError
                { Code = "error", Description = "user name already exist" });

            ApplicationUsersIdentity identityUser = Mapper.Map<RegisterDto, ApplicationUsersIdentity>(user);
            var result = await TheUnitOfWork.account.Register(identityUser);
            if (result.Succeeded)
            {
            }
            return result;
        }

        public async Task<IdentityResult> AssignToRole(string userid, string rolename)
        {
            if (userid == null || rolename == null)
                return null;
            return await TheUnitOfWork.account.AssignToRole(userid, rolename);
        }

        public async Task<bool> UpdatePassword(string userID, RegisterDto accountInfo, string oldPassword)
        {
            return await TheUnitOfWork.account.updatePassword(userID, accountInfo, oldPassword);

        }

        public async Task<bool> Update(RegisterDto user)
        {
            ApplicationUsersIdentity identityUser = await TheUnitOfWork.account.FindById(user.Id);
            var oldPassword = identityUser.PasswordHash;
            Mapper.Map(user, identityUser);
            identityUser.PasswordHash = oldPassword;
            return await TheUnitOfWork.account.UpdateAccount(identityUser);

        }

        public async Task<bool> checkUserNameExist(string userName)
        {
            var user = await TheUnitOfWork.account.FindByName(userName);
            return user == null ? false : true;
        }

        public async Task<IEnumerable<string>> GetUserRoles(ApplicationUsersIdentity user)
        {
            return await TheUnitOfWork.account.GetUserRoles(user);
        }
        public async Task<List<ApplicationUsersIdentity>> UsersInRole(string rolename)
        {
            return await TheUnitOfWork.account.UsersInRole(rolename);

        }

        public async Task<dynamic> CreateToken(ApplicationUsersIdentity user)
        {
            var userRoles = await GetUserRoles(user);

            var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim("role",userRoles.FirstOrDefault()),
                   new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
            };


        }

        public string getUserID()
        {

            return _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
        public async Task CreateFirstAdmin()
        {
            var firstAdmin = new RegisterDto()
            {
                Id = null,
                UserName = "Admin",
                Email = "Admin@gmail.com",
                PasswordHash = "@Admin12345",

            };
            Register(firstAdmin).Wait();
            ApplicationUsersIdentity foundedAdmin = await FindByName(firstAdmin.UserName);
            if (foundedAdmin != null)
                AssignToRole(foundedAdmin.Id, UserRoles.Admin).Wait();
        }

        public bool CheckAccountExistsByData(RegisterDto accountInfo)
        {
            ApplicationUsersIdentity std = Mapper.Map<ApplicationUsersIdentity>(accountInfo);
            if (std == null)
            {
                return false;
            }
            else
            {
                return TheUnitOfWork.account.CheckAccountExistsByData(std);
            }
        }


        public async Task<string> getRoleName(string UserID)
        {
            return await TheUnitOfWork.account.getRoleName(UserID);
        }
    }
}

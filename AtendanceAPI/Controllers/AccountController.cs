using AtendanceAPI.HelpClasses;
using BL.AppServices;
using BL.Dtos;
using BL.StaticClasses;
using DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.HttpSys;
using Microsoft.PowerBI.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AtendanceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private AccountAppService _accountAppService;
        private RoleAppService _roleAppService;
        private DailyLogsAppServices _dailyLogsAppServices;
        IHttpContextAccessor _httpContextAccessor;

        public AccountController(AccountAppService accountAppService,RoleAppService roleAppService,IHttpContextAccessor httpContextAccessor, DailyLogsAppServices DailyLogsAppServices)
        {
            this._accountAppService = accountAppService;
            this._roleAppService = roleAppService;
            this._httpContextAccessor = httpContextAccessor;
            this._dailyLogsAppServices = DailyLogsAppServices;
        }

        [HttpGet]
        public IActionResult GetAll()
        {        
            return Ok(_accountAppService.GetAllAccounts());
        }
        [HttpGet("{id}")]
        public IActionResult GetUserById(string id)
        {            
            return Ok(_accountAppService.GetAccountById(id));
        }
        [HttpGet("UsersInRole/{rolename}")]
        public async Task<List<ApplicationUsersIdentity>> UsersInRole(string rolename)
        {             
            return await this._accountAppService.UsersInRole(rolename);
        }

        [HttpGet("current")]
        public IActionResult GetCurrentUser()
        {
            var userID = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;            
            return Ok(_accountAppService.GetAccountById(userID));
        }
        [HttpPost("/RegisterAdmin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterDto model)
        {
            return await Register(model, UserRoles.Admin);

        }

        [HttpPost("/RegisterEmp")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterDto model)
        {
            return await Register(model, UserRoles.Employee);
        }

        private async Task<IActionResult> Register(RegisterDto model, string roleName)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!_accountAppService.CheckAccountExistsByData(model))
            {
                var result = await _accountAppService.Register(model);

                if (!result.Succeeded)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                          new Response { Status = "Error", Message = result.Errors.FirstOrDefault().Description });
                }
                else
                {
                    ApplicationUsersIdentity identityUser = await _accountAppService.Find(model.Email, model.PasswordHash);
                    await _accountAppService.AssignToRole(identityUser.Id, roleName);
                    return Ok(new Response { Status = "Success", Message = "User created successfully!" });
                }
            }
            else
            {
                return BadRequest();
            }
        }    
        [HttpPost("/Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            var user = await _accountAppService.Find(model.Email, model.PasswordHash);
            if (user != null)
            {
                string roleName = await _accountAppService.getRoleName(user.Id);
                dynamic token = await _accountAppService.CreateToken(user);
                _dailyLogsAppServices.SaveNewSetting(new EmployeeLogsDto() { UserID = user.Id, dateTime = DateTime.Now, LogInOrOut = LoggingInOrOut.LogIn, EmpRole = roleName });

                var ResponseAndRole = new { token = token, roleName = roleName,User= user.Id };
                return Ok(ResponseAndRole);
            }
            return Unauthorized();
        }
        [HttpPost("/Logout/{UserId}")]
        public async Task<IActionResult> Logout(string UserId)
        {                     
                string roleName = await _accountAppService.getRoleName(UserId);
                _dailyLogsAppServices.SaveNewSetting(new EmployeeLogsDto() { UserID = UserId, dateTime = DateTime.Now, LogInOrOut = LoggingInOrOut.LogOut, EmpRole = roleName });           
            return Ok();
        }



    }
}

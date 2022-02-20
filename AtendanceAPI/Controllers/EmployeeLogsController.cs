using BL.AppServices;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AtendanceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeLogsController : ControllerBase
    {
        DailyLogsAppServices _dailyLogsAppServices;
        SettingAppService _settingAppService;
        public EmployeeLogsController(DailyLogsAppServices dailyLogsAppServices,SettingAppService settingAppService)
        {
            this._dailyLogsAppServices = dailyLogsAppServices;
            this._settingAppService = settingAppService;

        }
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_dailyLogsAppServices.GetAllLogs());
        }

        [HttpGet("/GetUserLogs/{UserId}")]
        public IActionResult GetUserLogs(string UserId)
        {
            return Ok(_dailyLogsAppServices.GetUserLogsByID(UserId));
        }


        [HttpGet("/CountLate/{UserId}")]
        public IActionResult CountTimesOfLate(string UserId)
        {            
            return Ok(_dailyLogsAppServices.CountTimesOfLate(UserId, _settingAppService.getArriveTime().Time));
        }

        [HttpGet("/CountEarly/{UserId}")]
        public IActionResult CountTimesOfLeaveEarly(string UserId)
        {
            return Ok(_dailyLogsAppServices.CountTimesOfLeaveEarly(UserId, _settingAppService.getLeaveTime().Time));
        }

    }
}

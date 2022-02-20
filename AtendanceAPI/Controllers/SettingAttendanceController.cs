using BL.AppServices;
using BL.Dtos;
using BL.StaticClasses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AtendanceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingAttendanceController : ControllerBase
    {
        SettingAppService _settingappService;
        public SettingAttendanceController(SettingAppService settingappService)
        {
            this._settingappService = settingappService;
        }
        // GET: api/<SettingController>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(this._settingappService.GetAllSetting());
        }

        [HttpGet("{FeildName}")]
        public IActionResult Get(string FeildName)
        {
            return Ok(this._settingappService.GetSettingByFeildName(FeildName));
        }


        [HttpPost("/PostLeaveTime")]
        public IActionResult PostLeaveTime([FromBody] SettingDto setting)
        {
            if (ModelState.IsValid)
            {
                SettingDto settingExist =
                this._settingappService.CheckLeaveFeildsExists();
                if (settingExist != null)
                {
                    this._settingappService.DeleteSetting(settingExist.ID);
                    setting.FeildName = SettingFeildsNames.Leave;

                    return Ok(this._settingappService.SaveNewSetting(setting));

                }
                else
                {
                    setting.FeildName = SettingFeildsNames.Leave;
                    return Ok(this._settingappService.SaveNewSetting(setting));

                }
            }
            return BadRequest();

        }

  
        [HttpPost("/PostArrivalTime")]
        public IActionResult PostArrivalTime([FromBody] SettingDto setting)
        {
           
            if (ModelState.IsValid)
            {
                    SettingDto settingExist =
                    this._settingappService.CheckArriveFeildsExists();
                if (settingExist!=null)
                {
                        this._settingappService.DeleteSetting(settingExist.ID);
                        setting.FeildName = SettingFeildsNames.Arrive;

                        return Ok(this._settingappService.SaveNewSetting(setting));

                }
                else
                {
                        setting.FeildName = SettingFeildsNames.Arrive;
                        return Ok(this._settingappService.SaveNewSetting(setting));

                }
                }         
                return BadRequest();

        }
      
        [HttpGet("/getArriveTime")]
        public DateTime getArriveTime()
        {
            SettingDto settingDto = _settingappService.getArriveTime();          
            return settingDto.Time;
        }

        [HttpGet("/getLeaveTime")]
        public DateTime getLeaveTime()
        {
            SettingDto settingDto = _settingappService.getLeaveTime();
            return settingDto.Time;
        }


    }

}

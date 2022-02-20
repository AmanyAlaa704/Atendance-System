using AutoMapper;
using BL.Bases;
using BL.Dtos;
using BL.StaticClasses;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.AppServices
{
    public class SettingAppService : AppServiceBase
    {

        public SettingAppService(Interfaces.IUnitOfWork theUnitOfWork, IMapper mapper) : base(theUnitOfWork, mapper)
        {

        }
        public List<SettingDto> GetAllSetting()
        {

            return Mapper.Map<List<SettingDto>>(TheUnitOfWork.setting.GetAllSetting());
        }


        public SettingDto GetSettingByFeildName(string FeildName)
        {
            return Mapper.Map<SettingDto>(TheUnitOfWork.setting.GetWhere(fn=>fn.FeildName==FeildName).FirstOrDefault());
        }

        public SettingDto getArriveTime()
        {
            return Mapper.Map<SettingDto>(TheUnitOfWork.setting.GetWhere(fn => fn.FeildName == SettingFeildsNames.Arrive).FirstOrDefault());
        }

        public SettingDto getLeaveTime()
        {
            return Mapper.Map<SettingDto>(TheUnitOfWork.setting.GetWhere(fn => fn.FeildName == SettingFeildsNames.Leave).FirstOrDefault());
        }

      

        public bool SaveNewSetting(SettingDto settingModel)
        {
            if (settingModel == null)

                throw new ArgumentNullException();

            bool result = false;
            var setting = Mapper.Map<Setting>(settingModel);
            if (TheUnitOfWork.setting.InsertSetting(setting))
            {
                result = TheUnitOfWork.Commit() > new int();
            }
            return result;
        }


        public bool UpdateSetting(SettingDto settingModel)
        {
            var setting = Mapper.Map<Setting>(settingModel);
            TheUnitOfWork.setting.Update(setting);
            TheUnitOfWork.Commit();

            return true;
        }


        public bool DeleteSetting(int id)
        {
            bool result = false;

            TheUnitOfWork.setting.Delete(id);
            result = TheUnitOfWork.Commit() > new int();

            return result;
        }
        public SettingDto CheckArriveFeildsExists()
        {
            //return Mapper.Map<SettingDto>(TheUnitOfWork.setting.GetWhere(fn => fn.FeildName == SettingFeildsNames.Leave).FirstOrDefault());

            return Mapper.Map<SettingDto>(TheUnitOfWork.setting.CheckArriveFeildsExists());
        }

        public SettingDto CheckLeaveFeildsExists()
        {
            return Mapper.Map<SettingDto>(TheUnitOfWork.setting.CheckLeaveFeildsExists());
        }

        public string[] DateAndTime(DateTime dateTime)
        {
            string datetimestr = dateTime.ToString();
            string[] DateTime = datetimestr.Split(' ');
            return DateTime;
        }

    }
}

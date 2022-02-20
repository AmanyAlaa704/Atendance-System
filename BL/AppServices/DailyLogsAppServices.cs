using AutoMapper;
using BL.Bases;
using BL.Dtos;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.AppServices
{
    public class DailyLogsAppServices : AppServiceBase
    {

        public DailyLogsAppServices(Interfaces.IUnitOfWork theUnitOfWork, IMapper mapper) : base(theUnitOfWork, mapper)
        {

        }
        public List<EmployeeLogsDto> GetAllLogs()
        {
            return Mapper.Map<List<EmployeeLogsDto>>(TheUnitOfWork.dailyLogsRepository.GetAllLogs());
        }


        public List<EmployeeLogsDto> GetUserLogsByID(string UserId)
        {
            return Mapper.Map<List<EmployeeLogsDto>>(TheUnitOfWork.dailyLogsRepository.GetUserLogsByID(UserId));
        }

        public int CountTimesOfLeaveEarly(string UserID, DateTime Leave)
        {
            return TheUnitOfWork.dailyLogsRepository.CountTimesOfLeaveEarly(UserID, Leave);
        }
        public int CountTimesOfLate(string UserID, DateTime arrive)
        {
            return TheUnitOfWork.dailyLogsRepository.CountTimesOfLate(UserID, arrive);
        }

        public bool SaveNewSetting(EmployeeLogsDto DailyLogModel)
        {
            if (DailyLogModel == null)
                throw new ArgumentNullException();

            bool result = false;
            var dailyLogsRepository = Mapper.Map<EmployeeLogs>(DailyLogModel);
            if (TheUnitOfWork.dailyLogsRepository.InsertDailyLog(dailyLogsRepository))
            {
                result = TheUnitOfWork.Commit() > new int();
            }
            return result;
        }


    }
    }

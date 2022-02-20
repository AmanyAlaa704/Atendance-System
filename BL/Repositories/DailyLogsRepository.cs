using BL.Bases;
using BL.StaticClasses;
using DAL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Repositories
{
    public class DailyLogsRepository : BaseRepository<EmployeeLogs>
    {
        private DbContext AS_DbContext;

        public DailyLogsRepository(DbContext AS_DbContext) : base(AS_DbContext)
        {
            this.AS_DbContext = AS_DbContext;
        }

        public List<EmployeeLogs> GetUserLogsByID(string UserID)
        {
            return GetWhere(log => log.UserID == UserID).ToList();
        }
        public List<EmployeeLogs> GetAllLogs()
        {
            return GetAll().ToList();
        }

        public bool InsertDailyLog(EmployeeLogs employeeLogs)
        {
            if (ChecklectureExists(employeeLogs, employeeLogs.LogInOrOut))
            {
                return false;
            }
            else
            return Insert(employeeLogs);
        }        
        public void DeleteDailyLogs(int id)
        {
            Delete(id);
        }

        public bool ChecklectureExists(EmployeeLogs employeeLogs,string LogInOrOut)
        {
            string[] DateTimeUser = DateAndTime(employeeLogs.dateTime);
            string[] DateTimeNow = DateAndTime(DateTime.Now);
            if (DateTimeUser[0] == DateTimeNow[0])
            {
                return GetAny(logs => logs.LogInOrOut == LogInOrOut && logs.dateTime.Day==employeeLogs.dateTime.Day &&logs.UserID==employeeLogs.UserID);
            }
            return false;

        }

        public string[] DateAndTime(DateTime dateTime)
        {
            string datetimestr = dateTime.ToString();
            string[] DateTime = datetimestr.Split(' ');
            return DateTime;
        } 
        

        public  int CountTimesOfLate(string UserID,DateTime arrive)
        {
            int Counter = 0;
             foreach(var log in GetUserLogsByID(UserID))
            {
                if (log.dateTime.CompareTo(arrive)<0 &&log.LogInOrOut== LoggingInOrOut.LogIn)
                {
                    Counter++;
                }
            }
            return Counter;            
        }

        public int CountTimesOfLeaveEarly(string UserID, DateTime Leave)
        {
            int Counter = 0;
            foreach (var log in GetUserLogsByID(UserID))
            {
                if (log.dateTime.CompareTo(Leave) < 0 && log.LogInOrOut == LoggingInOrOut.LogOut)
                {
                    Counter++;
                }
            }
            return Counter;
        }
    }
}

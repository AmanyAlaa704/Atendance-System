using BL.Interfaces;
using BL.Repositories;
using DAL;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Bases
{
    public class UnitOfWork : IUnitOfWork
    {

        private DbContext AS_DbContext { get; set; }
        private UserManager<ApplicationUsersIdentity> _userManager;
        private RoleManager<IdentityRole> _roleManager;

        public AccountRepository Account;
        public AccountRepository account
        {
            get
            {
                if (Account == null)
                    Account = new AccountRepository(AS_DbContext, _userManager, _roleManager);
                return Account;
            }
        }
        public RoleRepository Role;
        public RoleRepository role
        {
            get
            {
                if (Role == null)
                    Role = new RoleRepository(AS_DbContext, _roleManager);
                return Role;
            }
        }

        public SettingRepository Setting;
        public SettingRepository setting
        {
            get
            {
                if (Setting == null)
                    Setting = new SettingRepository(AS_DbContext);
                return Setting;
            }
        }



        public DailyLogsRepository DailyLogsRepository;
        public DailyLogsRepository dailyLogsRepository
        {
            get
            {
                if (DailyLogsRepository == null)
                    DailyLogsRepository = new DailyLogsRepository(AS_DbContext);
                return DailyLogsRepository;
            }
        }

        public UnitOfWork(ApplicationDBContext AS_DbContext, UserManager<ApplicationUsersIdentity> userManager, RoleManager<IdentityRole> roleManager)
        {
            this._userManager = userManager;
            this._roleManager = roleManager;
            this.AS_DbContext = AS_DbContext;
        }

        public int Commit()
        {
            return AS_DbContext.SaveChanges();
        }

        public void Dispose()
        {
            AS_DbContext.Dispose();
        }

       
    }
}

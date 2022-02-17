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

        private DbContext U_DbContext { get; set; }
        private UserManager<ApplicationUsersIdentity> _userManager;
        private RoleManager<IdentityRole> _roleManager;

        public AccountRepository Account;
        public AccountRepository account
        {
            get
            {
                if (Account == null)
                    Account = new AccountRepository(U_DbContext, _userManager, _roleManager);
                return Account;
            }
        }

        public UnitOfWork(ApplicationDBContext U_DbContext, UserManager<ApplicationUsersIdentity> userManager, RoleManager<IdentityRole> roleManager)
        {
            this._userManager = userManager;
            this._roleManager = roleManager;
            this.U_DbContext = U_DbContext;//
        }

        public int Commit()
        {
            return U_DbContext.SaveChanges();
        }

        public void Dispose()
        {
            U_DbContext.Dispose();
        }
   
    }
}

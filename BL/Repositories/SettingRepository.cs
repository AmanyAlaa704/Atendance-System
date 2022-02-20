using BL.Bases;
using BL.StaticClasses;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Repositories
{
    public class SettingRepository:BaseRepository<Setting>
    {
        private DbContext AS_DbContext;

        public SettingRepository(DbContext AS_DbContext) : base(AS_DbContext)
        {
            this.AS_DbContext = AS_DbContext;
        }
        #region CRUB

       
        public List<Setting> GetAllSetting()
        {
            return GetAll().ToList();
        }

        public Setting CheckArriveFeildsExists()
        {
            return GetWhere(l => l.FeildName == SettingFeildsNames.Arrive ).FirstOrDefault();            
        }

        public Setting CheckLeaveFeildsExists()
        {
            return GetWhere(l => l.FeildName == SettingFeildsNames.Leave).FirstOrDefault();
        }

        public bool InsertSetting(Setting setting)
        {
            return Insert(setting);
        }
        public void UpdateSetting(Setting setting)
        {
            Update(setting);
        }
        public void DeleteSetting(int id)
        {
            Delete(id);
        }
        
        #endregion

    }
}

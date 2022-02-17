using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class EmployeeLogs
    {
        public int ID { get; set; }
        public string UserID { get; set; }
        public string EmpRole { get; set; }
        public DateTime dateTime { get; set; }
        public string LogInOrOut { get; set; }

        [ForeignKey("UserID")]
        public ApplicationIdentity applicationIdentity { get; set; }

    }
}

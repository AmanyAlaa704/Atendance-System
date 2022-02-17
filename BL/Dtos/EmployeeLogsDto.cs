using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Dtos
{
    public class EmployeeLogsDto
    {
        public int ID { get; set; }
        public string UserID { get; set; }
        public string EmpRole { get; set; }
        public DateTime dateTime { get; set; }
        public string LogInOrOut { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace TelerikMvcApp1.Models
{
    public class NewUserModel
    {
        public IList<Department> Department { get; set; }

        public int DepartmentId { get; set; }

        public string UserName { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace userManagement
{
    public class User
    {
        public String UserName { get; set; }
        public String UserEmail { get; set; }
        public DateTime UserBirthDate { get; set; }
        public List<Tenant> UserTenants { get; set; }=new List<Tenant>();

     }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace userManagement
{
    public class User
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public DateTime UserBirthDate { get; set; }
        public List<Tenant> UserTenants { get; set; }=new List<Tenant>();

     }
}

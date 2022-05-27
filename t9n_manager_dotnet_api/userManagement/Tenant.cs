using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace userManagement
{
    public class Tenant
    {
        public Guid Id { get; set; } //Internal Id. Necessary for allowing a user to access Tenant data 
        public string Name { get; set; }

        public string AdminUserName { get; set; }
    }
}

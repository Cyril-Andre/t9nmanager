using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace userManagement
{
    public class Tenant
    {
        public Guid TenantKey { get; set; } //Internal Id. Necessary for allowing a user to access Tenant data 
        public string TenantName { get; set; }

        public string AdminUserName { get; set; }
    }
}

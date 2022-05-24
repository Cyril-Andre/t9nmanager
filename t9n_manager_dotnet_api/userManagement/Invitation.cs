using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace userManagement
{
    public class Invitation
    {
        public Tenant tenant { get; set; }
        public string userEmail { get; set; }
    }
}

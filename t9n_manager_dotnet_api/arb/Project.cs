using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using userManagement;

namespace arb
{
    public class Project
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Tenant Tenant { get; set; }
        public List<User> Users { get; set; }=new List<User>();
    }
}

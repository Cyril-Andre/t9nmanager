using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace t9n.DAL
{
    [Table("Tenants")]
    public class DbTenant
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid InternalId { get; set; }
        public string Name { get; set; }

        public string AdminUserName { get; set; }
        public List<DbUser> Users { get; set; }=new List<DbUser>();
        public List<DbProject> Projects { get; set; }=new List<DbProject>();
 
    }

}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace t9n.DAL
{
    [Table("Projects")]
    public class DbProject
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid InternalId { get; set; }
        public string Name { get; set; }
        public DbTenant Tenant { get; set; }
        public List<DbUser> Users { get; set; }= new List<DbUser>();
    }
}

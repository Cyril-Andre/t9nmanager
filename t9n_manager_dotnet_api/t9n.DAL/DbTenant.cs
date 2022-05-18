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
        public Guid TenantInternalId { get; set; }
        public string TenantName { get; set; }
        public List<DbUser> TenantUsers { get; set; } = new List<DbUser>();

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace t9n.DAL
{
    [Table("Users")]
    public class DbUser
    {
        [Key] [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid InternalId { get; set; }

        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string UserPasswordHash { get; set; }
        public bool UserEmailValidated { get; set; }
        public DateTime Birthdate { get; set; }
        public string Salt { get; set; }
        public string? ResetPasswordOtp { get; set; }
        public List<DbTenant> Tenants { get; set; }= new List<DbTenant>();
        public List<DbProject> Projects { get; set; } = new List<DbProject>();

    }
}

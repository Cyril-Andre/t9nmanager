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
        public Guid UserInternalId { get; set; }

        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string UserPasswordHash { get; set; }
        public bool UserEmailValidated { get; set; }
        public DateTime UserBirthdate { get; set; }
        public string Salt { get; set; }
    }
}

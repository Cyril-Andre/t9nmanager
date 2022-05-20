using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace t9n.DAL
{
    [Table("Invitations")]
    public class DbInvitation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid InvitationInternalId { get; set; }
        //        [Key, Column(Order =0)]
        public Guid TenantInternalId { get; set; }
        //        [Key, Column(Order = 1)]
        public String UserEmail { get; set; }
    }

}

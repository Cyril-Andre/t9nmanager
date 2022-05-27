using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace t9n.DAL
{
    [Table("ArbEntryAttribute")]
    public class DbArbResourceEntryAttribute
    {
        [Key] [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid AttributeInternalId { get; set; }
        public string AttributeName { get; set; }
        public string AttributeValue { get; set; }

    }


}

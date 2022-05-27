using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace t9n.DAL
{
    [Table("ArbEntryCollection")]
    public class DbArbResourceEntryCollection
    {
        [Key] [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid CollectionId { get; set; }
        public string Locale { get; set; }
        public string Context { get; set; }
        public List<DbArbResourceEntry> ArbEntries { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace t9n.DAL
{


    [Table("ArbEntry")]
    public class DbArbResourceEntry
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid EntryInternalId { get; set; }
        public string EntryId { get; set; }
        public string EntryValue { get; set; }

        [ForeignKey("Type")]
        public Guid? TypeId { get; set; }
        public DbArbResourceEntryAttribute Type { get; set; }
        [ForeignKey("Context")]
        public Guid? ContextId { get; set; }
        public DbArbResourceEntryAttribute Context { get; set; }
        [ForeignKey("Description")]
        public Guid? DescriptionId { get; set; }
        public DbArbResourceEntryAttribute Description { get; set; }
        [ForeignKey("SourceText")]
        public Guid? SourceTextId { get; set; }
        public DbArbResourceEntryAttribute SourceText { get; set; }
        [ForeignKey("Screen")]
        public Guid? ScreenId { get; set; }
        public DbArbResourceEntryAttribute Screen { get; set; }
        [ForeignKey("EntryCollection")]
        public Guid EntryCollectionId { get; set; }
        public DbArbResourceEntryCollection EntryCollection { get; set; }
    }
}

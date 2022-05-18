using System;
using Microsoft.EntityFrameworkCore;
namespace t9n.DAL
{
    public class t9nDbContext: DbContext
    {
        public t9nDbContext(DbContextOptions<t9nDbContext> options) : base(options) { }
        public DbSet<DbUser> Users { get; set; }
        public DbSet<DbTenant> Tenants { get; set; }
        public DbSet<DbArbResourceEntryAttribute> ArbAttributes { get; set; }
        public DbSet<DbArbResourceEntry> ArbEntry { get; set; }
        public DbSet<DbArbResourceEntryCollection> ArbCollection { get; set; }

        public DbSet<DbInvitation> Invations { get; set; }
    }
}

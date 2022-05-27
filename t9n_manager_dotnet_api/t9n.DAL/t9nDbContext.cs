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
        //public DbSet<DbUsersInTenant> UsersInTenant { get; set; }
        public DbSet<DbInvitation> Invitations { get; set; }
        public DbSet<DbProject> Projects { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Guid publicTenantGuid = Guid.Parse("1af0e668-086f-4a3c-8196-3231f21f6636");
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<DbTenant>().HasData(new DbTenant { InternalId = publicTenantGuid, AdminUserName = "t9nAdmin", Name = "Public" });

        }
    }
}

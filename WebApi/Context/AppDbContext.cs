using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApi.Entities;

namespace WebApi.Context
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>(u =>
            {
                u.ToTable("Users");
            });

            builder.Entity<IdentityRole>(u =>
            {
                u.ToTable("Roles");
            });

            builder.Entity<IdentityRoleClaim<string>>(u =>
            {
                u.ToTable("RoleClaims");
            });

            builder.Entity<IdentityUserRole<string>>(u =>
            {
                u.ToTable("UserRoles");
            });

            builder.Entity<IdentityUserClaim<string>>(u =>
            {
                u.ToTable("UserClaims");
            });

            builder.Entity<IdentityUserLogin<string>>(u =>
            {
                u.ToTable("UserLogins");
            });

            builder.Entity<IdentityUserToken<string>>(u =>
            {
                u.ToTable("UserTokens");
            });
        }
    }

}

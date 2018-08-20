using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Identity
{
    public class AppIdentityDbContext
        : IdentityDbContext<
            ApplicationUser, ApplicationRole, string,
            ApplicationUserClaim, ApplicationUserRole, ApplicationUserLogin,
            ApplicationRoleClaim, ApplicationUserToken>
    {
        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema("shop");

            modelBuilder.Entity<ApplicationUser>(b =>
            {
                // Each User can have many UserClaims
                b.HasMany(e => e.Claims)
                    .WithOne(e => e.User)
                    .HasForeignKey(uc => uc.UserId)
                    .IsRequired();

                // Each User can have many UserLogins
                b.HasMany(e => e.Logins)
                    .WithOne(e => e.User)
                    .HasForeignKey(ul => ul.UserId)
                    .IsRequired();

                // Each User can have many UserTokens
                b.HasMany(e => e.Tokens)
                    .WithOne(e => e.User)
                    .HasForeignKey(ut => ut.UserId)
                    .IsRequired();

                // Each User can have many entries in the UserRole join table
                b.HasMany(e => e.UserRoles)
                    .WithOne(e => e.User)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });

            modelBuilder.Entity<ApplicationRole>(b =>
            {
                // Each Role can have many entries in the UserRole join table
                b.HasMany(e => e.UserRoles)
                    .WithOne(e => e.Role)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

                // Each Role can have many associated RoleClaims
                b.HasMany(e => e.RoleClaims)
                    .WithOne(e => e.Role)
                    .HasForeignKey(rc => rc.RoleId)
                    .IsRequired();
            });

            modelBuilder.Entity<ApplicationUser>(b =>
            {
                b.ToTable("ApplicationUsers");
            });

            modelBuilder.Entity<ApplicationUserClaim>(b =>
            {
                b.ToTable("ApplicationUserClaims");
            });

            modelBuilder.Entity<ApplicationUserLogin>(b =>
            {
                b.ToTable("ApplicationUserLogins");
            });

            modelBuilder.Entity<ApplicationUserToken>(b =>
            {
                b.ToTable("ApplicationUserTokens");
            });

            modelBuilder.Entity<ApplicationRole>(b =>
            {
                b.ToTable("ApplicationRoles");
            });

            modelBuilder.Entity<ApplicationRoleClaim>(b =>
            {
                b.ToTable("ApplicationRoleClaims");
            });

            modelBuilder.Entity<ApplicationUserRole>(b =>
            {
                b.ToTable("ApplicationUserRoles");
            });

        }
    }
}
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Repositorys
{
    public class RepositoryContext : IdentityDbContext<User,Role,Guid,UserClaim,UserRole,IdentityUserLogin<Guid>,RoleClaim,IdentityUserToken<Guid>>
    {
        public RepositoryContext(DbContextOptions<RepositoryContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>(b =>
            {
                b.ToTable("users");
                b.Property(t => t.Id).HasColumnName("Id");
                b.Property(t => t.AccessFailedCount).HasColumnName("AccessFailedCount");
                b.Property(t => t.ConcurrencyStamp).HasColumnName("ConcurrencyStamp");
                b.Property(t => t.Email).HasColumnName("Email");
                b.Property(t => t.EmailConfirmed).HasColumnName("EmailConfirmed");
                b.Property(t => t.LockoutEnabled).HasColumnName("LockoutEnabled");
                b.Property(t => t.LockoutEnd).HasColumnName("LockoutEnd");
                b.Property(t => t.NormalizedEmail).HasColumnName("NormalizedEmail");
                b.Property(t => t.NormalizedUserName).HasColumnName("NormalizedUserName");
                b.Property(t => t.PhoneNumber).HasColumnName("PhoneNumber");
                b.Property(t => t.PhoneNumberConfirmed).HasColumnName("PhoneNumberConfirmed");
                b.Property(t => t.SecurityStamp).HasColumnName("SecurityStamp");
                b.Property(t => t.TwoFactorEnabled).HasColumnName("TwoFactorEnabled");
                b.Property(t => t.UserName).HasColumnName("UserName");
                b.Property(t => t.PasswordHash).HasColumnName("PasswordHash");
            });

            builder.Entity<Role>(b =>
            {
                b.ToTable("roles");
                b.Property(t => t.Id).HasColumnName("Id");
                b.Property(t => t.Name).HasColumnName("Name");
                b.Property(t => t.ConcurrencyStamp).HasColumnName("ConcurrencyStamp");
                b.Property(t => t.NormalizedName).HasColumnName("NormalizedName");
            });

            builder.Entity<UserRole>(b =>
            {
                b.ToTable("userRole");
                b.Property(t => t.UserId).HasColumnName("UserId");
                b.Property(t => t.RoleId).HasColumnName("RoleId");
            });

            builder.Entity<UserClaim>(b =>
            {
                b.ToTable("userClaim");
                b.Property(t => t.Id).HasColumnName("Id");
                b.Property(t => t.ClaimType).HasColumnName("ClaimType");
                b.Property(t => t.ClaimValue).HasColumnName("ClaimValue");
                b.Property(t => t.UserId).HasColumnName("UserId");
            });

            builder.Entity<RoleClaim>(b =>
            {
                b.ToTable("roleClaim");
                b.Property(t => t.Id).HasColumnName("Id");
                b.Property(t => t.ClaimType).HasColumnName("ClaimType");
                b.Property(t => t.ClaimValue).HasColumnName("ClaimValue");
                b.Property(t => t.RoleId).HasColumnName("RoleId");
            });

        }
    }
}

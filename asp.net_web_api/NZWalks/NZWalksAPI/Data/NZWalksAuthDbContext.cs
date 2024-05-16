using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace NZWalksAPI.Data
{
    public class NZWalksAuthDbContext : IdentityDbContext
    {
        // <NZWalksAuthDbContext> this is for choose type and this so important because 
        // the program.cs file can't detect which dbcontext will use 
        // and to distinguish between  between two dbcontext file
        public NZWalksAuthDbContext(DbContextOptions<NZWalksAuthDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            var readerRoleId = "[13f6cdce-d099-45db-8271-5d0f4c917c3c]";
            var writerRoleId = "[682cf265-33d4-4a6b-be5b-ae15c9044290]";
         //   var writerRoleId = "[108702e3-8b08-4a6c-bde1-4169113edf08]";
            var roles = new List<IdentityRole> {
            new IdentityRole{
            Id = readerRoleId,
            ConcurrencyStamp=readerRoleId,
            Name = "Reader",
            NormalizedName= "Reader".ToUpper()
            },
            new IdentityRole{ Id = writerRoleId,
            ConcurrencyStamp = writerRoleId,
            Name ="Writer",
            NormalizedName = "Writer".ToUpper()
            }
            };
            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuthTrainingDL.Context
{
    public class AuthTrainingContext : IdentityDbContext<IdentityUser>
    {
        public AuthTrainingContext(DbContextOptions<AuthTrainingContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}

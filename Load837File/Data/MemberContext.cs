using EncDataModel.Submission837;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Load837File.Data
{
    public class MemberContext : DbContext
    {
        public MemberContext(DbContextOptions<MemberContext> options)
          : base(options)
        {
            Database.SetCommandTimeout(TimeSpan.FromMinutes(30));
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MemberDetail>().HasNoKey();
        }
        public DbSet<MemberDetail> MemberDetails { get; set; }
    }
}

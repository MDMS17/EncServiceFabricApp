using EncDataModel.Meditrac;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Load837Meditrac.Data
{
    public class MeditracContext : DbContext
    {
        public MeditracContext(DbContextOptions<MeditracContext> options)
          : base(options)
        {
            Database.SetCommandTimeout(TimeSpan.FromMinutes(30));
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MeditracHeader>().HasNoKey();
            modelBuilder.Entity<MeditracLine>().HasNoKey();
            modelBuilder.Entity<MeditracCode>().HasNoKey();
        }
        public DbSet<MeditracHeader> meditracHeaders { get; set; }
        public DbSet<MeditracLine> meditracLines { get; set; }
        public DbSet<MeditracCode> meditracCodes { get; set; }
    }

}

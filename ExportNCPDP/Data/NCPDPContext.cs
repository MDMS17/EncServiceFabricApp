using EncDataModel.NCPDP42;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExportNCPDP.Data
{
    public class NCPDPContext : DbContext
    {
        public NCPDPContext(DbContextOptions<NCPDPContext> options)
                  : base(options)
        {
            Database.SetCommandTimeout(TimeSpan.FromMinutes(30));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("dbo");
            modelBuilder.Entity<HisCompound1>().ToTable("HisCompound1");
            modelBuilder.Entity<HisCompound2>().ToTable("HisCompound2");
            modelBuilder.Entity<HisDetail>().ToTable("HisDetails");
            modelBuilder.Entity<HisHeader>().ToTable("HisHeaders");
        }
        public DbSet<HisHeader> Headers { get; set; }
        public DbSet<HisDetail> Details { get; set; }
        public DbSet<HisCompound1> Compound1s { get; set; }
        public DbSet<HisCompound2> Compound2s { get; set; }
    }
}

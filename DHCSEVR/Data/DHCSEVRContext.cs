using EncDataModel.DHCS;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DHCSEVR.Data
{
    public class DHCSEVRContext : DbContext
    {
        public DHCSEVRContext(DbContextOptions<DHCSEVRContext> options)
                  : base(options) 
        {
            Database.SetCommandTimeout(TimeSpan.FromMinutes(30));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("Response");
            modelBuilder.Entity<DHCSFile>().ToTable("DHCSFile");
            modelBuilder.Entity<DHCSTransaction>().ToTable("DHCSTransaction");
            modelBuilder.Entity<DHCSEncounter>().ToTable("DHCSEncounter");
            modelBuilder.Entity<DHCSEncounterResponse>().ToTable("DHCSEncounterResponse");
            modelBuilder.Entity<DHCSServiceLine>().ToTable("DHCSServiceLine");
        }
        public DbSet<DHCSFile> Files { get; set; }
        public DbSet<DHCSTransaction> Transactions { get; set; }
        public DbSet<DHCSEncounter> Encounters { get; set; }
        public DbSet<DHCSEncounterResponse> Responses { get; set; }
        public DbSet<DHCSServiceLine> ServiceLines { get; set; }
    }
}

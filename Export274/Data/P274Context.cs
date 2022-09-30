using EncDataModel.Provider274;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Export274.Data
{
    public class P274Context : DbContext
    {
        public P274Context(DbContextOptions<P274Context> options)
                  : base(options)
        {
            Database.SetCommandTimeout(TimeSpan.FromMinutes(30));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("dbo");
            modelBuilder.Entity<P274File>().ToTable("P274File");
            modelBuilder.Entity<P274Detail>().ToTable("P274Detail");
            modelBuilder.Entity<P274AffiliatedEntity>().ToTable("P274AffiliatedEntity");
            modelBuilder.Entity<P274GroupIdNumber>().ToTable("P274GroupIdNumber");
            modelBuilder.Entity<P274Information>().ToTable("P274Information");
            modelBuilder.Entity<P274SiteCRC>().ToTable("P274SiteCRC");
            modelBuilder.Entity<P274SiteWorkSchedule>().ToTable("P274SiteWorkSchedule");
            modelBuilder.Entity<P274SpecializationArea>().ToTable("P274SpecializationArea");
        }
        public DbSet<P274File> Files { get; set; }
        public DbSet<P274Detail> Details { get; set; }
        public DbSet<P274AffiliatedEntity> Entities { get; set; }
        public DbSet<P274GroupIdNumber> Groups { get; set; }
        public DbSet<P274Information> Infos { get; set; }
        public DbSet<P274SiteCRC> Crcs { get; set; }
        public DbSet<P274SiteWorkSchedule> Schedules { get; set; }
        public DbSet<P274SpecializationArea> Areas { get; set; }
    }
}

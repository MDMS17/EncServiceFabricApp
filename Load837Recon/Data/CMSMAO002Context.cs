using EncDataModel.CMSMAO002;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Load837Recon.Data
{
    public class CMSMAO002Context : DbContext
    {
        public CMSMAO002Context(DbContextOptions<CMSMAO002Context> options)
                   : base(options)
        {
            Database.SetCommandTimeout(TimeSpan.FromMinutes(30));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("Response");
            modelBuilder.Entity<MAO2File>().ToTable("MAO2File");
            modelBuilder.Entity<MAO2Detail>().ToTable("MAO2Detail");
        }
        public DbSet<MAO2File> Files { get; set; }
        public DbSet<MAO2Detail> Details { get; set; }
    }
}

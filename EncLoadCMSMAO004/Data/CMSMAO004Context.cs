using EncDataModel.CMSMAO004;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EncLoadCMSMAO004.Data
{
    public class CMSMAO004Context : DbContext
    {
        public CMSMAO004Context(DbContextOptions<CMSMAO004Context> options)
                   : base(options) 
        {
            Database.SetCommandTimeout(TimeSpan.FromMinutes(30));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("dbo");
            modelBuilder.Entity<MAO_004_Header>().ToTable("MAO_004_Header");
            modelBuilder.Entity<MAO_004_Detail>().ToTable("MAO_004_Detail");
            modelBuilder.Entity<MAO_004_DiagnosisCode>().ToTable("MAO_004_DiagnosisCode");
        }
        public DbSet<MAO_004_Header> Headers { get; set; }
        public DbSet<MAO_004_Detail> Details { get; set; }
        public DbSet<MAO_004_DiagnosisCode> DiagCodes { get; set; }
    }
}

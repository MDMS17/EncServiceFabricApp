using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EncDataModel.CMS277CA;

namespace EncLoadCMS277CA.Data
{
    public class CMS277CAContext : DbContext
    {
        public CMS277CAContext(DbContextOptions<CMS277CAContext> options)
            : base(options)
        {
            Database.SetCommandTimeout(TimeSpan.FromMinutes(30));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("Response");
            modelBuilder.Entity<_277CABillProv>().ToTable("277CABillProv");
            modelBuilder.Entity<_277CAFile>().ToTable("277CAFile");
            modelBuilder.Entity<_277CALine>().ToTable("277CALine");
            modelBuilder.Entity<_277CAPatient>().ToTable("277CAPatient");
            modelBuilder.Entity<_277CAStc>().ToTable("277CAStc");
        }
        public DbSet<_277CABillProv> BillProvs { get; set; }
        public DbSet<_277CAFile> Files { get; set; }
        public DbSet<_277CALine> Lines { get; set; }
        public DbSet<_277CAPatient> Patients { get; set; }
        public DbSet<_277CAStc> Stcs { get; set; }
    }
}

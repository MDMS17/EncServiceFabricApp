using EncDataModel.CMSMAO001;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EncLoadCMSMAO001.Data
{
    public class CMSMAO001Context : DbContext
    {
        public CMSMAO001Context(DbContextOptions<CMSMAO001Context> options)
                   : base(options) 
        {
            Database.SetCommandTimeout(TimeSpan.FromMinutes(30));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("dbo");
            modelBuilder.Entity<Mao001Header>().ToTable("Mao001Header");
            modelBuilder.Entity<Mao001Detail>().ToTable("Mao001Detail");
        }
        public DbSet<Mao001Header> Headers { get; set; }
        public DbSet<Mao001Detail> Details { get; set; }

    }
}

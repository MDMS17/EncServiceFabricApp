using EncDataModel.CMS999;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EncLoadCMS999.Data
{
    public class CMS999Context : DbContext
    {
        public CMS999Context(DbContextOptions<CMS999Context> options)
            : base(options)
        {
            Database.SetCommandTimeout(TimeSpan.FromMinutes(30));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("Response");
            modelBuilder.Entity<_999Element>().ToTable("999Element");
            modelBuilder.Entity<_999Error>().ToTable("999Error");
            modelBuilder.Entity<_999File>().ToTable("999File");
            modelBuilder.Entity<_999Transaction>().ToTable("999Transaction");
        }
        public DbSet<_999Element> Elements { get; set; }
        public DbSet<_999Error> Errors { get; set; }
        public DbSet<_999File> Files { get; set; }
        public DbSet<_999Transaction> Transactions { get; set; }
    }
}

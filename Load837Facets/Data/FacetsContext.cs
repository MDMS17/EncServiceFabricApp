using EncDataModel.Facets;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Load837Facets.Data
{
    public class FacetsContext : DbContext
    {
        public FacetsContext(DbContextOptions<FacetsContext> options)
                 : base(options)
        {
            Database.SetCommandTimeout(TimeSpan.FromMinutes(30));
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FacetsCode>().HasNoKey();
            modelBuilder.Entity<FacetsExtraSbr>().HasNoKey();
            modelBuilder.Entity<FacetsHeader>().HasNoKey();
            modelBuilder.Entity<FacetsLine>().HasNoKey();
            modelBuilder.Entity<FacetsExtraSvd>().HasNoKey();
            modelBuilder.Entity<FacetsProvider>().HasNoKey();
            modelBuilder.Entity<FacetsCount>().HasNoKey();
        }
        public DbSet<FacetsCode> FacetsCodes { get; set; }
        public DbSet<FacetsExtraSbr> FacetsExtraSbrs { get; set; }
        public DbSet<FacetsHeader> FacetsHeaders { get; set; }
        public DbSet<FacetsLine> FacetsLines { get; set; }
        public DbSet<FacetsExtraSvd> FacetsExtraSvds { get; set; }
        public DbSet<FacetsProvider> FacetsProviders { get; set; }
        public DbSet<FacetsCount> FacetsCounts { get; set; }
    }
}

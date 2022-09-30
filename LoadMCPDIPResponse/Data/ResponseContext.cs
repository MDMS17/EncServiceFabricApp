using EncDataModel.MCPDIP;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoadMCPDIPResponse.Data
{
    public class ResponseContext : DbContext
    {
        public ResponseContext(DbContextOptions<ResponseContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("Response");
            modelBuilder.Entity<McpdipHeader>().ToTable("McpdipHeader");
            modelBuilder.Entity<McpdipHierarchy>().ToTable("McpdipHierarchy");
            modelBuilder.Entity<McpdipChildren>().ToTable("McpdipChildren");
            modelBuilder.Entity<McpdipDetail>().ToTable("McpdipDetail");
        }
        public DbSet<McpdipHeader> McpdipHeaders { get; set; }
        public DbSet<McpdipHierarchy> McpdipHierarchies { get; set; }
        public DbSet<McpdipChildren> McpdipChildrens { get; set; }
        public DbSet<McpdipDetail> McpdipDetails { get; set; }

    }
}

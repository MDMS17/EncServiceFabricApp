using EncDataModel.Remittance835;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Remittance835.Data
{
    public class Remittance835Context : DbContext
    {
        public Remittance835Context(DbContextOptions<Remittance835Context> options)
                  : base(options)
        {
            Database.SetCommandTimeout(TimeSpan.FromMinutes(30));
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("dbo");
            modelBuilder.Entity<File835>().ToTable("File835");
            modelBuilder.Entity<Claim835>().ToTable("Claim835");
        }
        public DbSet<File835> Files { get; set; }
        public DbSet<Claim835> Claims { get; set; }
    }
}

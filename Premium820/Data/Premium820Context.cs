using EncDataModel.Premium820;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Premium820.Data
{
    public class Premium820Context : DbContext
    {
        public Premium820Context(DbContextOptions<Premium820Context> options)
                  : base(options) 
        {
            Database.SetCommandTimeout(TimeSpan.FromMinutes(30));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("dbo");
            modelBuilder.Entity<File820>().ToTable("File820");
            modelBuilder.Entity<Member820>().ToTable("Member820");
        }
        public DbSet<File820> Files { get; set; }
        public DbSet<Member820> Members { get; set; }
    }
}

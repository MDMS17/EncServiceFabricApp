using EncDataModel.Vega;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoadMCPDIPExcel.Data
{
    public class VegaContext : DbContext
    {
        public VegaContext(DbContextOptions<VegaContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("dbo");
            modelBuilder.Entity<NPIVega>().ToTable("NPI");
        }
        public virtual DbSet<NPIVega> NPIs { get; set; }
    }

}

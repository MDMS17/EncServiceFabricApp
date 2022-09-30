using EncDataModel.WPC_837I;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Load837WPC.Data
{
    public class WPC837IContext : DbContext
    {
        public WPC837IContext(DbContextOptions<WPC837IContext> options)
                  : base(options)
        {
            Database.SetCommandTimeout(TimeSpan.FromMinutes(30));
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("dbo");
            modelBuilder.Entity<EDITransaction>().ToTable("EDITransaction");
            modelBuilder.Entity<CLM>().ToTable("CLM");
            modelBuilder.Entity<Loops>().ToTable("Loops");
            modelBuilder.Entity<XMLDocument>().ToTable("XMLDocument");
        }
        public virtual DbSet<EDITransaction> EdiTransactions { get; set; }
        public virtual DbSet<CLM> Clms { get; set; }
        public virtual DbSet<Loops> Loopss { get; set; }
        public virtual DbSet<XMLDocument> XmlDocuments { get; set; }
    }
}
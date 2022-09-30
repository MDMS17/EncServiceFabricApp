using EncDataModel.MCPDIP;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeneratePCPAJson.Data
{
    public class LogContext : DbContext
    {
        public LogContext(DbContextOptions<LogContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("dbo");
            modelBuilder.Entity<ProcessLog>().ToTable("ProcessLog");
            modelBuilder.Entity<SubmissionLog>().ToTable("SubmissionLog");
            modelBuilder.Entity<OperationLog>().ToTable("OperationLog");
            modelBuilder.Entity<IpaFileLog>().ToTable("IpaFileLog");
            modelBuilder.Entity<TaxonomyCode>().ToTable("TaxonomyCode");
        }
        public DbSet<ProcessLog> ProcessLogs { get; set; }
        public DbSet<SubmissionLog> SubmissionLogs { get; set; }
        public DbSet<OperationLog> OperationLogs { get; set; }
        public DbSet<IpaFileLog> IpaFileLogs { get; set; }
        public DbSet<TaxonomyCode> TaxonomyCodes { get; set; }

    }
}

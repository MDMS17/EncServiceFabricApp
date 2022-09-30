using EncDataModel.Submission837;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Load837Recon.Data
{
    public class SubHistoryContext : DbContext
    {
        public SubHistoryContext(DbContextOptions<SubHistoryContext> options)
                  : base(options)
        {
            Database.SetCommandTimeout(TimeSpan.FromMinutes(30));
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("SubHistory");
            modelBuilder.Entity<ClaimCAS>().ToTable("ClaimCAS");
            modelBuilder.Entity<ClaimCRC>().ToTable("ClaimCRCs");
            modelBuilder.Entity<ClaimHeader>().ToTable("ClaimHeaders");
            modelBuilder.Entity<ClaimHI>().ToTable("ClaimHIs");
            modelBuilder.Entity<ClaimK3>().ToTable("ClaimK3s");
            modelBuilder.Entity<ClaimLineFRM>().ToTable("ClaimLineFRMs");
            modelBuilder.Entity<ClaimLineLQ>().ToTable("ClaimLineLQs");
            modelBuilder.Entity<ClaimLineMEA>().ToTable("ClaimLineMEAs");
            modelBuilder.Entity<ClaimLineSVD>().ToTable("ClaimLineSVDs");
            modelBuilder.Entity<ClaimNte>().ToTable("ClaimNtes");
            modelBuilder.Entity<ClaimPatient>().ToTable("ClaimPatients");
            modelBuilder.Entity<ClaimProvider>().ToTable("ClaimProviders");
            modelBuilder.Entity<ClaimPWK>().ToTable("ClaimPWKs");
            modelBuilder.Entity<ClaimSBR>().ToTable("ClaimSBRs");
            modelBuilder.Entity<ClaimSecondaryIdentification>().ToTable("ClaimSecondaryIdentifications");
            modelBuilder.Entity<ProviderContact>().ToTable("ProviderContacts");
            modelBuilder.Entity<ServiceLine>().ToTable("ServiceLines");
            modelBuilder.Entity<SubmissionLog>().ToTable("SubmissionLogs");
            modelBuilder.Entity<ToothStatus>().ToTable("ToothStatus");
        }
        public virtual DbSet<ClaimHeader> ClaimHeaders { get; set; }
        public virtual DbSet<ServiceLine> ServiceLines { get; set; }
        public virtual DbSet<SubmissionLog> SubmissionLogs { get; set; }
        public virtual DbSet<ClaimCAS> ClaimCAS { get; set; }
        public virtual DbSet<ClaimCRC> ClaimCRCs { get; set; }
        public virtual DbSet<ClaimHI> ClaimHIs { get; set; }
        public virtual DbSet<ClaimK3> ClaimK3s { get; set; }
        public virtual DbSet<ClaimLineFRM> ClaimLineFRMs { get; set; }
        public virtual DbSet<ClaimLineLQ> ClaimLineLQs { get; set; }
        public virtual DbSet<ClaimLineMEA> ClaimLineMEAs { get; set; }
        public virtual DbSet<ClaimLineSVD> ClaimLineSVDs { get; set; }
        public virtual DbSet<ClaimNte> ClaimNtes { get; set; }
        public virtual DbSet<ClaimProvider> ClaimProviders { get; set; }
        public virtual DbSet<ClaimPWK> ClaimPWKs { get; set; }
        public virtual DbSet<ClaimSBR> ClaimSBRs { get; set; }
        public virtual DbSet<ClaimPatient> ClaimPatients { get; set; }
        public virtual DbSet<ClaimSecondaryIdentification> ClaimSecondaryIdentifications { get; set; }
        public virtual DbSet<ProviderContact> ProviderContacts { get; set; }
        public virtual DbSet<ToothStatus> ToothStatus { get; set; }
    }
}

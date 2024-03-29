﻿using EncDataModel.MCPDIP;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeneratePCPAJson.Data
{
    public class HistoryContext : DbContext
    {
        public HistoryContext(DbContextOptions<HistoryContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("History");
            modelBuilder.Entity<McpdGrievance>().ToTable("McpdGrievance");
            modelBuilder.Entity<McpdAppeal>().ToTable("McpdAppeal");
            modelBuilder.Entity<McpdContinuityOfCare>().ToTable("McpdContinuityOfCare");
            modelBuilder.Entity<McpdOutOfNetwork>().ToTable("McpdOutOfNetwork");
            modelBuilder.Entity<McpdHeader>().ToTable("McpdHeader");
            modelBuilder.Entity<PcpAssignment>().ToTable("PcpAssignment");
            modelBuilder.Entity<PcpHeader>().ToTable("PcpHeader");
        }
        public DbSet<McpdGrievance> Grievances { get; set; }
        public DbSet<McpdAppeal> Appeals { get; set; }
        public DbSet<McpdContinuityOfCare> McpdContinuityOfCare { get; set; }
        public DbSet<McpdOutOfNetwork> McpdOutOfNetwork { get; set; }
        public DbSet<McpdHeader> McpdHeaders { get; set; }
        public DbSet<PcpAssignment> PcpAssignments { get; set; }
        public DbSet<PcpHeader> PcpHeaders { get; set; }
    }
}

using EncDataModel.ChartReview;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenerateChartReview.Data
{
    public class ChartReviewContext : DbContext
    {
        public ChartReviewContext(DbContextOptions<ChartReviewContext> options)
                  : base(options)
        {
            Database.SetCommandTimeout(TimeSpan.FromMinutes(30));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("dbo");
            modelBuilder.Entity<ChartReviewRecord>().ToTable("ChartReviewRecord");
        }
        public DbSet<ChartReviewRecord> Records { get; set; }
    }
}

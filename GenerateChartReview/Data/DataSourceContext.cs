using EncDataModel.ChartReview;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenerateChartReview.Data
{
    public class DataSourceContext : DbContext
    {
        public DataSourceContext(DbContextOptions<DataSourceContext> options)
          : base(options)
        {
            Database.SetCommandTimeout(TimeSpan.FromMinutes(30));
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChartReviewData>().HasNoKey();
        }
        public DbSet<ChartReviewData> ChartReviewData { get; set; }
    }
}

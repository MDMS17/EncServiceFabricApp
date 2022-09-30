using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Load274Data.Data
{
    public class P274SourceContext : DbContext
    {
        public P274SourceContext(DbContextOptions<P274SourceContext> options)
                  : base(options)
        {
            Database.SetCommandTimeout(TimeSpan.FromMinutes(30));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MoviePortal.Models
{
    public class MoviePortalContext : DbContext
    {
        public MoviePortalContext (DbContextOptions<MoviePortalContext> options)
            : base(options)
        {
        }

        public DbSet<MoviePortal.Models.Movie> Movie { get; set; }
    }
}

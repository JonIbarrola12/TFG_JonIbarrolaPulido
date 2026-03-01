using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace SportsHome.IL.Repository.EF
{
    public class SportsHomeContext : DbContext
    {
        public SportsHomeContext(DbContextOptions options) : base(options) { }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
        }
    }
}

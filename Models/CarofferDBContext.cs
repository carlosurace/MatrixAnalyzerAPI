using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MatrixAnalyzer.Models
{
    public class carofferDBContext: DbContext
    {
        public carofferDBContext(DbContextOptions<carofferDBContext> options):base(options)
        {

        }

        public DbSet<Dealership> Dealerships {get;set;}
    }
}
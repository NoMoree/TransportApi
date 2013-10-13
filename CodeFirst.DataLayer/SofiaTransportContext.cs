using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFirst.Model;

namespace CodeFirst.DataLayer
{
    public class SofiaTransportContext: DbContext
    {
        //public SofiaTransportContext()
        //    : base("BloggingDB")
        //{
        //}

        public DbSet<Transport> Transport { get; set; }
        public DbSet<Direction> Directions { get; set; }
        public DbSet<Stop> Stops { get; set; }

        public DbSet<StopName> StopName { get; set; }

    }
}

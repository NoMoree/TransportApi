using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeFirst.Model
{
    public class Transport
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public int Type { get; set; }
        public string SiteId { get; set; }

        public virtual ICollection<Direction> Directions { get; set; }

        public Transport()
        {
            this.Directions = new HashSet<Direction>();
        }
    }
}

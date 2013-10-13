using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeFirst.Model
{
    public class Direction
    {
        public int Id { get; set; }

        public virtual Stop FirstStop { get; set; }
        public virtual Stop LastStop { get; set; }


        public string DirectionId { get; set; }
        public virtual ICollection<Stop> Stops { get; set; }

        public string DirectionStops { get; set; }

        public Direction()
        {
            this.Stops = new HashSet<Stop>();
        }



        public virtual Transport TransportId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeFirst.Model
{
    public class Stop
    {
        public int Id { get; set; }

        public string StopId { get; set; }

        public int PublicId { get; set; }

        public virtual StopName Name { get; set; }
    }
}

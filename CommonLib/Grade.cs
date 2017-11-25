using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLib
{
    public class Grade
    {
        public int id { get; set; }
        public int personId { get; set; }
        public string name { get; set; }

        public override string ToString()
        {
            return $"{this.id}->{this.personId}->{this.name}";
        }
    }
}

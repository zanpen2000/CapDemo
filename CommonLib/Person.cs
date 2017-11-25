using System;

namespace CommonLib
{
    public class Person
    {
        public int id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }

        public override string ToString()
        {
            return $"{this.id}: {this.Name} 今年 {this.Age} 岁了";
        }
    }
}

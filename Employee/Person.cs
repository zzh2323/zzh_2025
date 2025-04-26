using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee
{
    public enum PersonnelLevel
    {
        Level_0 = 0, // 等级0
        Level_1 = 1, // 等级1
        Level_2 = 2, // 等级2
        Level_3 = 3  // 等级3
    }
    public class Person
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public PersonnelLevel Level { get; set; }

        public Person(string name, PersonnelLevel level, int id)
        {
            ID = id;
            Name = name;
            Level = level;
        }

    }
}

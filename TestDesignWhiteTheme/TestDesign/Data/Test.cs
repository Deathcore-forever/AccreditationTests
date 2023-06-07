using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDesign.Data
{
    public class Test
    {
        public int ID { get;}
        public string Name { get; }

        public int ResponseNumber { get; }

        public Test(int _id, string _name, int responseNumber)
        {
            ID = _id;
            Name = _name;
            ResponseNumber = responseNumber;
        }
        public override string ToString() => Name;
    }
}

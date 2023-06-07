using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDesign.Data
{
    public class Administrator
    {
        public int ID { get; }
        public string Login { get; }

        public Administrator(int constructorID, string login)
        {
            ID = constructorID;
            Login = login;
        }
    }
}

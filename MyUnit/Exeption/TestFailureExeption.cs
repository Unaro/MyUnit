using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyUnit.Exeption
{
    internal class TestFailureExeption : Exception
    {
        public TestFailureExeption(string message) : base(message) {
            
        }
    }
}

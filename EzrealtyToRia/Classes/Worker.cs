using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EzrealtyToRia.Classes
{
    public class Worker
    {
        public Worker ()
        {

        }

        public int Id { get; set; }
        public string Name { get; set; }
        public List<string> Phones { get; set; }
        public string Email { get; set; }
    }
}

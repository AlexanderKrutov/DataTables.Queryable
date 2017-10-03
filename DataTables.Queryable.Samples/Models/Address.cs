using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataTables.Queryable.Samples.Models
{
    public class Address
    {
        public int ID { get; set; }
        public string Street { get; set; }
        public int Building { get; set; }

        public Address() { }

        public Address(string street, int building)
        {
            Street = street;
            Building = building;
        } 
    }
}
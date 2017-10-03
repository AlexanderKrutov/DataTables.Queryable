using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataTables.Queryable.Samples.Models
{
    public class Office
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public Address Address { get; set; }
        public int AddressID { get; set; }

        public Office() { }

        public Office(string name, Address address)
        {
            Name = name;
            Address = address;
        }
    }
}
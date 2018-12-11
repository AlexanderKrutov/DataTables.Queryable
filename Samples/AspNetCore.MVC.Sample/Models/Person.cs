using System;

namespace AspNetCore.MVC.Sample.Models
{
    public class Person
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Position { get; set; }

        public int OfficeId { get; set; }

        public Office Office { get; set; }

        public int Extn { get; set; }

        public DateTime StartDate { get; set; }

        public decimal Salary { get; set; }
    }
}
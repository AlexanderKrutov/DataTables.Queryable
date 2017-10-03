using System;

namespace DataTables.Queryable.Samples.Models
{
    public class Person
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public int OfficeID { get; set; }
        public Office Office { get; set; }
        public int Extn { get; set; }
        public DateTime StartDate { get; set; }
        public decimal Salary { get; set; }

        public Person() { }

        public Person(string name, string position, Office office, string extn, string startDate, string salary)
        {
            Name = name;
            Position = position;
            Office = office;
            Extn = int.Parse(extn);
            StartDate = DateTime.ParseExact(startDate, "yyyy/MM/dd", null);
            Salary = decimal.Parse(salary);
        }
    }
}
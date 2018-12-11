using AspNetCore.MVC.Sample.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace AspNetCore.MVC.Sample
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext( DbContextOptions options) : base(options) { }

        public DbSet<Person> Persons { get; set; }

        public DbSet<Address> Addresses { get; set; }

        public DbSet<Office> Offices { get; set; }

        /// <summary>
        /// Seed Data
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var addresses = new Address[]
            {
                new Address { Id = 1, Street = "New Orchard Road", Building = 1 },
                new Address { Id = 2, Street = "One Microsoft Way", Building = 17 },
                new Address { Id = 3, Street = "Amphitheatre Parkway", Building = 1600 }
            };

            var offices = new Office[]
            {
                new Office { Id = 1, Name = "New Orchard Road", AddressId = addresses[0].Id },
                new Office { Id = 2, Name = "One Microsoft Way", AddressId = addresses[1].Id },
                new Office { Id = 3, Name = "Amphitheatre Parkway", AddressId = addresses[2].Id }
            };

            var persons = new Person[]
            {
                new Person { Id = 1, Name = "Tiger Nixon", Position = "System Architect", OfficeId = offices[0].Id, Extn = 5421, StartDate = DateTime.ParseExact("2011/04/25", "yyyy/MM/dd", null), Salary = 320800 },
                new Person { Id = 2, Name = "Garrett Winters", Position = "Accountant", OfficeId = offices[0].Id, Extn = 8422, StartDate = DateTime.ParseExact("2011/07/25", "yyyy/MM/dd", null), Salary = 170750 },
                new Person { Id = 3, Name = "Ashton Cox", Position = "Junior Technical Author", OfficeId = offices[1].Id, Extn = 1562, StartDate = DateTime.ParseExact("2009/01/12", "yyyy/MM/dd", null), Salary = 86000 },
                new Person { Id = 4, Name = "Cedric Kelly", Position = "Senior Javascript Developer", OfficeId = offices[0].Id, Extn = 6224, StartDate = DateTime.ParseExact("2012/03/29", "yyyy/MM/dd", null), Salary = 433060 },
                new Person { Id = 5, Name = "Airi Satou", Position = "Accountant", OfficeId = offices[0].Id, Extn = 5407, StartDate = DateTime.ParseExact("2008/11/28", "yyyy/MM/dd", null), Salary = 162700 },
                new Person { Id = 6, Name = "Brielle Williamson", Position = "Integration Specialist", OfficeId = offices[0].Id, Extn = 4804, StartDate = DateTime.ParseExact("2012/12/02", "yyyy/MM/dd", null), Salary = 372000 },
                new Person { Id = 7, Name = "Herrod Chandler", Position = "Sales Assistant", OfficeId = offices[0].Id, Extn = 9608, StartDate = DateTime.ParseExact("2012/08/06", "yyyy/MM/dd", null), Salary = 137500 },
                new Person { Id = 8, Name = "Rhona Davidson", Position = "Integration Specialist", OfficeId = offices[0].Id, Extn = 6200, StartDate = DateTime.ParseExact("2010/10/14", "yyyy/MM/dd", null), Salary = 327900 },
                new Person { Id = 9, Name = "Colleen Hurst", Position = "Javascript Developer", OfficeId = offices[1].Id, Extn = 2360, StartDate = DateTime.ParseExact("2009/09/15", "yyyy/MM/dd", null), Salary = 205500 },
                new Person { Id = 10, Name = "Sonya Frost", Position = "Software Engineer", OfficeId = offices[1].Id, Extn = 1667, StartDate = DateTime.ParseExact("2008/12/13", "yyyy/MM/dd", null), Salary = 103600 },
                new Person { Id = 11, Name = "Jena Gaines", Position = "Office Manager", OfficeId = offices[0].Id, Extn = 3814, StartDate = DateTime.ParseExact("2008/12/19", "yyyy/MM/dd", null), Salary = 90560 },
                new Person { Id = 12, Name = "Quinn Flynn", Position = "Support Lead", OfficeId = offices[1].Id, Extn = 9497, StartDate = DateTime.ParseExact("2013/03/03", "yyyy/MM/dd", null), Salary = 342000 },
                new Person { Id = 13, Name = "Charde Marshall", Position = "Regional Director", OfficeId = offices[1].Id, Extn = 6741, StartDate = DateTime.ParseExact("2008/10/16", "yyyy/MM/dd", null), Salary = 470600 },
                new Person { Id = 14, Name = "Haley Kennedy", Position = "Senior Marketing Designer", OfficeId = offices[0].Id, Extn = 3597, StartDate = DateTime.ParseExact("2012/12/18", "yyyy/MM/dd", null), Salary = 313500 },
                new Person { Id = 15, Name = "Tatyana Fitzpatrick", Position = "Regional Director", OfficeId = offices[0].Id, Extn = 1965, StartDate = DateTime.ParseExact("2010/03/17", "yyyy/MM/dd", null), Salary = 385750 },
                new Person { Id = 16, Name = "Michael Silva", Position = "Marketing Designer", OfficeId = offices[2].Id, Extn = 1581, StartDate = DateTime.ParseExact("2012/11/27", "yyyy/MM/dd", null), Salary = 198500 },
                new Person { Id = 17, Name = "Paul Byrd", Position = "Chief Financial Officer (CFO)", OfficeId = offices[0].Id, Extn = 3059, StartDate = DateTime.ParseExact("2010/06/09", "yyyy/MM/dd", null), Salary = 725000 },
                new Person { Id = 18, Name = "Gloria Little", Position = "Systems Administrator", OfficeId = offices[2].Id, Extn = 1721, StartDate = DateTime.ParseExact("2009/04/10", "yyyy/MM/dd", null), Salary = 237500 },
                new Person { Id = 19, Name = "Bradley Greer", Position = "Software Engineer", OfficeId = offices[0].Id, Extn = 2558, StartDate = DateTime.ParseExact("2012/10/13", "yyyy/MM/dd", null), Salary = 132000 },
                new Person { Id = 20, Name = "Dai Rios", Position = "Personnel Lead", OfficeId = offices[0].Id, Extn = 2290, StartDate = DateTime.ParseExact("2012/09/26", "yyyy/MM/dd", null), Salary = 217500 },
                new Person { Id = 21, Name = "Jenette Caldwell", Position = "Development Lead", OfficeId = offices[0].Id, Extn = 1937, StartDate = DateTime.ParseExact("2011/09/03", "yyyy/MM/dd", null), Salary = 345000 },
                new Person { Id = 22, Name = "Yuri Berry", Position = "Chief Marketing Officer (CMO)", OfficeId = offices[2].Id, Extn = 6154, StartDate = DateTime.ParseExact("2009/06/25", "yyyy/MM/dd", null), Salary = 675000 },
                new Person { Id = 23, Name = "Caesar Vance", Position = "Pre-Sales Support", OfficeId = offices[0].Id, Extn = 8330, StartDate = DateTime.ParseExact("2011/12/12", "yyyy/MM/dd", null), Salary = 106450 },
                new Person { Id = 24, Name = "Doris Wilder", Position = "Sales Assistant", OfficeId = offices[0].Id, Extn = 3023, StartDate = DateTime.ParseExact("2010/09/20", "yyyy/MM/dd", null), Salary = 85600 },
                new Person { Id = 25, Name = "Angelica Ramos", Position = "Chief Executive Officer (CEO)", OfficeId = offices[0].Id, Extn = 5797, StartDate = DateTime.ParseExact("2009/10/09", "yyyy/MM/dd", null), Salary = 1200000 },
                new Person { Id = 26, Name = "Gavin Joyce", Position = "Developer", OfficeId = offices[0].Id, Extn = 8822, StartDate = DateTime.ParseExact("2010/12/22", "yyyy/MM/dd", null), Salary = 92575 },
                new Person { Id = 27, Name = "Jennifer Chang", Position = "Regional Director", OfficeId = offices[2].Id, Extn = 9239, StartDate = DateTime.ParseExact("2010/11/14", "yyyy/MM/dd", null), Salary = 357650 },
                new Person { Id = 28, Name = "Brenden Wagner", Position = "Software Engineer", OfficeId = offices[0].Id, Extn = 1314, StartDate = DateTime.ParseExact("2011/06/07", "yyyy/MM/dd", null), Salary = 206850 },
                new Person { Id = 29, Name = "Fiona Green", Position = "Chief Operating Officer (COO)", OfficeId = offices[0].Id, Extn = 2947, StartDate = DateTime.ParseExact("2010/03/11", "yyyy/MM/dd", null), Salary = 850000 },
                new Person { Id = 30, Name = "Shou Itou", Position = "Regional Marketing", OfficeId = offices[1].Id, Extn = 8899, StartDate = DateTime.ParseExact("2011/08/14", "yyyy/MM/dd", null), Salary = 163000 },
                new Person { Id = 31, Name = "Michelle House", Position = "Integration Specialist", OfficeId = offices[0].Id, Extn = 2769, StartDate = DateTime.ParseExact("2011/06/02", "yyyy/MM/dd", null), Salary = 95400 },
                new Person { Id = 32, Name = "Suki Burks", Position = "Developer", OfficeId = offices[0].Id, Extn = 6832, StartDate = DateTime.ParseExact("2009/10/22", "yyyy/MM/dd", null), Salary = 114500 },
                new Person { Id = 33, Name = "Prescott Bartlett", Position = "Technical Author", OfficeId = offices[1].Id, Extn = 3606, StartDate = DateTime.ParseExact("2011/05/07", "yyyy/MM/dd", null), Salary = 145000 },
                new Person { Id = 34, Name = "Gavin Cortez", Position = "Team Leader", OfficeId = offices[0].Id, Extn = 2860, StartDate = DateTime.ParseExact("2008/10/26", "yyyy/MM/dd", null), Salary = 235500 },
                new Person { Id = 35, Name = "Martena Mccray", Position = "Post-Sales support", OfficeId = offices[1].Id, Extn = 8240, StartDate = DateTime.ParseExact("2011/03/09", "yyyy/MM/dd", null), Salary = 324050 },
                new Person { Id = 36, Name = "Unity Butler", Position = "Marketing Designer", OfficeId = offices[0].Id, Extn = 5384, StartDate = DateTime.ParseExact("2009/12/09", "yyyy/MM/dd", null), Salary = 85675 },
                new Person { Id = 37, Name = "Howard Hatfield", Position = "Office Manager", OfficeId = offices[2].Id, Extn = 7031, StartDate = DateTime.ParseExact("2008/12/16", "yyyy/MM/dd", null), Salary = 164500 },
                new Person { Id = 38, Name = "Hope Fuentes", Position = "Secretary", OfficeId = offices[0].Id, Extn = 6318, StartDate = DateTime.ParseExact("2010/02/12", "yyyy/MM/dd", null), Salary = 109850 },
                new Person { Id = 39, Name = "Vivian Harrell", Position = "Financial Controller", OfficeId = offices[0].Id, Extn = 9422, StartDate = DateTime.ParseExact("2009/02/14", "yyyy/MM/dd", null), Salary = 452500 },
                new Person { Id = 40, Name = "Timothy Mooney", Position = "Office Manager", OfficeId = offices[0].Id, Extn = 7580, StartDate = DateTime.ParseExact("2008/12/11", "yyyy/MM/dd", null), Salary = 136200 },
                new Person { Id = 41, Name = "Jackson Bradshaw", Position = "Director", OfficeId = offices[0].Id, Extn = 1042, StartDate = DateTime.ParseExact("2008/09/26", "yyyy/MM/dd", null), Salary = 645750 },
                new Person { Id = 42, Name = "Olivia Liang", Position = "Support Engineer", OfficeId = offices[2].Id, Extn = 2120, StartDate = DateTime.ParseExact("2011/02/03", "yyyy/MM/dd", null), Salary = 234500 },
                new Person { Id = 43, Name = "Bruno Nash", Position = "Software Engineer", OfficeId = offices[0].Id, Extn = 6222, StartDate = DateTime.ParseExact("2011/05/03", "yyyy/MM/dd", null), Salary = 163500 },
                new Person { Id = 44, Name = "Sakura Yamamoto", Position = "Support Engineer", OfficeId = offices[1].Id, Extn = 9383, StartDate = DateTime.ParseExact("2009/08/19", "yyyy/MM/dd", null), Salary = 139575 },
                new Person { Id = 45, Name = "Thor Walton", Position = "Developer", OfficeId = offices[0].Id, Extn = 8327, StartDate = DateTime.ParseExact("2013/08/11", "yyyy/MM/dd", null), Salary = 98540 },
                new Person { Id = 46, Name = "Finn Camacho", Position = "Support Engineer", OfficeId = offices[0].Id, Extn = 2927, StartDate = DateTime.ParseExact("2009/07/07", "yyyy/MM/dd", null), Salary = 87500 },
                new Person { Id = 47, Name = "Serge Baldwin", Position = "Data Coordinator", OfficeId = offices[0].Id, Extn = 8352, StartDate = DateTime.ParseExact("2012/04/09", "yyyy/MM/dd", null), Salary = 138575 },
                new Person { Id = 48, Name = "Zenaida Frank", Position = "Software Engineer", OfficeId = offices[0].Id, Extn = 7439, StartDate = DateTime.ParseExact("2010/01/04", "yyyy/MM/dd", null), Salary = 125250 },
                new Person { Id = 49, Name = "Zorita Serrano", Position = "Software Engineer", OfficeId = offices[0].Id, Extn = 4389, StartDate = DateTime.ParseExact("2012/06/01", "yyyy/MM/dd", null), Salary = 115000 },
                new Person { Id = 50, Name = "Jennifer Acosta", Position = "Junior Javascript Developer", OfficeId = offices[0].Id, Extn = 3431, StartDate = DateTime.ParseExact("2013/02/01", "yyyy/MM/dd", null), Salary = 75650 },
                new Person { Id = 51, Name = "Cara Stevens", Position = "Sales Assistant", OfficeId = offices[0].Id, Extn = 3990, StartDate = DateTime.ParseExact("2011/12/06", "yyyy/MM/dd", null), Salary = 145600 },
                new Person { Id = 52, Name = "Hermione Butler", Position = "Regional Director", OfficeId = offices[0].Id, Extn = 1016, StartDate = DateTime.ParseExact("2011/03/21", "yyyy/MM/dd", null), Salary = 356250 },
                new Person { Id = 53, Name = "Lael Greer", Position = "Systems Administrator", OfficeId = offices[1].Id, Extn = 6733, StartDate = DateTime.ParseExact("2009/02/27", "yyyy/MM/dd", null), Salary = 103500 },
                new Person { Id = 54, Name = "Jonas Alexander", Position = "Developer", OfficeId = offices[1].Id, Extn = 8196, StartDate = DateTime.ParseExact("2010/07/14", "yyyy/MM/dd", null), Salary = 86500 },
                new Person { Id = 55, Name = "Shad Decker", Position = "Regional Director", OfficeId = offices[0].Id, Extn = 6373, StartDate = DateTime.ParseExact("2008/11/13", "yyyy/MM/dd", null), Salary = 183000 },
                new Person { Id = 56, Name = "Michael Bruce", Position = "Javascript Developer", OfficeId = offices[0].Id, Extn = 5384, StartDate = DateTime.ParseExact("2011/06/27", "yyyy/MM/dd", null), Salary = 183000 },
                new Person { Id = 57, Name = "Donna Snider", Position = "Customer Support", OfficeId = offices[0].Id, Extn = 4226, StartDate = DateTime.ParseExact("2011/01/25", "yyyy/MM/dd", null), Salary = 112000 }
            };

            modelBuilder.Entity<Address>().HasData(addresses);
            modelBuilder.Entity<Office>().HasData(offices);
            modelBuilder.Entity<Person>().HasData(persons);
        }

    }
}
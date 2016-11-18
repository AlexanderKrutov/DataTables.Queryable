using DataTables.Queryable.Samples.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DataTables.Queryable.Samples.Database
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Person> Persons { get; set; }

        public DatabaseContext() : base()
        {
            System.Data.Entity.Database.SetInitializer(new DatabaseInitializer());
        }

        private class DatabaseInitializer : CreateDatabaseIfNotExists<DatabaseContext>
        {
            protected override void Seed(DatabaseContext context)
            {
                if (context.Persons.Any())
                {
                    return;  
                }

                var persons = new Person[]
                {
                    new Person("Tiger Nixon", "System Architect", "Edinburgh", "5421", "2011/04/25", "320800"),
                    new Person("Garrett Winters", "Accountant", "Tokyo", "8422", "2011/07/25", "170750"),
                    new Person("Ashton Cox", "Junior Technical Author", "San Francisco", "1562", "2009/01/12", "86000"),
                    new Person("Cedric Kelly", "Senior Javascript Developer", "Edinburgh", "6224", "2012/03/29", "433060"),
                    new Person("Airi Satou", "Accountant", "Tokyo", "5407", "2008/11/28", "162700"),
                    new Person("Brielle Williamson", "Integration Specialist", "New York", "4804", "2012/12/02", "372000"),
                    new Person("Herrod Chandler", "Sales Assistant", "San Francisco", "9608", "2012/08/06", "137500"),
                    new Person("Rhona Davidson", "Integration Specialist", "Tokyo", "6200", "2010/10/14", "327900"),
                    new Person("Colleen Hurst", "Javascript Developer", "San Francisco", "2360", "2009/09/15", "205500"),
                    new Person("Sonya Frost", "Software Engineer", "Edinburgh", "1667", "2008/12/13", "103600"),
                    new Person("Jena Gaines", "Office Manager", "London", "3814", "2008/12/19", "90560"),
                    new Person("Quinn Flynn", "Support Lead", "Edinburgh", "9497", "2013/03/03", "342000"),
                    new Person("Charde Marshall", "Regional Director", "San Francisco", "6741", "2008/10/16", "470600"),
                    new Person("Haley Kennedy", "Senior Marketing Designer", "London", "3597", "2012/12/18", "313500"),
                    new Person("Tatyana Fitzpatrick", "Regional Director", "London", "1965", "2010/03/17", "385750"),
                    new Person("Michael Silva", "Marketing Designer", "London", "1581", "2012/11/27", "198500"),
                    new Person("Paul Byrd", "Chief Financial Officer (CFO)", "New York", "3059", "2010/06/09", "725000"),
                    new Person("Gloria Little", "Systems Administrator", "New York", "1721", "2009/04/10", "237500"),
                    new Person("Bradley Greer", "Software Engineer", "London", "2558", "2012/10/13", "132000"),
                    new Person("Dai Rios", "Personnel Lead", "Edinburgh", "2290", "2012/09/26", "217500"),
                    new Person("Jenette Caldwell", "Development Lead", "New York", "1937", "2011/09/03", "345000"),
                    new Person("Yuri Berry", "Chief Marketing Officer (CMO)", "New York", "6154", "2009/06/25", "675000"),
                    new Person("Caesar Vance", "Pre-Sales Support", "New York", "8330", "2011/12/12", "106450"),
                    new Person("Doris Wilder", "Sales Assistant", "Sidney", "3023", "2010/09/20", "85600"),
                    new Person("Angelica Ramos", "Chief Executive Officer (CEO)", "London", "5797", "2009/10/09", "1200000"),
                    new Person("Gavin Joyce", "Developer", "Edinburgh", "8822", "2010/12/22", "92575"),
                    new Person("Jennifer Chang", "Regional Director", "Singapore", "9239", "2010/11/14", "357650"),
                    new Person("Brenden Wagner", "Software Engineer", "San Francisco", "1314", "2011/06/07", "206850"),
                    new Person("Fiona Green", "Chief Operating Officer (COO)", "San Francisco", "2947", "2010/03/11", "850000"),
                    new Person("Shou Itou", "Regional Marketing", "Tokyo", "8899", "2011/08/14", "163000"),
                    new Person("Michelle House", "Integration Specialist", "Sidney", "2769", "2011/06/02", "95400"),
                    new Person("Suki Burks", "Developer", "London", "6832", "2009/10/22", "114500"),
                    new Person("Prescott Bartlett", "Technical Author", "London", "3606", "2011/05/07", "145000"),
                    new Person("Gavin Cortez", "Team Leader", "San Francisco", "2860", "2008/10/26", "235500"),
                    new Person("Martena Mccray", "Post-Sales support", "Edinburgh", "8240", "2011/03/09", "324050"),
                    new Person("Unity Butler", "Marketing Designer", "San Francisco", "5384", "2009/12/09", "85675"),
                    new Person("Howard Hatfield", "Office Manager", "San Francisco", "7031", "2008/12/16", "164500"),
                    new Person("Hope Fuentes", "Secretary", "San Francisco", "6318", "2010/02/12", "109850"),
                    new Person("Vivian Harrell", "Financial Controller", "San Francisco", "9422", "2009/02/14", "452500"),
                    new Person("Timothy Mooney", "Office Manager", "London", "7580", "2008/12/11", "136200"),
                    new Person("Jackson Bradshaw", "Director", "New York", "1042", "2008/09/26", "645750"),
                    new Person("Olivia Liang", "Support Engineer", "Singapore", "2120", "2011/02/03", "234500"),
                    new Person("Bruno Nash", "Software Engineer", "London", "6222", "2011/05/03", "163500"),
                    new Person("Sakura Yamamoto", "Support Engineer", "Tokyo", "9383", "2009/08/19", "139575"),
                    new Person("Thor Walton", "Developer", "New York", "8327", "2013/08/11", "98540"),
                    new Person("Finn Camacho", "Support Engineer", "San Francisco", "2927", "2009/07/07", "87500"),
                    new Person("Serge Baldwin", "Data Coordinator", "Singapore", "8352", "2012/04/09", "138575"),
                    new Person("Zenaida Frank", "Software Engineer", "New York", "7439", "2010/01/04", "125250"),
                    new Person("Zorita Serrano", "Software Engineer", "San Francisco", "4389", "2012/06/01", "115000"),
                    new Person("Jennifer Acosta", "Junior Javascript Developer", "Edinburgh", "3431", "2013/02/01", "75650"),
                    new Person("Cara Stevens", "Sales Assistant", "New York", "3990", "2011/12/06", "145600"),
                    new Person("Hermione Butler", "Regional Director", "London", "1016", "2011/03/21", "356250"),
                    new Person("Lael Greer", "Systems Administrator", "London", "6733", "2009/02/27", "103500"),
                    new Person("Jonas Alexander", "Developer", "San Francisco", "8196", "2010/07/14", "86500"),
                    new Person("Shad Decker", "Regional Director", "Edinburgh", "6373", "2008/11/13", "183000"),
                    new Person("Michael Bruce", "Javascript Developer", "Singapore", "5384", "2011/06/27", "183000"),
                    new Person("Donna Snider", "Customer Support", "New York", "4226", "2011/01/25", "112000")
                };
                context.Persons.AddRange(persons);
                context.SaveChanges();

                base.Seed(context);
            }
        }
    }


}
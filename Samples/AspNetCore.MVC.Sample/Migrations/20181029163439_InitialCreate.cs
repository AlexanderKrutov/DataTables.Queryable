using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AspNetCore.MVC.Sample.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Street = table.Column<string>(nullable: true),
                    Building = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Offices",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    AddressId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Offices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Offices_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    Position = table.Column<string>(nullable: true),
                    OfficeId = table.Column<int>(nullable: false),
                    Extn = table.Column<int>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    Salary = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Persons_Offices_OfficeId",
                        column: x => x.OfficeId,
                        principalTable: "Offices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Addresses",
                columns: new[] { "Id", "Building", "Street" },
                values: new object[] { 1, 1, "New Orchard Road" });

            migrationBuilder.InsertData(
                table: "Addresses",
                columns: new[] { "Id", "Building", "Street" },
                values: new object[] { 2, 17, "One Microsoft Way" });

            migrationBuilder.InsertData(
                table: "Addresses",
                columns: new[] { "Id", "Building", "Street" },
                values: new object[] { 3, 1600, "Amphitheatre Parkway" });

            migrationBuilder.InsertData(
                table: "Offices",
                columns: new[] { "Id", "AddressId", "Name" },
                values: new object[] { 1, 1, "New Orchard Road" });

            migrationBuilder.InsertData(
                table: "Offices",
                columns: new[] { "Id", "AddressId", "Name" },
                values: new object[] { 2, 2, "One Microsoft Way" });

            migrationBuilder.InsertData(
                table: "Offices",
                columns: new[] { "Id", "AddressId", "Name" },
                values: new object[] { 3, 3, "Amphitheatre Parkway" });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "Id", "Extn", "Name", "OfficeId", "Position", "Salary", "StartDate" },
                values: new object[] { 1, 5421, "Tiger Nixon", 1, "System Architect", 320800m, new DateTime(2011, 4, 25, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "Id", "Extn", "Name", "OfficeId", "Position", "Salary", "StartDate" },
                values: new object[] { 46, 2927, "Finn Camacho", 1, "Support Engineer", 87500m, new DateTime(2009, 7, 7, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "Id", "Extn", "Name", "OfficeId", "Position", "Salary", "StartDate" },
                values: new object[] { 47, 8352, "Serge Baldwin", 1, "Data Coordinator", 138575m, new DateTime(2012, 4, 9, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "Id", "Extn", "Name", "OfficeId", "Position", "Salary", "StartDate" },
                values: new object[] { 48, 7439, "Zenaida Frank", 1, "Software Engineer", 125250m, new DateTime(2010, 1, 4, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "Id", "Extn", "Name", "OfficeId", "Position", "Salary", "StartDate" },
                values: new object[] { 49, 4389, "Zorita Serrano", 1, "Software Engineer", 115000m, new DateTime(2012, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "Id", "Extn", "Name", "OfficeId", "Position", "Salary", "StartDate" },
                values: new object[] { 50, 3431, "Jennifer Acosta", 1, "Junior Javascript Developer", 75650m, new DateTime(2013, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "Id", "Extn", "Name", "OfficeId", "Position", "Salary", "StartDate" },
                values: new object[] { 51, 3990, "Cara Stevens", 1, "Sales Assistant", 145600m, new DateTime(2011, 12, 6, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "Id", "Extn", "Name", "OfficeId", "Position", "Salary", "StartDate" },
                values: new object[] { 52, 1016, "Hermione Butler", 1, "Regional Director", 356250m, new DateTime(2011, 3, 21, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "Id", "Extn", "Name", "OfficeId", "Position", "Salary", "StartDate" },
                values: new object[] { 55, 6373, "Shad Decker", 1, "Regional Director", 183000m, new DateTime(2008, 11, 13, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "Id", "Extn", "Name", "OfficeId", "Position", "Salary", "StartDate" },
                values: new object[] { 56, 5384, "Michael Bruce", 1, "Javascript Developer", 183000m, new DateTime(2011, 6, 27, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "Id", "Extn", "Name", "OfficeId", "Position", "Salary", "StartDate" },
                values: new object[] { 57, 4226, "Donna Snider", 1, "Customer Support", 112000m, new DateTime(2011, 1, 25, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "Id", "Extn", "Name", "OfficeId", "Position", "Salary", "StartDate" },
                values: new object[] { 3, 1562, "Ashton Cox", 2, "Junior Technical Author", 86000m, new DateTime(2009, 1, 12, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "Id", "Extn", "Name", "OfficeId", "Position", "Salary", "StartDate" },
                values: new object[] { 9, 2360, "Colleen Hurst", 2, "Javascript Developer", 205500m, new DateTime(2009, 9, 15, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "Id", "Extn", "Name", "OfficeId", "Position", "Salary", "StartDate" },
                values: new object[] { 10, 1667, "Sonya Frost", 2, "Software Engineer", 103600m, new DateTime(2008, 12, 13, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "Id", "Extn", "Name", "OfficeId", "Position", "Salary", "StartDate" },
                values: new object[] { 12, 9497, "Quinn Flynn", 2, "Support Lead", 342000m, new DateTime(2013, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "Id", "Extn", "Name", "OfficeId", "Position", "Salary", "StartDate" },
                values: new object[] { 13, 6741, "Charde Marshall", 2, "Regional Director", 470600m, new DateTime(2008, 10, 16, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "Id", "Extn", "Name", "OfficeId", "Position", "Salary", "StartDate" },
                values: new object[] { 30, 8899, "Shou Itou", 2, "Regional Marketing", 163000m, new DateTime(2011, 8, 14, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "Id", "Extn", "Name", "OfficeId", "Position", "Salary", "StartDate" },
                values: new object[] { 33, 3606, "Prescott Bartlett", 2, "Technical Author", 145000m, new DateTime(2011, 5, 7, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "Id", "Extn", "Name", "OfficeId", "Position", "Salary", "StartDate" },
                values: new object[] { 35, 8240, "Martena Mccray", 2, "Post-Sales support", 324050m, new DateTime(2011, 3, 9, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "Id", "Extn", "Name", "OfficeId", "Position", "Salary", "StartDate" },
                values: new object[] { 44, 9383, "Sakura Yamamoto", 2, "Support Engineer", 139575m, new DateTime(2009, 8, 19, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "Id", "Extn", "Name", "OfficeId", "Position", "Salary", "StartDate" },
                values: new object[] { 53, 6733, "Lael Greer", 2, "Systems Administrator", 103500m, new DateTime(2009, 2, 27, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "Id", "Extn", "Name", "OfficeId", "Position", "Salary", "StartDate" },
                values: new object[] { 54, 8196, "Jonas Alexander", 2, "Developer", 86500m, new DateTime(2010, 7, 14, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "Id", "Extn", "Name", "OfficeId", "Position", "Salary", "StartDate" },
                values: new object[] { 16, 1581, "Michael Silva", 3, "Marketing Designer", 198500m, new DateTime(2012, 11, 27, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "Id", "Extn", "Name", "OfficeId", "Position", "Salary", "StartDate" },
                values: new object[] { 18, 1721, "Gloria Little", 3, "Systems Administrator", 237500m, new DateTime(2009, 4, 10, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "Id", "Extn", "Name", "OfficeId", "Position", "Salary", "StartDate" },
                values: new object[] { 22, 6154, "Yuri Berry", 3, "Chief Marketing Officer (CMO)", 675000m, new DateTime(2009, 6, 25, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "Id", "Extn", "Name", "OfficeId", "Position", "Salary", "StartDate" },
                values: new object[] { 27, 9239, "Jennifer Chang", 3, "Regional Director", 357650m, new DateTime(2010, 11, 14, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "Id", "Extn", "Name", "OfficeId", "Position", "Salary", "StartDate" },
                values: new object[] { 45, 8327, "Thor Walton", 1, "Developer", 98540m, new DateTime(2013, 8, 11, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "Id", "Extn", "Name", "OfficeId", "Position", "Salary", "StartDate" },
                values: new object[] { 37, 7031, "Howard Hatfield", 3, "Office Manager", 164500m, new DateTime(2008, 12, 16, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "Id", "Extn", "Name", "OfficeId", "Position", "Salary", "StartDate" },
                values: new object[] { 43, 6222, "Bruno Nash", 1, "Software Engineer", 163500m, new DateTime(2011, 5, 3, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "Id", "Extn", "Name", "OfficeId", "Position", "Salary", "StartDate" },
                values: new object[] { 40, 7580, "Timothy Mooney", 1, "Office Manager", 136200m, new DateTime(2008, 12, 11, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "Id", "Extn", "Name", "OfficeId", "Position", "Salary", "StartDate" },
                values: new object[] { 2, 8422, "Garrett Winters", 1, "Accountant", 170750m, new DateTime(2011, 7, 25, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "Id", "Extn", "Name", "OfficeId", "Position", "Salary", "StartDate" },
                values: new object[] { 4, 6224, "Cedric Kelly", 1, "Senior Javascript Developer", 433060m, new DateTime(2012, 3, 29, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "Id", "Extn", "Name", "OfficeId", "Position", "Salary", "StartDate" },
                values: new object[] { 5, 5407, "Airi Satou", 1, "Accountant", 162700m, new DateTime(2008, 11, 28, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "Id", "Extn", "Name", "OfficeId", "Position", "Salary", "StartDate" },
                values: new object[] { 6, 4804, "Brielle Williamson", 1, "Integration Specialist", 372000m, new DateTime(2012, 12, 2, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "Id", "Extn", "Name", "OfficeId", "Position", "Salary", "StartDate" },
                values: new object[] { 7, 9608, "Herrod Chandler", 1, "Sales Assistant", 137500m, new DateTime(2012, 8, 6, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "Id", "Extn", "Name", "OfficeId", "Position", "Salary", "StartDate" },
                values: new object[] { 8, 6200, "Rhona Davidson", 1, "Integration Specialist", 327900m, new DateTime(2010, 10, 14, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "Id", "Extn", "Name", "OfficeId", "Position", "Salary", "StartDate" },
                values: new object[] { 11, 3814, "Jena Gaines", 1, "Office Manager", 90560m, new DateTime(2008, 12, 19, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "Id", "Extn", "Name", "OfficeId", "Position", "Salary", "StartDate" },
                values: new object[] { 14, 3597, "Haley Kennedy", 1, "Senior Marketing Designer", 313500m, new DateTime(2012, 12, 18, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "Id", "Extn", "Name", "OfficeId", "Position", "Salary", "StartDate" },
                values: new object[] { 15, 1965, "Tatyana Fitzpatrick", 1, "Regional Director", 385750m, new DateTime(2010, 3, 17, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "Id", "Extn", "Name", "OfficeId", "Position", "Salary", "StartDate" },
                values: new object[] { 17, 3059, "Paul Byrd", 1, "Chief Financial Officer (CFO)", 725000m, new DateTime(2010, 6, 9, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "Id", "Extn", "Name", "OfficeId", "Position", "Salary", "StartDate" },
                values: new object[] { 19, 2558, "Bradley Greer", 1, "Software Engineer", 132000m, new DateTime(2012, 10, 13, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "Id", "Extn", "Name", "OfficeId", "Position", "Salary", "StartDate" },
                values: new object[] { 20, 2290, "Dai Rios", 1, "Personnel Lead", 217500m, new DateTime(2012, 9, 26, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "Id", "Extn", "Name", "OfficeId", "Position", "Salary", "StartDate" },
                values: new object[] { 21, 1937, "Jenette Caldwell", 1, "Development Lead", 345000m, new DateTime(2011, 9, 3, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "Id", "Extn", "Name", "OfficeId", "Position", "Salary", "StartDate" },
                values: new object[] { 23, 8330, "Caesar Vance", 1, "Pre-Sales Support", 106450m, new DateTime(2011, 12, 12, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "Id", "Extn", "Name", "OfficeId", "Position", "Salary", "StartDate" },
                values: new object[] { 24, 3023, "Doris Wilder", 1, "Sales Assistant", 85600m, new DateTime(2010, 9, 20, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "Id", "Extn", "Name", "OfficeId", "Position", "Salary", "StartDate" },
                values: new object[] { 25, 5797, "Angelica Ramos", 1, "Chief Executive Officer (CEO)", 1200000m, new DateTime(2009, 10, 9, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "Id", "Extn", "Name", "OfficeId", "Position", "Salary", "StartDate" },
                values: new object[] { 26, 8822, "Gavin Joyce", 1, "Developer", 92575m, new DateTime(2010, 12, 22, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "Id", "Extn", "Name", "OfficeId", "Position", "Salary", "StartDate" },
                values: new object[] { 28, 1314, "Brenden Wagner", 1, "Software Engineer", 206850m, new DateTime(2011, 6, 7, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "Id", "Extn", "Name", "OfficeId", "Position", "Salary", "StartDate" },
                values: new object[] { 29, 2947, "Fiona Green", 1, "Chief Operating Officer (COO)", 850000m, new DateTime(2010, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "Id", "Extn", "Name", "OfficeId", "Position", "Salary", "StartDate" },
                values: new object[] { 31, 2769, "Michelle House", 1, "Integration Specialist", 95400m, new DateTime(2011, 6, 2, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "Id", "Extn", "Name", "OfficeId", "Position", "Salary", "StartDate" },
                values: new object[] { 32, 6832, "Suki Burks", 1, "Developer", 114500m, new DateTime(2009, 10, 22, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "Id", "Extn", "Name", "OfficeId", "Position", "Salary", "StartDate" },
                values: new object[] { 34, 2860, "Gavin Cortez", 1, "Team Leader", 235500m, new DateTime(2008, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "Id", "Extn", "Name", "OfficeId", "Position", "Salary", "StartDate" },
                values: new object[] { 36, 5384, "Unity Butler", 1, "Marketing Designer", 85675m, new DateTime(2009, 12, 9, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "Id", "Extn", "Name", "OfficeId", "Position", "Salary", "StartDate" },
                values: new object[] { 38, 6318, "Hope Fuentes", 1, "Secretary", 109850m, new DateTime(2010, 2, 12, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "Id", "Extn", "Name", "OfficeId", "Position", "Salary", "StartDate" },
                values: new object[] { 39, 9422, "Vivian Harrell", 1, "Financial Controller", 452500m, new DateTime(2009, 2, 14, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "Id", "Extn", "Name", "OfficeId", "Position", "Salary", "StartDate" },
                values: new object[] { 41, 1042, "Jackson Bradshaw", 1, "Director", 645750m, new DateTime(2008, 9, 26, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "Id", "Extn", "Name", "OfficeId", "Position", "Salary", "StartDate" },
                values: new object[] { 42, 2120, "Olivia Liang", 3, "Support Engineer", 234500m, new DateTime(2011, 2, 3, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.CreateIndex(
                name: "IX_Offices_AddressId",
                table: "Offices",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Persons_OfficeId",
                table: "Persons",
                column: "OfficeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Persons");

            migrationBuilder.DropTable(
                name: "Offices");

            migrationBuilder.DropTable(
                name: "Addresses");
        }
    }
}

namespace AspNetCore.MVC.Sample.Models
{
    public class Office
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Address Address { get; set; }

        public int AddressId { get; set; }

    }
}
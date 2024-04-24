namespace Task6.Models
{
    public class Product
    {
        //based on the db schema

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}
namespace Task6.Models
{
    public class ProductWarehouse
    {
        //based on the db schema
        public int IdProductWarehouse { get; set; }

        public int IdWarehouse { get; set; }
        public int IdProduct { get; set; }

        public int IdOrder { get; set; }
        public int Amount { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
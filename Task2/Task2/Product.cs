using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task2
{
    public class Product
    {
        public static List<Product> products = new List<Product>();

        public Product(string name, double temperature)
        {
            Name = name;
            Temperature = temperature;
        }

        public string Name { get; set; }
        public double Temperature { get; set; }

        public static void AddProduct(Product product)
        {
            products.Add(product);
        }

        public static void RemoveProduct(Product product)
        { products.Remove(product); }

        public static void initProducts()
        {
            products = new List<Product>();
            products.Add(new Product("Bananas", 13.3));
            products.Add(new Product("Chocolate", 18));
            products.Add(new Product("Fish", 2));
            products.Add(new Product("Meat", -15));
            products.Add(new Product("Ice cream", -18));
            products.Add(new Product("Frozen pizza", -30));
            products.Add(new Product("Cheese", 7.2));
            products.Add(new Product("Sausages", 5));
            products.Add(new Product("Butter", 20.5));
            products.Add(new Product("Eggs", 19));
        }

        public static Product findProductByName(string name)
        {
            foreach (Product product in products)
            {
                if (product.Name == name)
                {
                    return product;
                }
            }
            return null;
        }

        public static double getTemperatureByName(string name)
        {
            foreach (Product product in products)
            {
                if (product.Name == name)
                {
                    return product.Temperature;
                }
            }
            return -999;
        }
    }
}
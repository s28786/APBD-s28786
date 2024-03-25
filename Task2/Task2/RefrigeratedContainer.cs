namespace Task2
{
    public class RefrigeratedContainer : Container
    {
        public RefrigeratedContainer(double cargoMass, double height, double tareWeight, double depth, double maxPayload, string productType, double temperature)
            : base("C", cargoMass, height, tareWeight, depth, maxPayload)
        {
            if (temperature < Product.getTemperatureByName(productType) && Product.getTemperatureByName(productType) != -999)
            {
                throw new TemperatureTooLowException("Temperature is too low for this product.");
            }
            ProductType = productType;
            Temperature = temperature;
        }

        public string ProductType { get; set; }
        public double Temperature { get; set; }

        public override void EmptyCargo()
        {
            CargoMass = 0;
        }

        public override void LoadCargo(double mass)
        {
            if (mass > MaxPayload)
            {
                throw new OverfillException("Cargo mass exceeds maximum payload.");
            }
            CargoMass = mass;
        }

        public override void PrintInfo()

        {
            Console.WriteLine(getBasicInfo() + " - Product: " + ProductType + " - Temperature: " + Temperature);
        }

        public static void creationMenu()
        {
            try
            {
                Console.WriteLine("Enter cargo mass:");
                double cargoMass = double.Parse(Console.ReadLine());

                Console.WriteLine("Enter height:");
                double height = double.Parse(Console.ReadLine());

                Console.WriteLine("Enter tare weight:");
                double tareWeight = double.Parse(Console.ReadLine());

                Console.WriteLine("Enter depth:");
                double depth = double.Parse(Console.ReadLine());

                Console.WriteLine("Enter max payload:");
                double maxPayload = double.Parse(Console.ReadLine());

                Console.Write("Product Type: ");
                string productType = Console.ReadLine();

                Console.Write("Temperature: ");
                double temperature = double.Parse(Console.ReadLine());

                Container container = new RefrigeratedContainer(cargoMass, height, tareWeight, depth, maxPayload, productType, temperature);
                Console.WriteLine("Container created successfully.");
                container.PrintInfo();
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred: " + e.Message);
            }
        }
    }
}
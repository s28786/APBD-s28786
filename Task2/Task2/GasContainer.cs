namespace Task2
{
    public class GasContainer : Container, IHazardNotifier
    {
        public GasContainer(double cargoMass, double height, double tareWeight, double depth, double maxPayload)
            : base("G", cargoMass, height, tareWeight, depth, maxPayload)
        {
        }

        public void NotifyHazard(string message)
        {
            Console.WriteLine(message + " - Container: " + SerialNumber);
        }

        public override void LoadCargo(double mass)
        {
            if (CargoMass + mass > MaxPayload)
            {
                throw new OverfillException("Overfilling the container");
            }
            else
            {
                CargoMass += mass;
            }
        }

        public override void EmptyCargo()
        {
            CargoMass = CargoMass * 5 / 100;
        }

        public override void PrintInfo()
        {
            Console.WriteLine(getBasicInfo());
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

                Container container = new GasContainer(cargoMass, height, tareWeight, depth, maxPayload);
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
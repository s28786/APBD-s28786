namespace Task2
{
    public class LiquidContainer : Container, IHazardNotifier
    {
        private bool isHazardous;

        public LiquidContainer(double cargoMass, double height, double tareWeight, double depth, double maxPayload, bool isHazardous)
            : base("L", cargoMass, height, tareWeight, depth, maxPayload)
        {
            this.isHazardous = isHazardous;
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

            CargoMass += mass;
            if (CargoMass > MaxPayload * 0.5 && isHazardous)
            {
                NotifyHazard("Warning: Overfilling the 50% limit of Hazardous Liquid Cargo");
            }
            else if (CargoMass > MaxPayload * 0.9)
            {
                NotifyHazard("Warning: Overfilling the 90% limit of Liquid Cargo");
            }
        }

        public override void EmptyCargo()
        {
            CargoMass = 0;
        }

        public override void PrintInfo()

        {
            string hazardous = isHazardous ? "Yes" : "No";
            Console.WriteLine(getBasicInfo() + " - Harzadous: " + hazardous);
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

                Console.WriteLine("Is the container hazardous? (true/false):");
                bool isHazardous = bool.Parse(Console.ReadLine());

                Container container = new LiquidContainer(cargoMass, height, tareWeight, depth, maxPayload, isHazardous);
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
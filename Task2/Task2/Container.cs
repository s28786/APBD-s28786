namespace Task2
{
    public abstract class Container
    {
        protected Container(string conType, double cargoMass, double height, double tareWeight, double depth, double maxPayload)
        {
            MaxPayload = maxPayload;
            CargoMass = 0;
            SerialNumber = GenerateSerialNumber(conType);
            LoadCargo(cargoMass);
            Height = height;
            TareWeight = tareWeight;
            Depth = depth;

            ConType = conType;
            ContainerList.Add(this);
        }

        public static List<Container> ContainerList = new List<Container>();

        public string ConType { get; }

        private static int IdCounter = 0;
        public string SerialNumber { get; }
        public double CargoMass { get; protected set; }
        public double Height { get; }
        public double TareWeight { get; }
        public double Depth { get; }
        public double MaxPayload { get; }

        public abstract void LoadCargo(double mass);

        public abstract void EmptyCargo();

        public abstract void PrintInfo();

        public String getFullType()
        {
            if (ConType == "L")
            {
                return "Liquid";
            }
            else if (ConType == "G")
            {
                return "Gas";
            }
            else if (ConType == "C")
            {
                return "Refrigerated";
            }
            else
            {
                throw new System.ArgumentException("Invalid");
            }
        }

        private static string GenerateSerialNumber(string type)

        {
            IdCounter++;

            return "KON-" + type + "-" + IdCounter;
        }

        public string getBasicInfo()
        {
            return "Container: " + SerialNumber + " - Type: " + ConType + " - Height: " + Height + " - Tare Weight: " + TareWeight + " - Depth: " + Depth + " - Max Payload: " + MaxPayload;
        }

        public object Shallowcopy()
        {
            return this.MemberwiseClone();
        }

        public static void deleteContainer(string serialNumber)
        {
            ContainerList.RemoveAll(c => c.SerialNumber == serialNumber);
            foreach (ContainerShip s in ContainerShip.containerShips)
            {
                s.Containers.RemoveAll(c => c.SerialNumber == serialNumber);
            }
        }

        public static void printAllContainers(List<Container> listCont)
        {
            foreach (Container c in listCont)
            {
                c.PrintInfo();
            }
        }

        public static Container getContainerBySerialNumber(string serialNumber)
        {
            return ContainerList.Find(c => c.SerialNumber == serialNumber);
        }

        public static void LoadToShipMenu(Container container)
        {
            if (ContainerShip.checkInUseContainer(container) != null)
            {
                Console.WriteLine("Container is already in use");
                return;
            }
            try
            {
                Console.WriteLine("Enter ship name:");
                string name = Console.ReadLine();
                ContainerShip containerShip = ContainerShip.findContainerShipByName(name);
                containerShip.printInfo();

                Console.WriteLine("1. Add this container to ship");
                Console.WriteLine("2. Exit");
                Console.Write("Enter your choice: ");
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        try
                        {
                            containerShip.AddContainer(container);
                            Console.WriteLine("Container added to ship");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }

                        break;

                    case "2":
                        return;

                    default:
                        return;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
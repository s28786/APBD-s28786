using System.ComponentModel;

namespace Task2
{
    public class ContainerShip
    {
        public ContainerShip(string name, int maxSpeed, int maxContainerNumber, double maxWeight)

        {
            if (isNameTaken(name))
            {
                throw new Exception("Name is already taken.");
            }
            Name = name;
            MaxSpeed = maxSpeed;
            MaxContainerNumber = maxContainerNumber;
            MaxWeight = maxWeight;
            containerShips.Add(this);
        }

        public static List<ContainerShip> containerShips = new List<ContainerShip>();
        public List<Container> Containers { get; set; } = new List<Container>();
        public int MaxSpeed { get; }
        public int MaxContainerNumber { get; }
        public double MaxWeight { get; }
        public string Name { get; set; }

        public void AddContainer(Container container)
        {
            if (Containers.Count >= MaxContainerNumber)
            {
                throw new OverfillException("Container ship is full.");
            }
            else if (Containers.Sum(c => c.CargoMass) + container.CargoMass > MaxWeight)
            {
                throw new OverfillException("Container ship is too heavy.");
            }
            Containers.Add(container);
        }

        public double getMaxWeightInTons()
        {
            return MaxWeight / 1000;
        }

        public void removeContainer(Container container)
        {
            Containers.Remove(container);
        }

        public static ContainerShip findContainerShipByName(string name)
        {
            foreach (ContainerShip containerShip in containerShips)
            {
                if (containerShip.Name == name)
                {
                    return containerShip;
                }
            }
            return null;
        }

        public bool isNameTaken(string name)
        {
            foreach (ContainerShip containerShip in containerShips)
            {
                if (containerShip.Name == name)
                {
                    return true;
                }
            }
            return false;
        }

        public void printInfo()
        {
            Console.WriteLine("Ship: " + Name + " - Max Speed: " + MaxSpeed + " - Max Container Number: " + MaxContainerNumber + " - Max Weight: " + MaxWeight);
            Container.printAllContainers(this.Containers);
        }

        public static void printAllShips(List<ContainerShip> listShips)
        {
            foreach (ContainerShip ship in listShips)
            {
                ship.printInfo();
            }
        }

        public object Shallowcopy()
        {
            return this.MemberwiseClone();
        }

        public static void creationMenu()
        {
            try
            {
                Console.WriteLine("Enter ship name:");
                string name = Console.ReadLine();

                Console.WriteLine("Enter max speed:");
                int maxSpeed = int.Parse(Console.ReadLine());

                Console.WriteLine("Enter max container number:");
                int maxContainerNumber = int.Parse(Console.ReadLine());

                Console.WriteLine("Enter max weight:");
                double maxWeight = double.Parse(Console.ReadLine());

                ContainerShip containerShip = new ContainerShip(name, maxSpeed, maxContainerNumber, maxWeight);
                Console.WriteLine("Ship created successfully.");
                containerShip.printInfo();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static void deleteShip(string name)
        {
            containerShips.RemoveAll(s => s.Name == name);
        }

        public static ContainerShip checkInUseContainer(Container container)
        {
            for (int i = 0; i < containerShips.Count; i++)
            {
                if (containerShips[i].Containers.Contains(container))
                {
                    return containerShips[i];
                }
            }
            return null;
        }

        public void removeAContainerFromShip()
        {
            try
            {
                Console.WriteLine("Enter container serial number:");
                string serialNumber = Console.ReadLine();
                Container container = Container.getContainerBySerialNumber(serialNumber);
                removeContainer(container);
                Console.WriteLine("Container removed from ship");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static void LoadToShipMenu(ContainerShip containerShip)
        {
            try
            {
                Console.WriteLine("Enter container Id:");
                string id = Console.ReadLine();
                Container container = Container.getContainerBySerialNumber(id);
                container.PrintInfo();
                if (checkInUseContainer(container) != null)
                {
                    Console.WriteLine("Container is already in use");
                    return;
                }

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

        public double getSumContainerWeight()
        {
            double sum = 0;
            foreach (Container container in Containers)
            {
                sum += container.CargoMass;
            }
            return sum;
        }

        public void ReplaceAContainer()
        {
            try
            {
                Console.WriteLine("Enter container serial number to enter:");
                string serialNumber = Console.ReadLine();
                Console.WriteLine("Enter container serial number to remove:");
                string serialNumberTwo = Console.ReadLine();

                Container oldContainer = Container.getContainerBySerialNumber(serialNumberTwo);
                Container newContainer = Container.getContainerBySerialNumber(serialNumber);

                if (getSumContainerWeight() + newContainer.CargoMass - oldContainer.CargoMass > MaxWeight)
                {
                    throw new Exception("The ship is too heavy.");
                }

                removeContainer(oldContainer);
                AddContainer(newContainer);
                Console.WriteLine("Container replaced successfully.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void AddListContainers(List<Container> listContainers)

        {
            if (Containers.Count + listContainers.Count > MaxContainerNumber)
            {
                throw new Exception("The ship is full.");
            }
            foreach (Container container in listContainers)
            {
                try
                {
                    AddContainer(container);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }

    // Other methods - for loading cargo, removing container, etc.
}
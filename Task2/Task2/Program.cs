using Task2;

public class Program
{
    public static void Main(string[] args)
    {
        while (true)
        {
            Console.WriteLine("Welcome to the Shipping System!");
            Console.WriteLine("1. Create a container");
            Console.WriteLine("2. Find a container by ID");
            Console.WriteLine("3. Create a ship");
            Console.WriteLine("4. Find a ship by name");
            Console.WriteLine("5. List all containers");
            Console.WriteLine("6. List all ships");
            Console.WriteLine("7. Exit");

            Console.Write("Enter your choice: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    createContainerMenu();
                    break;

                case "2":
                    MainContainerMenu();

                    break;

                case "3":
                    createShipMenu();
                    break;

                case "4":
                    MainShipMenu();
                    break;

                case "5":
                    Console.WriteLine("Containers List");
                    Container.printAllContainers(Container.ContainerList);

                    break;

                case "6":
                    Console.WriteLine("Ships List");
                    ContainerShip.printAllShips(ContainerShip.containerShips);

                    break;

                case "7":
                    Console.WriteLine("Exiting...");
                    return;

                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }

            Console.WriteLine();
        }
    }

    public static void createContainerMenu()
    {
        Console.WriteLine("Creating a container");
        Console.WriteLine("1. Create a liquid container");
        Console.WriteLine("2. Create a gas container");
        Console.WriteLine("3. Create a refrigerated container");
        Console.WriteLine("4. Exit");

        Console.Write("Enter your choice: ");
        string choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                LiquidContainer.creationMenu();
                break;

            case "2":
                GasContainer.creationMenu();
                break;

            case "3":
                RefrigeratedContainer.creationMenu();
                break;

            case "4":
                Console.WriteLine("Exiting container creation menu...");
                return;

            default:
                Console.WriteLine("Invalid choice. Please try again.");
                break;
        }
    }

    public static void MainContainerMenu()
    {
        try
        {
            Console.Write("Enter your id: ");
            string choice = Console.ReadLine();
            Container container = Container.getContainerBySerialNumber(choice);

            if (container == null)
            {
                return;
            }
            container.PrintInfo();
            Console.WriteLine("Managing a container");
            Console.WriteLine("1. Add cargo");
            Console.WriteLine("2. Unload cargo");
            Console.WriteLine("3. Load this on a ship");
            Console.WriteLine("4. Print info of this containerr");
            Console.WriteLine("5. Exit");

            Console.Write("Enter your choice: ");
            choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.WriteLine("Enter adding amount:");
                    double amount = double.Parse(Console.ReadLine());
                    container.LoadCargo(amount);
                    break;

                case "2":
                    container.EmptyCargo();
                    break;

                case "3":
                    Container.LoadToShipMenu(container);
                    break;

                case "4":
                    container.PrintInfo();
                    break;

                case "5":
                    Console.WriteLine("Exiting container creation menu...");
                    return;

                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    public static void createShipMenu()
    {
        ContainerShip.creationMenu();
    }

    public static void MainShipMenu()
    {
        try
        {
            Console.Write("Enter your ship name: ");
            string choice = Console.ReadLine();
            ContainerShip containerShip = ContainerShip.findContainerShipByName(choice);
            if (containerShip == null)
            {
                return;
            }
            containerShip.printInfo();
            Console.WriteLine("Managing a ship");
            Console.WriteLine("1. Add a container");
            Console.WriteLine("2. Add a list of container");
            Console.WriteLine("3. Remove a container from the ship");
            Console.WriteLine("4. Delete this ship");
            Console.WriteLine("5. Replace a container");
            Console.WriteLine("6. Exit");

            Console.Write("Enter your choice: ");
            choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    ContainerShip.LoadToShipMenu(containerShip);
                    break;

                case "2":
                    Console.WriteLine("Enter the containers serial Num you want to add (separated by ;)");
                    string input = Console.ReadLine();
                    string[] serialNumbers = input.Split(";");
                    List<Container> containers = new List<Container>();
                    foreach (string serialNumber in serialNumbers)
                    {
                        Container container = Container.getContainerBySerialNumber(serialNumber);
                        containers.Add(container);
                    }
                    containerShip.AddListContainers(containers);
                    break;

                case "3":
                    containerShip.removeAContainerFromShip();
                    break;

                case "4":
                    ContainerShip.deleteShip(containerShip.Name);
                    break;

                case "5":
                    containerShip.ReplaceAContainer();
                    return;

                case "6":

                    return;

                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
}
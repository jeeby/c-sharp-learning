/// <summary>
/// Creating instances of the different vehicles
/// Using inheritance, it's very succinct as all the configuration is baked into the classes
/// </summary>
//Automobile sedan = new Sedan();
//Automobile coupe = new Coupe();
//Automobile pickupTruck = new PickupTruck();
//Automobile van = new Van();

public enum DoorPosition
{
    FrontDriverSide,
    FrontPassengerSide,
    RearDriverSide,
    RearPassengerSide
}

// Inheritance 

public abstract class Vehicle
{
    // Abstract, so can't be instantiated
}

public abstract class Automobile : Vehicle
{
    // Abstract functions must be implemented by classes inheriting this class
    public abstract void StartEngine();

    public abstract void OpenDoor(DoorPosition doorPosition);
}

public abstract class Car : Automobile
{
    // Still abstract class

    // Implementing this function, therefore doesn't need to be implemented in 
    //  classes inheriting from this one
    public override void StartEngine()
    {
        Console.WriteLine("Car starting engine. Vroom vroom.");
    }
}

public class Sedan : Car
{
    public override void OpenDoor(DoorPosition doorPosition)
    {
        Console.WriteLine($"Sedan opening {doorPosition} door!");
    }
}

public class Coupe : Car
{
    public override void OpenDoor(DoorPosition doorPosition)
    {
        // This is not great code as we are allowing this code to be executed when it is not valid
        if (doorPosition == DoorPosition.RearDriverSide || 
            doorPosition == DoorPosition.RearPassengerSide)
        {
            throw new InvalidOperationException("Coupes only have 2 doors!");
        }

        Console.WriteLine($"Coupe opening {doorPosition} door!");
    }
}

public class PickupTruck : Automobile
{

    // If you want to make a pickup truck with a smaller engine, you'd need to make a new class that overrides
    //  this one, just so you could have a different engine size

    // Alternative is to move the function to a parent class - see StartEngine in AutomobileAlternate class below
    public override void StartEngine()
    {
        Console.WriteLine("Truck starting big 4L engine!");
    }

    // Same if you want a pickup truck with more than 2 doors, need a new class (eg: PickupTruckWith4Doors) that
    //  derives from this one
    public override void OpenDoor(DoorPosition doorPosition)
    {
        // This is not great code as we are allowing this code to be executed when it is not valid
        if (doorPosition == DoorPosition.RearDriverSide || 
            doorPosition == DoorPosition.RearPassengerSide)
        {
            throw new InvalidOperationException("Trucks only have 2 doors!");
        }

        Console.WriteLine($"Truck opening {doorPosition} door!");

    }
}

// Following is an attempt to move some configuration into the base class so we can
//  try to reuse some code
public abstract class AutomobileAlternate : Vehicle
{
    private readonly string _engineType;

    protected AutomobileAlternate(string engineType)
    {
        _engineType = engineType;
    }

    // Abstract functions must be implemented by classes inheriting this class
    public void StartEngine()
    {
        StartEngine(_engineType);
    }

    public abstract void OpenDoor(DoorPosition doorPosition);

    // protected: only accessible in this class and child classes
    // Static as it's not tied to an instance - it's getting all the info it needs passed in
    protected static void StartEngine(string engineType)
    {
        Console.WriteLine($"Starting {engineType} engine!");
    }
}


public class Van : Automobile
{
    public override void StartEngine()
    {
        Console.WriteLine("Van starting big 4L engine!");
    }

    public override void OpenDoor(DoorPosition doorPosition)
    {
        // This is not great code as we are allowing this code to be executed when it is not valid
        if (doorPosition == DoorPosition.RearDriverSide)
        {
            throw new InvalidOperationException("Vans don't have a rear driver side door!");
        }
        
        if (doorPosition == DoorPosition.RearPassengerSide)
        {
            Console.WriteLine($"Van sliding open {doorPosition} door!");
        }
        else
        {
            Console.WriteLine($"Van swinging open {doorPosition} door!");
        }
        

    }
}
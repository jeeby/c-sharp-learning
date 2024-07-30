// Composition

/// <summary>
/// Creating instances of the different vehicles
/// More verbose than with inheritance as you need to provide the configuration
/// Can use patterns to make this code easier to manage, eg:
///   Factory Pattern
///   Builder Pattern
/// </summary>
public class ContainerForInstantiationCode
{
    ComposedVehicle composedSedan = new(
        new ConfigurableEngine(1.8f),
        new Dictionary<DoorPosition, IDoor>
        {
        { DoorPosition.FrontPassengerSide, new StandardSwingingDoor() },
        { DoorPosition.FrontDriverSide, new StandardSwingingDoor() },
        { DoorPosition.RearDriverSide, new StandardSwingingDoor() },
        { DoorPosition.RearPassengerSide, new StandardSwingingDoor() }
        });
    ComposedVehicle composedCoupe = new(
        new ConfigurableEngine(1.5f),
        new Dictionary<DoorPosition, IDoor>
        {
        { DoorPosition.FrontPassengerSide, new StandardSwingingDoor() },
        { DoorPosition.FrontDriverSide, new StandardSwingingDoor() }
        });
    ComposedVehicle composedPickupTruck = new(
        new V8Engine(),
        new Dictionary<DoorPosition, IDoor>
        {
        { DoorPosition.FrontPassengerSide, new StandardSwingingDoor() },
        { DoorPosition.FrontDriverSide, new StandardSwingingDoor() }
        });
    ComposedVehicle composedVan = new(
        new ConfigurableEngine(2.5f),
        new Dictionary<DoorPosition, IDoor>
        {
        { DoorPosition.FrontPassengerSide, new StandardSwingingDoor() },
        { DoorPosition.FrontDriverSide, new StandardSwingingDoor() },
        { DoorPosition.RearDriverSide, new SlidingDoor() },
        { DoorPosition.RearPassengerSide, new SlidingDoor() }
        });
}

///
/// Can use Inheritance and Composition together, eg:
/// 
public abstract class EngineSwapCoupe : Coupe
{
    private readonly IEngine _engine;

    protected EngineSwapCoupe(IEngine engine)
    {
        _engine = engine;
    }

    // Overriding the function in Coupe, and using the method from the Engine class instead
    public override void StartEngine()
    {
        _engine.Start();
    }
}

public interface IEngine
{
    void Start();
}

public class V8Engine : IEngine
{
    public void Start()
    {
        Console.WriteLine("Big ol' V8 engine starting!");
    }
}

public class ConfigurableEngine : IEngine
{
    private readonly float _displacementInLiters;

    public ConfigurableEngine(float displacementInLiters)
    {
        _displacementInLiters = displacementInLiters;
    }

    public void Start()
    {
        Console.WriteLine($"Starting {_displacementInLiters}L engine!");
    }
}

public interface IDoor
{
    void Open();
}

public class StandardSwingingDoor : IDoor
{
    public void Open()
    {
        Console.WriteLine($"Swinging door opening!");
    }
}

public class SlidingDoor : IDoor
{
    public void Open()
    {
        Console.WriteLine($"Sliding door opening!");
    }
}

public sealed class ComposedVehicle
{
    private readonly IEngine _engine;
    private readonly IReadOnlyDictionary<DoorPosition, IDoor> _doors;

    public ComposedVehicle(
        IEngine engine,
        Dictionary<DoorPosition, IDoor> doors)
    {
        _engine = engine;
        _doors = doors;
    }

    public void StartEngine()
    {
        // method that just calls another method is called a pass-through method
        _engine.Start();
    }

    public void OpenDoor(DoorPosition doorPosition)
    {
        // Dictionary lookup - this is encapsulated in this function
        if (!_doors.TryGetValue(doorPosition, out IDoor? door))
        {
            throw new InvalidOperationException($"There is no door at position {doorPosition}");
        }

        Console.WriteLine($"Opening {doorPosition} door..");
        door.Open();
    }
}
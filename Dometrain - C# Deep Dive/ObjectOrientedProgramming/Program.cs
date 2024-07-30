// Creating instances of the different vehicles
Automobile sedan = new Sedan();
Automobile coupe = new Coupe();
Automobile pickupTruck = new PickupTruck();
Automobile van = new Van();

sedan.OpenDoor(DoorPosition.RearPassengerSide);
van.StartEngine();

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

composedSedan.OpenDoor(DoorPosition.RearPassengerSide);
composedPickupTruck.StartEngine();


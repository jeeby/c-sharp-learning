public sealed class InheritanceVehicleExample
{
    public void RunExample()
    {
        Car sedan = new() { DoorCount = 4 };
        Car coupe = new() { DoorCount = 2 };
        Truck pickupTruck = new() { DoorCount = 2 };
        Bike Bike = new();

        sedan.OpenDoors();
        coupe.OpenDoors();
        pickupTruck.OpenDoors();
        Bike.OpenDoors();
    }

    public class Vehicle
    {
        public int DoorCount { get; init; }

        public void OpenDoors()
        {
            Console.WriteLine(
                $"{GetType().Name} opening {DoorCount} doors!");
        }

        // Be careful about putting too much in base classes.  
        // You could, in theory, use GetType() to determine what type you're in, and act differently,
        //  but then you're making the base class have knowledge of the children which is not good.
        // Better to put the code on the children where possible.
    }

    public class Automobile : Vehicle
    {
    }

    public class Car : Automobile
    {
    }

    public class Truck : Automobile
    {
    }

    public class Van : Automobile
    {
    }

    public class Bike : Vehicle
    {
        public Bike()
        {
            DoorCount = 0;
        }
    }

    public class Plane : Vehicle
    {
    }
}
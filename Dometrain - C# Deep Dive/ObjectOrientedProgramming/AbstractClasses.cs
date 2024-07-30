public sealed class AbstractClasses
{
    // abstract classes in C# are the classes that:
    // - cannot be instantiated
    // - can define some base/shared functionality.
    // - can have abstract methods 
    // Abstract methods are *sort* of like what we see with interfaces
    // but we'll dive into this in more detail.

    // let's see how abstract classes look with the
    // concept of inheritance that we've seen so far.

    // NOTE: Putting code that you want shared across multiple classes in an abstract class
    //  is not necessarily the best option. Check Composition for a better solution.
    // It may make sense sometimes, but not always.

    public void RunExample()
    {
        // note that we cannot instantiate an abstract class!
        // MyBaseClass myBaseClass = new MyBaseClass(); // does not compile!

        // let's go run a quick example of this:
        MyDerivedClass myDerivedClass = new MyDerivedClass();
        myDerivedClass.Print();
        myDerivedClass.PrintAbstract();
    }

    abstract class MyBaseClass
    {
        public void Print()
        {
            Console.WriteLine("Print() in MyBaseClass");
        }

        // Abstract methods must be implemented (like an interface)
        public abstract void PrintAbstract();
    }

    // instead, we can create a derived class from it
    class MyDerivedClass : MyBaseClass
    {
        // we *MUST* implement the abstract method
        // when we derive from an abstract class!
        public override void PrintAbstract()
        {
            Console.WriteLine("PrintAbstract() in MyDerivedClass");
        }
    }

    // abstract classes can also inherit from interfaces AND
    // other abstract classes. Let's see an example of this:
    interface IMyInterface
    {
        void PrintInterface();
    }

    abstract class MyDerivedAbstractClass :
        MyBaseClass,
        IMyInterface
    {
        // Need to implement the interface's methods here,
        //  but can do it abstract so it's implemented further downstream
        //  or concrete so it's implemented here

        // abstract:
        public abstract void PrintInterface();

        // concrete:
        //public void PrintInterface()
        //{
        //    Console.WriteLine("PrintInterface() in MyDerivedAbstractClass");
        //}
    }

    class MyDerivedClass2 : MyDerivedAbstractClass
    {
        // note that we must implement BOTH the abstract methods!
        public override void PrintAbstract()
        {
            Console.WriteLine("PrintAbstract() in MyDerivedAbstractClass2");
        }

        public override void PrintInterface()
        {
            Console.WriteLine("PrintInterface() in MyDerivedAbstractClass2");
        }
    }


}
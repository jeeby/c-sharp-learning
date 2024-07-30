public sealed class ProtectedAndVirtual
{
    public void RunExample()
    {
        // what if we don't want our base class
        // to have methods that EVERYONE can use?
        // what if we want to limit it?
        // ... protected!

        DerivedClass derivedClass = new DerivedClass();
        derivedClass.PrintInDerivedClass(); // only function we can access, all the others are protected

        DerivedClass2 derivedClass2 = new DerivedClass2();
        derivedClass2.PrintInDerivedClass();
        derivedClass2.VirtualPrintInBaseClass();
    }

    class BaseClass2
    {
        protected void PrintInBaseClass()
        {
            Console.WriteLine("PrintInBaseClass");
        }

        // we can also use the virtual keyword to give us
        // a hybrid between abstract and non-abstract.
        // ... what does that even mean?!

        // Basically, virtual provides and implementation but gives the option to override
        //  it in derived classes if you want to.
        // Could be public or protected, but not private as it wouldn't be possible to override it
        public virtual void VirtualPrintInBaseClass()
        {
            Console.WriteLine("VirtualPrintInBaseClass");
        }
    }

    class DerivedClass2 : BaseClass2
    {
        public void PrintInDerivedClass()
        {
            Console.WriteLine("PrintInDerivedClass... then base!");
            base.PrintInBaseClass(); // base. is redundant here, but allows access to base functions
        }

        public override void VirtualPrintInBaseClass()
        {
            Console.WriteLine("VirtualPrintInBaseClass in the derived class");
            Console.WriteLine("... and now we'll call the base class method!");
            base.VirtualPrintInBaseClass(); // can call base implementation as well as local one
        }
    }

    abstract class AbstractBaseClass
    {
        // Can access this from the derived class, but can't from outside
        protected void ProtectedPrintInBaseClass()
        {
            Console.WriteLine("ProtectedPrintInBaseClass");
        }

        protected abstract void ProtectedAbstractPrint();
    }

    class DerivedClass : AbstractBaseClass
    {
        public void PrintInDerivedClass()
        {
            Console.WriteLine("We're in the derived class!");
            ProtectedPrintInBaseClass(); // Can access this as it's in the base class
            Console.WriteLine("We're leaving the method in the derived class!");
        }

        // Can't access this from outside this class - only accessible to current class, or classes 
        //  derived from this one
        protected override void ProtectedAbstractPrint()
        {
            Console.WriteLine("ProtectedAbstractPrint");
        }
    }
}
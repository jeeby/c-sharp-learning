﻿public sealed class CallbacksAndDelegates
{
    public void RunExample()
    {
        // we may write code that when something is complete,
        // we'd like something we specify to get run
        // this is called a callback!
        // we'll look at passing around methods
        // as parameters and storing them as variables

        // we have a type called a "delegate" which allows
        // us to define a method signature

        // the most basic form is "Action", so let's store 
        // a method into an Action variable
        // Actions need to be void return functions
        Action action = NicksAction;

        // now we can call the method by invoking the variable:
        action();
        action.Invoke(); // either way works!

        void NicksAction()
        {
            Console.WriteLine("Hello from Nick!");
        }

        // if you want to define a function, we can use the "Func" type:
        // the very last type parameter provided is the return type!  <--------- IMPORTANT
        Func<int, int, int> addFunction = AddFunction;
        Func<int, int, int> subtractFunction = SubtractFunction;
        Func<int, int, string> subtractFunctionAndReturnString = SubtractSubtractFunctionAndReturnStringFunction;

        int AddFunction(int a, int b)
        {
            return a + b;
        }

        int SubtractFunction(int a, int b)
        {
            return a - b;
        }
        
        string SubtractSubtractFunctionAndReturnStringFunction(int a, int b)
        {
            return (a - b).ToString();
        }

        // so how does this work with callbacks?
        // we can pass a method as a parameter to another method!

        void DoSomethingAfterUserPressesEnter(Action callback)
        {
            Console.WriteLine("Press enter for a surprise!");
            Console.ReadLine();
            callback();
        }

        // The following won't execute NicksAction until the callback() is executed 
        //  in the DoSomethingAfterUserPressesEnter function
        DoSomethingAfterUserPressesEnter(NicksAction);

        // you may want to do this in situations where
        // you need to control what is executed after
        // a certain event occurs or condition is met
        // - after a file is downloaded
        // - when a user clicks a button
        // - when a monitor detects an issue


        // ============================== AN EXAMPLE OF FUNC DELEGATES ============================

        // we can also pass a function as a parameter:
        void Calculate(Func<int, int, int> calculateCallback)
        {
            Console.WriteLine("Enter the first integer: ");
            int a = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter the second integer: ");
            int b = int.Parse(Console.ReadLine());

            int result = calculateCallback(a, b);
            Console.WriteLine($"The result is: {result}");
        }

        Console.WriteLine("Addition Example:");
        Calculate(addFunction);
        Console.WriteLine("Subtraction Example:");
        Calculate(subtractFunction);

        // - Action is simple
        // - Func is convenient but doesn't help readability
        // - we have Predicate which is essentially Func<T, bool>
        //   Predicates are useful for things like filtering
        //     (does this thing match the criteria you're passing in?)

         
        // But what if we want our own? Can we get more readability?
        // We can create a Delegate (see code at bottom of class)
        //  - Enables intellisense for parameters
        CalculateDelegate addDelegate = AddFunction;
        CalculateDelegate subtractDelegate = SubtractFunction;

        void Calculate2(CalculateDelegate calculateCallback)
        {
            Console.WriteLine("Enter the first integer: ");
            int a = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter the second integer: ");
            int b = int.Parse(Console.ReadLine());

            // check the intellisense to see that we can
            // get the names clearly provided now!
            int result = calculateCallback(a, b);
            Console.WriteLine($"The result is: {result}");
        }
    }

    // Can be void (like an action) 
    delegate int CalculateDelegate(
        int firstNumber,
        int secondNumber);
}
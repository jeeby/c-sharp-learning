public class AsyncAwait
{
    public async Task RunExample()
    {
        // like using Task objects, we can use the async/await keywords to
        // structure async code without having to think about it in terms
        // of objects

        #region Using Async/Await

        // in order to make an async method, we use a new keyword (async)
        // and the Task object as the return type
        async Task FirstAsyncMethod()
        {
            await Task.Delay(TimeSpan.FromSeconds(1));
            // write code that is async here!
        }

        // if we need to return anything, we use Task<T>, the generic
        // version, to be able to pass back data:
        async Task<int> SecondAsyncMethod()
        {
            await Task.Delay(TimeSpan.FromSeconds(1));
            return 42; // async keyword enables us to return a value and have it convert to a task for us
        }

        // much like the Task objects, we can call these async methods
        // and they'll go off and run... but we should track them!
        // we can use the await keyword to wait for the async method.

        // Note that other code can be executed by the scheduler while we are awaiting
        //  a task, allowing other tasks to run concurrently. Await tells our code to 
        //  wait for the task to finish so we can continue with our code.

        // within our context, we will not run code after the await
        // until the async method has completed.
        Console.WriteLine("awaiting FirstAsyncMethod...");
        await FirstAsyncMethod();

        // alternatively...
        Console.WriteLine("awaiting FirstAsyncMethod again...");
        Task firstAsyncMethodTask = FirstAsyncMethod(); // just storing the task for later use
        await firstAsyncMethodTask;

        #endregion

        #region Running several async methods

        // like our task examples, we can run several async methods
        async Task<string> ThirdAsyncMethod(
            TimeSpan timeToWait,
            string messageToWrite)
        {
            await Task.Delay(timeToWait);
            Console.WriteLine(messageToWrite);

            return messageToWrite;
        }

        Console.WriteLine("Starting 3 async methods...");
        Task<string> task1 = ThirdAsyncMethod(
            TimeSpan.FromSeconds(1),
            "Task 1 has completed.");
        Task<string> task2 = ThirdAsyncMethod(
            TimeSpan.FromSeconds(2),
            "Task 2 has completed.");
        Task<string> task3 = ThirdAsyncMethod(
            TimeSpan.FromSeconds(3),
            "Task 3 has completed.");

        // and we can wait for them all to complete:
        Console.WriteLine("Waiting for 3 async methods...");
        await Task.WhenAll(task1, task2, task3);
        Console.WriteLine("All 3 async methods have completed.");

        // Resulting execution is each console output occurs in order, 1 second apart.
        // This is because they all started at the same time and ran concurrently.

        // alternatively, we could wait until any of them completes:
        Task<string> firstTaskToComplete = await Task.WhenAny(task1, task2, task3);

        #endregion

        #region Not Always Async

        // let's look at this interesting behavior to understand
        // that marking something async doesn't just make it
        // automatically run asynchronously
        async Task NotActuallyAsync()
        {
            Console.WriteLine("Entering NotActuallyAsync...");
            Thread.Sleep(1000); // instead of awaiting on Task.Delay
            Console.WriteLine("Exiting NotActuallyAsync...");
        }

        // we can call this method and await it, but it will not
        // actually run asynchronously
        Console.WriteLine("Calling NotActuallyAsync...");
        Task notActuallyAsyncTask = NotActuallyAsync();
        Console.WriteLine("awaiting NotActuallyAsync..."); // should see this before 'Exiting..'
        await notActuallyAsyncTask;
        Console.WriteLine("Finished awaiting NotActuallyAsync.");

        // Result:  Calling -> Entering -> Exiting -> awaiting -> Finished
        // Didn't come back out of the function to awaiting, it just fully executed the function
        // This is because there was no async code to await on NotActuallyAsync

        #endregion

        #region  Leveraging Task.Yield

        // Using await Task.Yield enables the scheduler to give other tasks a chance to run

        async Task LeverageTaskYield()
        {
            Console.WriteLine("Entering LeverageTaskYield...");
            await Task.Yield(); // Give control back to scheduler to run other code
            Console.WriteLine("Continuing from LeverageTaskYield...");
            Thread.Sleep(1000);
            Console.WriteLine("Exiting LeverageTaskYield...");
        }

        // we can call this method and await it, and it will
        // at least allow the scheduler to run other tasks
        // thanks to the call to yield
        Console.WriteLine("Calling LeverageTaskYield...");
        Task leverageTaskYieldTask = LeverageTaskYield();
        Console.WriteLine("awaiting LeverageTaskYield...");
        await leverageTaskYieldTask;
        Console.WriteLine("Finished awaiting LeverageTaskYield.");

        // Result:  Calling-> Entering -> awaiting -> Continuing -> Exiting -> Finished
        // In this case, the await allows the scheduler to give other tasks a chance to run,
        //  so the awaiting.. code executes before the continuation from LeverageTaskYield...

        #endregion

        #region Catching Exceptions

        // it's important to note that once you introduce async/await
        // into the call tree, you should use it all the way up/down
        // let's look at what happens to our exception handling
        // when you mix async and non-async code
        async Task TestCatchingExceptions()
        {
            // we can await in here because it's marked as async
            Console.WriteLine("TestCatchingExceptions ThisIsNotATask...");
            await Task.Delay(TimeSpan.FromSeconds(1));
            Console.WriteLine("Finished delay inside TestCatchingExceptions...");

            Console.WriteLine("Calling async method...");
            try
            {
                await ThisIsATask(); // throws an exception
                // Result: Everything executes in the right order, and the exception is caught and handled

                // Alternative - calling non-task method:
                // We *can't* await this because this method is async but does not return a Task.
                //   We can only await on async Task methods!
                // await ThisIsNotATask(); - does not compile

                // The following compiles but breaks the async/await pattern as we're running code
                //  synchronously from within an async/awaited task
                ThisIsNotATask();

                // Result: Everything executes in right order until the await Task.Delay() call
                //  This does not execute, nor is the next console logged or the exception thrown.
                //  Code did not wait on execution to complete.
                //  If we put a Console.ReadLine() after the await TestCatchingExceptions() call,
                //   we'll see that an unhandled exception was thrown and the program crashed.
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Caught exception from async method: {ex.Message}");
            }
        }

        async Task ThisIsATask()
        {
            // we can await in here because it's marked as async
            Console.WriteLine("Entering ThisIsATask...");
            await Task.Delay(TimeSpan.FromSeconds(1));
            Console.WriteLine("Finished delay inside ThisIsATask...");

            throw new Exception("ThisIsATask has thrown an exception!");
        }

        // Read above why you should not use asyc void in async methods
        // Exceptions are event handlers, or when signatures for methods don't line up to use tasks
        //  eg: if you don't have control over that
        // Recommended to always wrap code within an async void method to prevent unhandled exceptions
        //  crashing your program!
        async void ThisIsNotATask()
        {
            // we can await in here because it's marked as async
            Console.WriteLine("Entering ThisIsNotATask...");
            await Task.Delay(TimeSpan.FromSeconds(1));
            Console.WriteLine("Finished delay inside ThisIsNotATask...");

            throw new Exception("ThisIsNotATask has thrown an exception!");
        }

        await TestCatchingExceptions();

        #endregion
    }
}
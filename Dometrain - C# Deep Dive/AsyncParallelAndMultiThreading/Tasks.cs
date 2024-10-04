public class Tasks
{
    public void RunExample()
    {
        // Tasks in C# allow us to perform asynchronous operations.
        // Using Task objects, we can get more control over
        // how we'd like our asynchronous operations to be executed.

        #region Basic Task Creation

        // Let's run some tasks and see which threads they are executed on:
        Console.WriteLine($"Main Thread Id: {Thread.CurrentThread.ManagedThreadId}");

        Task task1 = Task.Run(() =>
        {
            Console.WriteLine($"Task 1 Thread Id: {Thread.CurrentThread.ManagedThreadId}");
        });

        Task task2 = Task.Run(() =>
        {
            Console.WriteLine($"Task 2 Thread Id: {Thread.CurrentThread.ManagedThreadId}");
        });

        Task task3 = Task.Run(() =>
        {
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine($"Task 3 Thread Id: {Thread.CurrentThread.ManagedThreadId} ({i})");
                Thread.Sleep(1000);
            }
        });
        
        // something is weird with this output! why did task3 not perform its iterations as we'd expect?
        //  Because tasks run as background threads, and the main thread completed before the for loop completed.

        #endregion

        #region Task.WaitAll and Task.WaitAny
        
        // To get around this, we should wait for the tasks to complete before allowing continued execution:
        Task.WaitAll(task1, task2, task3);
        Console.WriteLine("Tasks 1, 2, and 3 have completed.");        

        // Another option is Task.WaitAny(task1, task2, task3), which waits for any of the tasks to complete 
        //  and returns the completed task.
        
        // we can even wait for all three tasks to complete before we start a 4th task, 
        //  which we will also wait on
        Task task4 = Task.Run(() =>
        {
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine($"Task 4 Thread Id: {Thread.CurrentThread.ManagedThreadId} ({i})");
                Thread.Sleep(500);
            }
        });

        // Can use a wait operation directly on the task
        task4.Wait();
        Console.WriteLine("Task 4 has completed.");

        #endregion
                

        #region Builder Pattern

        // we can also use the "builder pattern" to chain things together on task objects.
        // Here, we chain our tasks together using the ContinueWith method, which has access 
        //  to the previous task's result (via the prevTask property).
        Task task5 = Task.Run(() =>
        {
            Console.WriteLine($"Task 5 Thread Id: {Thread.CurrentThread.ManagedThreadId}");
        }).ContinueWith((prevTask) =>
        {
            // This will run on the same thread as the previous task
            Console.WriteLine($"Task 5 Continuation Thread Id: {Thread.CurrentThread.ManagedThreadId}");
        });
        task5.Wait();
        
        #endregion

        #region Builder Pattern with Exception Handling

        // We can also handle exceptions that occur during the execution of our tasks, and handle them properly
        //  like a try catch.
        // In this example, we will throw an exception in the first task, and catch it in the continuation task.
        // The continuation task will catch the exception and print it out, but only if the first task threw an exception.
        //  (TaskContinuationOptions.OnlyOnFaulted) will ensure that the continuation task only runs if the first task throws an exception).
        Task task6 = Task.Run(() =>
        {
            Console.WriteLine($"Task 5 Continuation Thread Id: {Thread.CurrentThread.ManagedThreadId}");
            throw new Exception("We intended to do this!");
        }).ContinueWith((prevTask) =>
        {
            Console.WriteLine($"Task 5 Continuation 2 Thread Id: {Thread.CurrentThread.ManagedThreadId}");
            Console.WriteLine($"{prevTask.Exception.GetType().Name}: {prevTask.Exception.Message}");
        }, TaskContinuationOptions.OnlyOnFaulted);
        
        #endregion

        #region Aggregage Exceptions

        // Aggregate exceptions are a way to handle multiple exceptions that can occur when working with tasks
        // They allow you to pass in multiple innner exceptions.
        AggregateException aggregateException = new(
            "This is the aggregage exception message.",
            new InvalidOperationException("This is the first inner exception."),
            new ArgumentException("This is the second inner exception."));

        try
        {
            throw aggregateException;
        }
        catch (AggregateException ex)
        {
            Console.WriteLine($"{ex.GetType().Name}: {ex.Message}");
            foreach (Exception innerEx in ex.InnerExceptions)
            {
                Console.WriteLine($"\t{innerEx.GetType().Name}: {innerEx.Message}");
            }
        }
        
        #endregion
    }
}
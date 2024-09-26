public class Threads
{
    public void RunExample()
    {
        // Thread objects in C# allow us to create and manage threads.
        // Pass in anonymous delegate
        Thread thread = new Thread(() =>
        {
            // do stuff
        });
        thread.Start();

        #region Pass Parameters into Threads

        // We can also pass parameters to the thread
        // Good practice to pass parameters in rather than having threads access external variables

        // Following is a custom type declared at end of the file
        ThreadContext thread1Context = new(
            Name: "Thread 1",
            Message: "Hello from thread 1!");

        // Still an anonymous delegate, but this one takes in 1 parameter (o),
        //  which is passed in when we start the thread
        Thread thread1 = new Thread(new ParameterizedThreadStart(o =>
        {
            // Need to cast as the object received by this function is a generic object
            ThreadContext context = (ThreadContext)o;

            Thread.CurrentThread.Name = context.Name;
            Console.WriteLine($"{Thread.CurrentThread.Name}: {context.Message}"); // Result:  Thread 1 : Hello from thread 1!
        }));

        // Start the thread and pass in the threadContext object
        thread1.Start(thread1Context);

        #endregion

        #region Running in Background

        // threads can be useful for running work in the background for us

        // Thread 2 example: runs in foreground, and continues forever
        ThreadContext thread2Context = new(
            Name: "Thread 2",
            Message: "Hello from thread 2!");

        Thread thread2 = new Thread(new ParameterizedThreadStart(o =>
        {
            ThreadContext context = (ThreadContext)o;

            Thread.CurrentThread.Name = context.Name;

            while (true)
            {
                Console.WriteLine($"{Thread.CurrentThread.Name}: {context.Message}");
                Thread.Sleep(1000);
            }
        }));
        thread2.Start(thread2Context);



        // Thread 3 example: Background Thread

        // we can also set a thread to be a background thread
        // which will automatically stop when the main thread stops
        ThreadContext thread3Context = new(
            Name: "Thread 3",
            Message: "Hello from thread 3!");
        Thread thread3 = new Thread(new ParameterizedThreadStart(o =>
        {
            ThreadContext context = (ThreadContext)o;

            Thread.CurrentThread.Name = context.Name;

            while (true)
            {
                Console.WriteLine($"{Thread.CurrentThread.Name}: {context.Message}");
                Thread.Sleep(1000);
            }
        }));
        thread3.IsBackground = true; // This is how we mark a thread as a background thread
        thread3.Start(thread3Context);

        Console.WriteLine("Press enter to stop Thread3.");
        Console.ReadLine(); // program will exit when a button is pressed, as the thread is marked as background

        // Can also cancel tokens, see Cancelation Tokens

        #endregion
    }

    record ThreadContext(
    string Name,
    string Message);
}
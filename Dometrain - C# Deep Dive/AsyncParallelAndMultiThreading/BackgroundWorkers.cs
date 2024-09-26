using System.ComponentModel;

public class BackgroundWorkers
{
    public void RunExample()
    {
        // we can use a BackgroundWorker to run a method in the background
        // and historically this was used a lot in WinForms applications

        #region Basic Example

        // here's how we'd create a new BackgroundWorker
        BackgroundWorker worker1 = new BackgroundWorker();

        // we can then subscribe to the DoWork event (passing in an anonymous (or non) method)
        worker1.DoWork += (object sender, DoWorkEventArgs e) =>
        {
            // all of this code is what's run in the background
            while (true)
            {
                Console.WriteLine("Worker 1: Working in the background...");
                Thread.Sleep(1000);
            }

            // The following will never be called
            Console.WriteLine("Worker 1: DoWork has completed.");
        };
        worker1.RunWorkerAsync();

        // Needs the other executing code at the end of the RunExample method,
        //  otherwise it'll just terminate immediately

        #endregion

        #region Passing Parameters

        // like with threads, we can pass parameters into the background worker
        // when we start it
        BackgroundWorker worker2 = new BackgroundWorker();
        worker2.DoWork += (sender, e) =>
        {
            int iterations = (int)e.Argument;
            for (int i = 0; i < iterations; i++)
            {
                Console.WriteLine($"Worker 2: Working in the background on iteration {i}...");
                Thread.Sleep(1000);
            }

            Console.WriteLine("Worker 2: DoWork has completed.");
        };
        worker2.RunWorkerAsync(5);
        
        #endregion

        #region Handling Completion of Events

        // we can also subscribe to the RunWorkerCompleted event
        worker2.RunWorkerCompleted += (sender, e) =>
        {
            Console.WriteLine("Background Worker 2 has completed.");
        };

        #endregion

        #region Cancellation of Events

        // we can cancel the background worker too! let's create a new
        //  cancellable worker (worker3), and modify
        // worker2 to cancel worker3 when it finishes.

        BackgroundWorker worker3 = new BackgroundWorker();
        worker3.WorkerSupportsCancellation = true;

        worker3.DoWork += (sender, e) =>    
        {
            while (!worker3.CancellationPending)
            {
                Console.WriteLine("Worker 3: Working in the background...");
                Thread.Sleep(1000); // Note cancellation will not stop this sleep execution
            }
        }

        // Now we can make worker2 cancel worker3 when it finishes
        worker2.RunWorkerCompleted += (sender, e) =>
        {
            Console.WriteLine("Background Worker 2 has completed.");
            worker3.CancelAsync();
        };

        #endregion

        Console.WriteLine("Press enter to exit.");
        Console.ReadLine();
    }
}
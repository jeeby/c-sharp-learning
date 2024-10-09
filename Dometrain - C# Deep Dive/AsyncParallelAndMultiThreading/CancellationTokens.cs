public class CancellationTokens
{
    public async Task RunExample()
    {
        // we can use cancellation tokens with our async/await code
        // to cancel tasks that are running:

        // It's recommended to always pass in a CancellationToken to all async/await methods

        #region Using Cacenllatoin Tokens

        // we can get a token from a CancellationTokenSource:
        CancellationTokenSource cts = new CancellationTokenSource();
        var cancellationToken = cts.Token;

        // Pass the token in to the task
        async Task LoopUntilCancelledAsync(
            CancellationToken cancellationToken)
        {
            await Task.Yield();
            Console.WriteLine("Looping until cancelled...");

            // Requesting cancellation is the only way this loop will end
            while (!cancellationToken.IsCancellationRequested)
            {
                Console.WriteLine("Waiting...");

                // Pass cancellationToken to Tasks so they can be canceled
                await Task.Delay(3000, cancellationToken);
                // This wil throw an exception when the cancellatoinToken is canceled   
                // Therefore, need to handle the exception when the cancellation token is canceled             
                //  if you need to handle it gracefully. Otherwise, let it bubble up and handle it somewhere else
                
                try
                {
                    await Task.Delay(3000, cancellationToken);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                
            }

            // Alternative way to handle cancellation (throws an exception):
            // Not as good as using exceptions for control flow is not recommended
            cancellationToken.ThrowIfCancellationRequested();

            Console.WriteLine("Cancelled.");
        }

        Console.WriteLine("Press enter to cancel the loop.");
        Task loopTask = LoopUntilCancelledAsync(cancellationToken);

        Console.ReadLine();

        // When a value is entered, we cancel the loop.
        cts.Cancel();

        // Make sure the loopTask is finally exited before we leave the program
        await loopTask;

        #endregion

        #region Chaining Cancellation Tokens

        // we can chain cancellation tokens together:
        CancellationTokenSource cts2 = new CancellationTokenSource();
        var cancellationToken2 = cts2.Token;
        var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken2);
        var linkedToken = linkedTokenSource.Token;

        Console.WriteLine("Using a linked token source!");
        Console.WriteLine("Press enter to cancel the loop.");
        loopTask = LoopUntilCancelledAsync(linkedToken);

        Console.ReadLine();
        cts2.Cancel();

        await loopTask;

        #endregion
    }
}
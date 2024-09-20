public sealed class Laziness
{
    public void RunExample()
    {
        // Lazy<T> is a generic type that we have in C#
        // that allows us to defer the creation of a value.
        // It also acts like a singleton, without being
        // global. It's a thread-safe (no race conditions)
        // way to create a value only when it's needed.

        // Useful when you start an application but don't need your
        // code to be available immediately, allows deferral of the execution
        // until it's first required

        Lazy<int> lazyValue = new Lazy<int>(() => // This is an inline function declaration (anonymous delegate)
        {
            // Method content could be anything, this is just an example that takes some time for demo purposes

            Console.WriteLine("This will only run once.");
            Console.WriteLine("Finding the max...");
            int[] numbers = [35, 20, 30, 40, 50];

            int max = int.MinValue;
            foreach (var number in numbers)
            {
                if (number > max)
                {
                    max = number;
                }

                // pretend this is an expensive operation
                Thread.Sleep(1000);
            }

            Console.WriteLine("The max value is: " + max); // Displays after 5 seconds
            return max;
        });

        Console.WriteLine("The value of lazyValue is: " + lazyValue.Value); // Displays instantly after above WriteLine
        Console.WriteLine("The value of lazyValue is: " + lazyValue.Value); // Instantaneous
        Console.WriteLine("The value of lazyValue is: " + lazyValue.Value); // Instantaneous
    }
}
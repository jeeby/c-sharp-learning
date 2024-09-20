using System.Text;

public sealed class StreamIntroduction
{
    public void RunExample()
    {
        // Streams in C# allow us to work with data
        // in a more abstract way than just having
        // a byte array.

        // Streams provide us a common API for working
        // with binary data without having to know
        // exactly how that data is represented
        // behind the scenes!

        // Remember inheritance? There is a base class
        // for all streams called Stream, which is abstract.
        // Stream stream = new Stream(); // won't compile!

        // Streams provide information about the data source
        // that we're working with, like:
        // - the length of the data
        // - the current position in the data
        // - whether we can read from or write to the data
        // It's important to note that all streams get
        // these properties/methods because they inherit
        // from the base... but not all of them support
        // all of the functionality!

        // let's start with one of the most basic streams,
        // the MemoryStream

        MemoryStream memoryStream = new MemoryStream();

        // we can write data to a memory stream
        Console.WriteLine("Writing data to memory stream...");
        Console.WriteLine($"Stream Position Before: {memoryStream.Position}"); // 0. Position in the stream that we're at
        Console.WriteLine($"  Stream Length Before: {memoryStream.Length}"); // 0. # bytes 
        Console.WriteLine($"Stream Capacity Before: {memoryStream.Capacity}"); // 0. Capacity of the array. This can change behind the scenes, managed by memorystream implementation

        byte[] data = Encoding.UTF8.GetBytes("Hello, World! From Nick Cosentino");
        memoryStream.Write(
            data,
            offset:0, // Where to start in the data array
            count:data.Length);

        Console.WriteLine($"Stream Position After: {memoryStream.Position}"); // 33. 
        Console.WriteLine($"  Stream Length After: {memoryStream.Length}"); // 33. 
        Console.WriteLine($"Stream Capacity After: {memoryStream.Capacity}"); // 256. Dynamically resized to make space for the data (and future data).
        Console.WriteLine();

        // if we wanted to read the data back out of the stream,
        // it's important to note the position we're currently in now

        // let's get back to the start using "Seek", which allows
        // us to specify where we're seeking relative to
        Console.WriteLine("Repositioning memory stream...");
        memoryStream.Seek(0, SeekOrigin.Begin); 
        // or
        memoryStream.Position = 0; // Same as above
        Console.WriteLine($"Stream Position After: {memoryStream.Position}"); // 0

        // now we can read our data back out!
        // ... but how much data do we need to read?
        // ... where are we putting it? Need to copy it to somewhere, eg: read Buffer

        byte[] readBuffer = new byte[memoryStream.Length];

        Console.WriteLine("Reading data from memory stream...");
        int numberOfBytesRead = memoryStream.Read(
            readBuffer, // Needs to be bigger than what you're copying in to it
            0,
            readBuffer.Length);
        Console.WriteLine($"Number of bytes read: {numberOfBytesRead}"); // 33.

        string readString = Encoding.UTF8.GetString(readBuffer);
        Console.WriteLine($"Read string: {readString}");
        Console.WriteLine();

        // also important to note that MemoryStream is backed by an array, so it 
        // has a little "shortcut" for getting the bytes:
        Console.WriteLine("Reading data from memory stream using ToArray()...");
        byte[] memoryStreamBytes = memoryStream.ToArray();
        readString = Encoding.UTF8.GetString(memoryStreamBytes);
        Console.WriteLine($"Read string: {readString}"); // Matches original string
    }
}
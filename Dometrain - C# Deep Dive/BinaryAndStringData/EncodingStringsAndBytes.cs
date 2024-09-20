using System.Text;

public sealed class EncodingStringsAndBytes
{
    public void RunExample()
    {
        // string in C# represent a sequence of characters
        // but when it comes to sending data, computers
        // work with binary data

        // this means that we need to be able to convert between
        // strings and binary data, and we can do that
        // by mapping characters to/from their binary representation!

        // the concept of an "encoding" is a way to map characters.
        // the basic one that we'll start with is called ASCII
        // - ASCII only has 128 characters that it supports
        // - This is largely because of history... 1963!!!
        // - This was enough for English characters, numbers, and some punctuation        

        // we can get the bytes for a string as ASCII encoded
        string helloWorld = "Hello World!";
        byte[] bytesForHelloWorldAsAscii = Encoding.ASCII.GetBytes(helloWorld);

        // we can go backwards too!
        string helloWorldConvertedBack = Encoding.ASCII.GetString(bytesForHelloWorldAsAscii);

        // The following works, and the result after encoding to bytes and back match
        Console.WriteLine($"Converting '{helloWorld}' to bytes and back with ASCII");
        Console.WriteLine($" Original: {helloWorld}");
        Console.WriteLine($"Converted: {helloWorldConvertedBack}");
        Console.WriteLine();

        // what happens if we use characters in the string
        // that aren't in the ASCII character set?
        string unsupportedAsciiString = "😀 I'm in danger!";
        byte[] unsupportedAsciiBytes = Encoding.ASCII.GetBytes(unsupportedAsciiString);
        string convertedBackFromUnsupportedAscii = Encoding.ASCII.GetString(unsupportedAsciiBytes);
        Console.WriteLine("Converting to ASCII and back with unsupported characters");
        Console.WriteLine($" Original: {unsupportedAsciiString}"); // Shows ?? for emoji (console can't display it). Can see emoji in debugger.
        Console.WriteLine($"Converted: {convertedBackFromUnsupportedAscii}"); // Also shows ?? (console). Looks the same. Shows !! in Debugger. Lost the emoji.
        Console.WriteLine($" Original String Length: {unsupportedAsciiString.Length}"); // 17
        Console.WriteLine($"Converted String Length: {convertedBackFromUnsupportedAscii.Length}"); // 17
        Console.WriteLine($"     ASCII Bytes Length: {unsupportedAsciiBytes.Length}"); // 17
        Console.WriteLine($"Strings Equal: {unsupportedAsciiString == convertedBackFromUnsupportedAscii}"); // False
        Console.WriteLine($"First Chars Equal: {unsupportedAsciiString[0] == convertedBackFromUnsupportedAscii[0]}"); // False
        Console.WriteLine();

        // to handle this, we can use "Unicode", which is a standard
        // that that defines a much larger character set
        // and we'll use UTF-8, which is a way to encode Unicode characters
        // to make this example work!
        byte[] unsupportedStringAsUtf8Bytes = Encoding.UTF8.GetBytes(unsupportedAsciiString);
        string unsupportedStringAsUtf8 = Encoding.UTF8.GetString(unsupportedStringAsUtf8Bytes);
        Console.WriteLine("Converting to UTF-8 and back with the original characters");
        Console.WriteLine($" Original: {unsupportedAsciiString}"); // Shows same as above ASCII example
        Console.WriteLine($"Converted: {unsupportedStringAsUtf8}"); // Shows same as above ASCII example
        Console.WriteLine($"   Original Length: {unsupportedAsciiString.Length}"); // 17
        Console.WriteLine($"  Converted Length: {unsupportedStringAsUtf8.Length}"); // 17
        Console.WriteLine($"ASCII Bytes Length: {unsupportedAsciiBytes.Length}"); // 17
        Console.WriteLine($" UTF8 Bytes Length: {unsupportedStringAsUtf8Bytes.Length}"); // 19
        Console.WriteLine($"Strings Equal: {unsupportedAsciiString == unsupportedStringAsUtf8}"); // True
        Console.WriteLine($"First Chars Equal: {unsupportedAsciiString[0] == unsupportedStringAsUtf8[0]}"); // True
        Console.WriteLine();
    }
}
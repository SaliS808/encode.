using System;
using System.IO;

interface ICoder
{
    string Encode(string message, int key);
    string Decode(string encodedMessage, int key);
}

class EncoderDecoder : ICoder
{
    public string Encode(string message, int key)
    {
        char[] encodedChars = new char[message.Length];
        for (int i = 0; i < message.Length; i++)
        {
            int charValue = GetCharValue(message[i]);
            if (charValue > 0)
            {
                int newCharValue = (charValue + key) % 52;
                encodedChars[i] = GetCharFromValue(newCharValue);
            }
            else
            {
                encodedChars[i] = message[i]; 
            }
        }
        return new string(encodedChars);
    }

    public string Decode(string encodedMessage, int key)
    {
        char[] decodedChars = new char[encodedMessage.Length];
        for (int i = 0; i < encodedMessage.Length; i++)
        {
            int charValue = GetCharValue(encodedMessage[i]);
            if (charValue > 0)
            {
                int newCharValue = (charValue - key + 52) % 52;
                decodedChars[i] = GetCharFromValue(newCharValue);
            }
            else
            {
                decodedChars[i] = encodedMessage[i]; 
            }
        }
        return new string(decodedChars);
    }

    private int GetCharValue(char c)
    {
        if (char.IsLower(c)) return c - 'a' + 1;
        if (char.IsUpper(c)) return c - 'A' + 27;
        return -1; 
    }

    private char GetCharFromValue(int value)
    {
        if (value >= 1 && value <= 26) return (char)('a' + value - 1);
        if (value >= 27 && value <= 52) return (char)('A' + value - 27);
        return '?'; 
    }
}

class Program
{
    static void Main()
    {
        EncoderDecoder coder = new EncoderDecoder();

        Console.Write("Enter sender name: ");
        string sender = Console.ReadLine();
        Console.Write("Enter receiver name: ");
        string receiver = Console.ReadLine();

        int senderKey = CalculateNameKey(sender);
        int receiverKey = CalculateNameKey(receiver);

        int keyMethod1 = (senderKey + receiverKey) % 52;
        int keyMethod2 = (senderKey * receiverKey) % 52;

        Console.Write("Enter your message: ");
        string message = Console.ReadLine();

        
        string encodedMethod1 = coder.Encode(message, keyMethod1);
        string encodedMethod2 = coder.Encode(message, keyMethod2);

        Console.WriteLine($"\nEncoded with Method 1: {encodedMethod1}");
        Console.WriteLine($"Encoded with Method 2: {encodedMethod2}");

        
        SaveToFile("method1_encoded.txt", encodedMethod1);
        SaveToFile("method2_encoded.txt", encodedMethod2);

       
        string decodedMethod1 = coder.Decode(encodedMethod1, keyMethod1);
        string decodedMethod2 = coder.Decode(encodedMethod2, keyMethod2);

        Console.WriteLine($"\nDecoded with Method 1: {decodedMethod1}");
        Console.WriteLine($"Decoded with Method 2: {decodedMethod2}");
    }

    static int CalculateNameKey(string name)
    {
        int key = 0;
        foreach (char c in name)
        {
            key += GetCharValue(c);
        }
        return key % 52;
    }

    static int GetCharValue(char c)
    {
        if (char.IsLower(c)) return c - 'a' + 1;
        if (char.IsUpper(c)) return c - 'A' + 27;
        return 0;
    }

    static void SaveToFile(string fileName, string content)
    {
        File.WriteAllText(fileName, content);
        Console.WriteLine($"Saved to {fileName}");
    }
}

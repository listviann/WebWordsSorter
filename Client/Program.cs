using System.Net.Sockets;
using System.Text.RegularExpressions;

int port = 13000;
string serverAddress = "127.0.0.1";

if (args.Length > 0)
    serverAddress = args[0];

var tcpClient = new TcpClient(serverAddress, port);

try
{
    var stream = tcpClient.GetStream();
    var streamReader = new StreamReader(stream);
    var streamWriter = new StreamWriter(stream)
    {
        AutoFlush = true
    };

    // Console.WriteLine(streamReader.ReadLine());

    while (true)
    {
        Console.WriteLine("Words: ");
        string? words = Console.ReadLine();
        Console.WriteLine(words);
        if (words is null || words == "") break;
        streamWriter.WriteLine(words);
        if (words == "") break;
        Console.WriteLine("Sorted words: ");

        var response = streamReader.ReadLine();
        var formatResponse = response!.Replace("\t", Environment.NewLine);
        Console.WriteLine(formatResponse);
    }

    stream.Close();
}
catch (Exception ex)
{
    Console.WriteLine($"ERROR: {ex.Message}");
}
finally
{
    tcpClient.Close();
}
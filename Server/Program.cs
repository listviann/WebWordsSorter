using System.Net;
using System.Net.Sockets;
using System.Text;

const int LIMIT = 5;
int port = 13000;
var localAddress = IPAddress.Parse("127.0.0.1");
var tcpListener = new TcpListener(localAddress, port);
tcpListener.Start();
Console.WriteLine($"Server mounted, listening to port {port}");

// accept and serve more than one client
for (int i = 0; i < LIMIT; i++)
{
    Thread t = new(new ThreadStart(Service));
    t.Start();
}

Service();

void Service()
{
    while (true)
    {
        Socket sock = tcpListener.AcceptSocket();
        Console.WriteLine($"Connected {sock.RemoteEndPoint}");

        try
        {
            var networkStream = new NetworkStream(sock);
            var streamReader = new StreamReader(networkStream);
            var streamWriter = new StreamWriter(networkStream)
            {
                AutoFlush = true
            };

            while (true)
            {
                string? message = streamReader.ReadLine();
                Console.WriteLine(message);
                if (message is null || string.IsNullOrWhiteSpace(message)) break;
                string[] words = message.Split(' ');
                Array.Sort(words);
                var stringBuilder = new StringBuilder();

                for (int i = 0; i < words.Length; i++)
                    stringBuilder.Append(words[i] + "\t");

                string responseMessage = stringBuilder.ToString();
                Console.WriteLine(responseMessage);
                streamWriter.WriteLine(responseMessage);
            }

            networkStream.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR: {ex.Message}");
        }

        Console.WriteLine($"Disconnected:: {sock.RemoteEndPoint}");
        sock.Close();
    }
}

using System;
using System.Net.Sockets;
using System.Net;
using System.Text;

namespace ChatBot2._0
{
    class Program
    {
        static void Main(string[] args)
        {
            Socket listenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            
            // config: IP dove ascoltare. Possiamo usare l'opzione Any: ascolta da tutte le interfaccie all'interno del mio pc.
            IPAddress ipaddr = IPAddress.Any;

            // config: devo configurare l'EndPoint
            IPEndPoint ipep = new IPEndPoint(ipaddr, 23000);

            // config: Bind -> collegamento
            // listenerSocket lo collego all'endpoint che ho appena configurato
            listenerSocket.Bind(ipep);

            // Mettere in ascolto il server.
            // parametro: il numero massimo di connessioni da mettere in coda.
            listenerSocket.Listen(5);
            Console.WriteLine("Server in ascolto...");
            Console.WriteLine("in attesa di connessione da parte del client...");
            // Istruzione bloccante
            // restituisce una variabile di tipo socket.
            Socket client = listenerSocket.Accept();

            Console.WriteLine("Client IP: " + client.RemoteEndPoint.ToString());

            // mi attrezzo per ricevere un messaggio dal client
            // siccome è di tipo stream io riceverò dei byte, o meglio un byte array
            // riceverò anche il numero di byte.
            byte[] recvBuff = new byte[128];
            byte[] sendBuff = new byte[128];
            int receivedBytes = 0;
            int sendedBytes = 0;
            string receivedString, sendString;

            // crea il messaggio
            sendString = "Benvenuto client!";
             
            while (true)
            {
                receivedBytes = client.Receive(recvBuff);
                Console.WriteLine("Numero di byte ricevuti: " + receivedBytes);
                receivedString = Encoding.ASCII.GetString(recvBuff, 0, receivedBytes);
                Console.WriteLine("Stringa ricevuta: " + receivedString);

                // \r\n è l'invio
                if (receivedString != "\r\n")
                {
                    if (receivedString.ToUpper() == "QUIT")
                    {
                        break;
                    }

                    sendString = Answer(receivedBytes, sendedBytes, receivedString, sendString);

                    // lo converto in byte
                    sendBuff = Encoding.ASCII.GetBytes(sendString);

                    //invio al client il messaggio
                    sendedBytes = client.Send(sendBuff);

                    Array.Clear(recvBuff, 0, recvBuff.Length);
                    receivedBytes = 0;
                    sendedBytes = 0;
                }
            }

            Console.WriteLine("Connessione chiusa");

            // Termina il programma
            Console.ReadLine();
        }

        static string Answer(int receivedBytes, int sendedBytes, string receivedString, string sendString)
        {
            Random rnd = new Random();
            int n = rnd.Next(10) % 2;

            switch (receivedString.ToLower())
            {
                case "ciao":
                    if (n == 0)
                    {
                        sendString = "Salve";
                    }
                    else
                    {
                        sendString = "Buon Pomeriggio";
                    }
                    break;

                case "come stai?":
                    if (n == 0)
                    {
                        sendString = "Bene";
                    }
                    else
                    {
                        sendString = "Male";
                    }
                    break;

                case "che fai?":
                    if (n == 0)
                    {
                        sendString = "Niente";
                    }
                    else
                    {
                        sendString = "Studio";
                    }
                    break;

                case "risolto un bag":
                    if (n == 0)
                    {
                        sendString = "si creano mille bag nuovi";
                    }
                    else
                    {
                        sendString = "si creano duemila bag nuovi";
                    }
                    break;

                default:
                    sendString = "Ripeti per favore";
                    break;
            };

            return sendString;
        }
    }
}
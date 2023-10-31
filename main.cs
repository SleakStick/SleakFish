using System;
using System.Net;
//using FEN_BTBConverterNS;
using System.Net.Sockets;
using System.Text;
using System.Threading;


namespace mainScript
{
    class MainClass
    {
        bool running = true;
        public string connectionIP = "127.0.0.1";
        public int connectionPort = 25001;
        IPAddress localAdd;
        TcpListener listener;
        TcpClient client;
            
        static void Main(string[] args)
        {
            //FEN_BTBConverter FEN_BTBconvert = new FEN_BTBConverter();
            //Bitboards bitboards = FEN_BTBconvert.FENtoBTB("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
            //Console.WriteLine(bitboards.Wrook);
            MainClass classInstance = new MainClass();
            Thread dataCheckThread = new Thread(new ThreadStart(classInstance.requestHandler));
            dataCheckThread.Start();
            
        }

        public void requestHandler(){
            localAdd = IPAddress.Parse(connectionIP);
            listener = new TcpListener(IPAddress.Any, connectionPort);
            listener.Start();

            client = listener.AcceptTcpClient();

            while (running){
                NetworkStream nwStream = client.GetStream();
                byte[] buffer = new byte[client.ReceiveBufferSize];

                //---receiving Data from the Host----
                int requestBytes = nwStream.Read(buffer, 0, client.ReceiveBufferSize); //Getting data in Bytes from Python
                string request = Encoding.UTF8.GetString(buffer, 0, requestBytes); //Converting byte data to string
                if (request != null){
                    byte[] response = Encoding.ASCII.GetBytes("Hey I got your request Python! Do You see this response?"); //Converting string to byte data
                    nwStream.Write(response, 0, response.Length); //Sending the data in Bytes to Python script
                }
            }
        }

    }
}
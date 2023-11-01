using System;
using System.IO.Pipes;
using System.Net;
using FEN_BTBConverterNS;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Security.Cryptography.X509Certificates;


namespace mainScript
{
    class MainClass
    {
        public bool running = true;
        public string connectionIP = "127.0.0.1";
        public int connectionPort = 25001;
        IPAddress localAdd;
        TcpListener listener;
        TcpClient client;
            
        static void Main(string[] args)
        {
            MainClass classInstance = new MainClass();
            Thread dataCheckThread = new Thread(new ThreadStart(classInstance.requestHandler));
            dataCheckThread.Start();
            
        }

        public void requestHandler(){
            localAdd = IPAddress.Parse(connectionIP);
            listener = new TcpListener(IPAddress.Any, connectionPort);
            listener.Start();
            client = listener.AcceptTcpClient();
            Console.WriteLine("Connection to python script established");

            while (true){
                NetworkStream nwStream = client.GetStream();
                byte[] buffer = new byte[client.ReceiveBufferSize];

                //---receiving Data from the Host----
                int requestBytes = nwStream.Read(buffer, 0, client.ReceiveBufferSize); //Getting data in Bytes from Python
                string request = Encoding.UTF8.GetString(buffer, 0, requestBytes); //Converting byte data to string
                if (request != null){
                    Console.WriteLine("Request " + request + " received from python, computing response");
                    byte[] response = Encoding.ASCII.GetBytes(responseHandler(request)); //Converting string to byte data
                    nwStream.Write(response, 0, response.Length); //Sending the data in Bytes to Python script
                    Console.WriteLine("Response sent");
                }
            }
        }
        public string responseHandler(string request){
            string response="";
            if (request == "pieces_position"){
                FEN_BTBConverter FEN_BTBconvert = new FEN_BTBConverter();
                Bitboards bitboards = FEN_BTBconvert.FENtoBTB("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
                response = (bitboards.Wpawn.ToString() + bitboards.Bpawn.ToString() + bitboards.Wking.ToString() + bitboards.Bking.ToString() 
                + bitboards.Wqueen.ToString() + bitboards.Bqueen.ToString() + bitboards.Wrook.ToString() + bitboards.Brook.ToString()
                + bitboards.Wbishop.ToString() + bitboards.Bbishop.ToString() + bitboards.Wknight.ToString() + bitboards.Bknight.ToString());
            }
            return response;
        }

    }
}
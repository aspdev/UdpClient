using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerClient
{
    class Program
    {
        
        static void Main(string[] args)
        {

            string message = "Hello from UdpClient";
            byte[] packet = Encoding.UTF8.GetBytes(message);
            UdpClient udpClient = new UdpClient();

            
            IPEndPoint remoteEndpoint = new IPEndPoint(IPAddress.Parse("192.168.1.2"), 24000);

            try
            {
                udpClient.BeginSend(packet, packet.Length, remoteEndpoint, OnCompleteSend, udpClient);
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
           
            Console.ReadLine();
        }

        private static void OnCompleteSend(IAsyncResult asyncResult)
        {
            UdpClient udpClient = (UdpClient) asyncResult.AsyncState;
            int numberOfBytesSent = udpClient.EndSend(asyncResult);
            Console.WriteLine($"Sent {numberOfBytesSent} bytes to the server...");

            try
            {
                udpClient.BeginReceive(OnCompleteReceive, udpClient);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            
        }

        private static void OnCompleteReceive(IAsyncResult asyncResult)
        {
            UdpClient udpClient = (UdpClient) asyncResult.AsyncState;
            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, 0);
            byte[] echoedBytes = udpClient.EndReceive(asyncResult, ref ipEndPoint);

            Console.WriteLine($"Echoed message from {ipEndPoint.ToString()}:" +
                              $" {Encoding.UTF8.GetString(echoedBytes)}");

        }
    }
}

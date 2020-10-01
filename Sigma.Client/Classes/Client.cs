﻿using Sigma.Networking.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sigma.Networking
{
    public class StateObject
    {
        // Client socket.  
        public Socket workSocket = null;
        // Size of receive buffer.  
        public const int BufferSize = 256;
        // Receive buffer.  
        public byte[] buffer = new byte[BufferSize];
        // Received data string.  
        public StringBuilder sb = new StringBuilder();
    }

    public class Client
    {

        // The port number for the remote device.  
        private const int port = 11000;

        // ManualResetEvent instances signal completion.  
        private static ManualResetEvent connectDone =
            new ManualResetEvent(false);
        private static ManualResetEvent sendDone =
            new ManualResetEvent(false);
        private static ManualResetEvent receiveDone =
            new ManualResetEvent(false);

        private static String response = String.Empty; 

        private int currentIP = 0; 
        //unused initializer...might want to scan ports or something like that - unsure at this moment 
        public Client()
        {

        }

        
        public async Task<bool> StartClientAsync()
        {

            bool connected = false;
            Task<bool> searchHostTask = searchForHostAsync();

            connected = await searchHostTask;

            return connected; 


            //// Send test data to the remote device.  
            //Send(client, "This is a test \n");
            //sendDone.WaitOne();

            //// Receive the response from the remote device.  
            //Receive(client);
            //receiveDone.WaitOne();

            //// Write the response to the console.  
            //Console.WriteLine("Response received : {0}", response);

            //// Release the socket.  
            //client.Shutdown(SocketShutdown.Both);
            //client.Close();
        }

        private async Task<bool> searchForHostAsync()
        {
            int ipCore = 0;
            Sigma_IpAddress currentIP; 
            while (ipCore < 200)
            {
                
                ipCore++; 
            }

            IPHostEntry ipHost = Dns.GetHostEntry(IPAddress.Parse("192.168.1.24"));
            IPAddress ipAddress = ipHost.AddressList[0];
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);


            //create TCP socket 
            Socket client = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            //connect to the remote endpoint
            client.BeginConnect(remoteEP, new AsyncCallback(ConnectCallback), client);
            connectDone.WaitOne();

            if (client.Connected)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.  
                Socket client = (Socket)ar.AsyncState;

                // Complete the connection.  
                client.EndConnect(ar);

                Console.WriteLine("Socket connected to {0}",
                    client.RemoteEndPoint.ToString());

                // Signal that the connection has been made.  
                connectDone.Set();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private static void Receive(Socket client)
        {
            try
            {
                // Create the state object.  
                StateObject state = new StateObject();
                state.workSocket = client;

                // Begin receiving the data from the remote device.  
                client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReceiveCallback), state);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private static void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the state object and the client socket
                // from the asynchronous state object.  
                StateObject state = (StateObject)ar.AsyncState;
                Socket client = state.workSocket;

                // Read data from the remote device.  
                int bytesRead = client.EndReceive(ar);

                if (bytesRead > 0)
                {
                    // There might be more data, so store the data received so far.  
                    state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));

                    // Get the rest of the data.  
                    client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                        new AsyncCallback(ReceiveCallback), state);
                }
                else
                {
                    // All the data has arrived; put it in response.  
                    if (state.sb.Length > 1)
                    {
                        response = state.sb.ToString();
                    }
                    // Signal that all bytes have been received.  
                    receiveDone.Set();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private static void Send(Socket client, String data)
        {
            // Convert the string data to byte data using ASCII encoding.  
            byte[] byteData = Encoding.ASCII.GetBytes(data);

            // Begin sending the data to the remote device.  
            client.BeginSend(byteData, 0, byteData.Length, 0,
                new AsyncCallback(SendCallback), client);
        }

        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.  
                Socket client = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.  
                int bytesSent = client.EndSend(ar);
                Console.WriteLine("Sent {0} bytes to server.", bytesSent);

                // Signal that all bytes have been sent.  
                sendDone.Set();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        internal static Int64 AddressToInt(IPAddress addr)
        {
            byte[] addressBits = addr.GetAddressBytes();

            Int64 retval = 0;
            for (int i = 0; i < addressBits.Length; i++)
            {
                retval = (retval << 8) + (int)addressBits[i];
            }

            return retval;
        }

        internal static Int64 AddressToInt(string addr)
        {
            return AddressToInt(IPAddress.Parse(addr));
        }

        internal static IPAddress IntToAddress(Int64 addr)
        {
            return IPAddress.Parse(addr.ToString());
        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Sigma.Networking
{
    public class Client
    {
        private int portNum = 1234;

        private IPHostEntry ipHost; 
        private IPAddress ipAddress;
        private IPEndPoint localEndPoint;
        private Socket sender; 

        private int currentIP = 0; 
        //unused initializer...might want to scan ports or something like that - unsure at this moment 
        public Client()
        {
            ipHost = Dns.GetHostEntry(IPAddress.Parse("192.168.1.8")); 
            ipAddress = ipHost.AddressList[0]; 
            
        }

        public void Initilize()
        {
            try
            {
                //Connect Socket to the remote endpoint using method Connect()
                

            }catch(Exception ex)
            {

            }
        }

        //sends a message to the port on the designated address
        //private bool TestAddress(IPAddress tempIP)
        //{

        //}

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
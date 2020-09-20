using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Sigma.Project
{
    class Client
    {
        private int portNum = 1234;

        private IPHostEntry iphost; 
        private IPAddress ipAddr;
        private IPEndPoint localEndPoint;
        private Socket sender; 

        //unused initializer...might want to scan ports or something like that - unsure at this moment 
        public Client()
        {

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

    }
}
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Xml;

namespace Sigma.Networking.Classes
{
    class Sigma_IpAddress
    {
        private int[] address; 

        public Sigma_IpAddress()
        {
            this.address = new int[4]; 
        }
        public Sigma_IpAddress(string fullAddress)
        {
            try
            {
                string[] tokens = fullAddress.Split('.');
                for (int i = 0; i < 4; i++)
                {
                    this.address[i] = int.Parse(tokens[i]);
                }
            }
            catch(Exception ex)
            {
                throw ex; 
            }
        }

        //convert
        public IPAddress toIP()
        {
            string sIp = string.Empty;

            if (this.address != null)
            {
                for (int i = 0; i < 4; i++)
                {
                    sIp += this.address[i];
                }
                return IPAddress.Parse(sIp);
            }
            return null; 
        }
    }
}

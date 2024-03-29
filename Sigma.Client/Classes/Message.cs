﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Sigma.Networking.Classes
{
    public class Message
    {
        struct msgType
        {
            public bool isTime;
            public bool isValid;
            public void IsTime() { this.isTime = true; this.isValid = true;  }
            public void Invalid() { this.isTime = false; this.isValid = false; }
        }
        private msgType type; 
        private string message; 

        public Message(string inMessage)
        {
            this.message = inMessage; 
        }

        public bool IsValid() { return this.type.isValid;  }
        public bool IsTime() { return this.type.isTime; }
        private void processMessage() 
        {
            if (this.message.Contains("TIME:"))
            {
                this.type.IsTime();
            }
            else
            {
                this.type.Invalid(); 
            }
        }
    }
}

using FlightMobileApp.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightMobileApp.Manager
{
    public interface ICommandManager
    {
        // Functions:
        void connect(string ip, int port);
        void write(string command);
        string read();
        void disconnect();

        // Properties:
        public double throttle { set; get; }
        double aileron { set; get; }
        double elevator { set; get; }
        public double rudder { set; get; }
    }
}

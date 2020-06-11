using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightMoblie.Manager
{
    public interface ICommandManager
    {
        // Functions:
        public void connect(string ip, int port);
        public void write(string command);
        public string read();
        public void disconnect();

        // Properties:
        public double throttle { set; get; }
        public double aileron { set; get; }
        public double elevator { set; get; }
        public double rudder { set; get; }
    }
}

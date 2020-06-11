using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FlightMoblie.Model
{
    public class Command
    {
        public double aileron { get; set; }
        public double rudder { get; set; }
        public double elevator { get; set; }
        public double throttle { get; set; }
    }
}

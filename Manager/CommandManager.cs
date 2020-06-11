using FlightMoblie.Client;
using FlightMoblie.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FlightMoblie.Manager
{
    public class CommandManager : ICommandManager
    {
        // Fields.
        IClient telnetClient;
        private volatile Boolean stop; 
        public Mutex MuTexLock = new Mutex();

        // CTR
        public CommandManager(IClient c)
        {
            this.telnetClient = c;
        }

        public void connect(string ip, int port)
        {
            telnetClient.connect(ip, port);
        }


        public void disconnect()
        {
            telnetClient.disconnect();
        }

        public string read()
        {
            return telnetClient.read();
        }

        public void write(string command)
        {
            telnetClient.write(command);
        }


        public double elevator
        {
            get { return elevator; }
            set
            {
                elevator = value;
            }
        }


        public double aileron
        {
            get { return aileron; }
            set
            {
                aileron = value;
            }
        }

        public double throttle
        {
            get { return throttle; }
            set
            {
                throttle = value;

            }
        }


        public double rudder
        {
            get { return rudder; }
            set
            {
                rudder = value;

            }
        }

        // The logic of the sampling function from the simulator.
        public void startFromSimulator(Command command)
        {

                try
                {
                        // Gets: 
                       string s;
                       double val;

                       // Aileron.
                       this.telnetClient.write("set /controls/flight/aileron " + command.aileron + "\r\n");
                       this.telnetClient.write("get /controls/flight/aileron \r\n");
                       s = telnetClient.read();
                       val = Convert.ToDouble(s);
                       Debug.WriteLine("Aileron works good " + val);


                        // Elevator.
                        this.telnetClient.write("set /controls/flight/elevator " + command.elevator + "\r\n");
                        this.telnetClient.write("get /controls/flight/elevator \r\n");
                        s = telnetClient.read();
                        val = Convert.ToDouble(s);
                        Debug.WriteLine("Elevator works good " + val);


                        // Rudder.
                        this.telnetClient.write("set /controls/flight/rudder " + command.rudder + "\r\n");
                        this.telnetClient.write("get /controls/flight/rudder \r\n");
                        s = telnetClient.read();
                        val = Convert.ToDouble(s);
                        Debug.WriteLine("Rudder works good " + val);


                        // Throttle.
                        this.telnetClient.write("set /controls/engines/current-engine/throttle " + command.throttle + "\r\n");
                        this.telnetClient.write("get /controls/engines/current-engine/throttle \r\n");
                        s = telnetClient.read();
                        val = Convert.ToDouble(s);
                        Debug.WriteLine("throttle works good " + val);

                       this.telnetClient.write("data\r\n");
                }
                catch (Exception e)
                {
                   Debug.WriteLine("problem in the catch");
                    e.ToString();
                }
            //}).Start();

        }
    }
}

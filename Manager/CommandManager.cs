using FlightMobileApp.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FlightMobileApp.Manager
{
    public class CommandManager : ICommandManager
    {
        // Fields.
        IClient telnetClient;
        private volatile Boolean stop;

        // CTR
        public CommandManager(IClient c)
        {
            this.telnetClient = c;
        }

        public void connect(string ip, int port)
        {
            telnetClient.connect("localhost", 5402);
            startFromSimulator();
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

        private double _elevator;
        public double elevator
        {
            get { return _elevator; }
            set
            {
                _elevator = value;
            }
        }

        private double _aileron;
        public double aileron
        {
            get { return _aileron; }
            set
            {
                _aileron = value;
            }
        }

        private double _throttle;
        public double throttle
        {
            get { return _throttle; }
            set
            {
                _throttle = value;

            }
        }

        private double _rud;
        public double rudder
        {
            get { return _rud; }
            set
            {
                _rud = value;
               
            }
        }

        // The logic of the sampling function from the simulator.
        private void startFromSimulator()
        {
             new Thread(delegate ()
             {
                 try
                 {
                     while (!stop)
                     {
                         // Gets: 
                         string s;
                         int len;
                         
                         // Aileron.
                         this.telnetClient.write("set /controls/flight/aileron\r\n");
                         this.telnetClient.write("get /controls/flight/aileron\r\n");
                         s = (getBetween(telnetClient.read(), "=", "(string)")).Trim();
                         len = s.Length;
                         aileron = Convert.ToDouble(s.Substring(1, len - 2));

                         // Elevator.
                         this.telnetClient.write("set /controls/flight/elevator\r\n");
                         this.telnetClient.write("get /controls/flight/elevator\r\n");
                         s = (getBetween(telnetClient.read(), "=", "(string)")).Trim();
                         len = s.Length;
                         elevator = Convert.ToDouble(s.Substring(1, len - 2));

                         // Rudder.
                         this.telnetClient.write("set /controls/flight/rudder\r\n");
                         this.telnetClient.write("get /controls/flight/rudder\r\n");
                         s = (getBetween(telnetClient.read(), "=", "(string)")).Trim();
                         len = s.Length;
                         rudder = Convert.ToDouble(s.Substring(1, len - 2));

                         // Throttle.
                         this.telnetClient.write("set /controls/flight/throttle\r\n");
                         this.telnetClient.write("get /controls/engines/current-engine/throttle\r\n");
                         s = (getBetween(telnetClient.read(), "=", "(string)")).Trim();
                         len = s.Length;
                         throttle = Convert.ToDouble(s.Substring(1, len - 2));

                         Thread.Sleep(250);
                      }
             } catch (Exception e)
             {
                 e.ToString();
              }
          }).Start();
     
        }

        // The following function will be in used in startFromSimulator in order to parse values.
        private static string getBetween(string strSource, string strStart, string strEnd)
        {
            int Start, End;
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                End = strSource.IndexOf(strEnd, Start);
                return strSource.Substring(Start, End - Start);
            }
            else
            {
                return "";
            }
        }
    }
}

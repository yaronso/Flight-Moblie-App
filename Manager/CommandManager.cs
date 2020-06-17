using FlightMobile.Model;
using FlightMoblie.Client;
using FlightMoblie.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace FlightMoblie.Manager
{
    public class CommandManager : ICommandManager
    {
        // Fields.
        IClient telnetClient;
        private volatile Boolean stop; 
        private BlockingCollection<AsyncCommand> queue;

        // CTR
        public CommandManager(IClient c)
        {
            this.queue = new BlockingCollection<AsyncCommand>();
            this.telnetClient = c;
            Start();
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

        // new Execute
        public Task<Result> Execute(Command cmd)
        {
            var asyncCommand = new AsyncCommand(cmd);
            queue.Add(asyncCommand);
            return asyncCommand.Task;
        }

        // new start
        public void Start()
        {
            Task.Factory.StartNew(ProcessCommands);
        }
        
        // new process commands
        public void ProcessCommands()
        {
            try
            {
                connect("127.0.0.1", 5402);
                write("data\r\n");

            } catch (Exception e)
            {
                e.ToString();
                Debug.WriteLine("im here");
                return;
            }
            // iterate through the async commands in the queue.
            foreach(AsyncCommand asyncCommand in queue.GetConsumingEnumerable())
            {
                Result result;
                var command = asyncCommand.Command;
                try
                {
                    startFromSimulator(command);
                    result = Result.Ok;
                    asyncCommand.Completion.SetResult(result);
                } catch (Exception)
                {
                    Debug.WriteLine("catch of startFromSimulator");
                    result = Result.NotOk;
                    asyncCommand.Completion.SetResult(result);
                }
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

                        // Elevator.
                        this.telnetClient.write("set /controls/flight/elevator " + command.elevator + "\r\n");
                        this.telnetClient.write("get /controls/flight/elevator \r\n");
                        s = telnetClient.read();

                        // Rudder.
                        this.telnetClient.write("set /controls/flight/rudder " + command.rudder + "\r\n");
                        this.telnetClient.write("get /controls/flight/rudder \r\n");
                        s = telnetClient.read();

                        // Throttle.
                        this.telnetClient.write("set /controls/engines/current-engine/throttle " + command.throttle + "\r\n");
                        this.telnetClient.write("get /controls/engines/current-engine/throttle \r\n");
                        s = telnetClient.read();

                       this.telnetClient.write("data\r\n");
                }
                catch (Exception e)
                {
                   Debug.WriteLine("problem in the catch");
                    e.ToString();
                }

        }
    }
}

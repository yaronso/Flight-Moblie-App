using FlightMobile.Model;
using FlightMoblie.Client;
using FlightMoblie.Model;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading.Tasks;

namespace FlightMoblie.Manager
{
    // The following class implements the ICommandManager. It uses dependency injection of the IClient class 
    // and a queue from type AsyncCommand.
    public class CommandManager : ICommandManager
    {
        // Fields.
        IClient telnetClient;
        private BlockingCollection<AsyncCommand> queue;

        // CTR
        public CommandManager(IClient c)
        {
            this.queue = new BlockingCollection<AsyncCommand>();
            this.telnetClient = c;
            Start();
        }

        // The following 4th functions are implementations of the interface IClient's functions.
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


        // The following function init a new async command and enqueue it.
        public Task<Result> Execute(Command cmd)
        {
            var asyncCommand = new AsyncCommand(cmd);
            queue.Add(asyncCommand);
            return asyncCommand.Task;
        }

        // The following function invokes the function ProcessCommands().
        public void Start()
        {
            Task.Factory.StartNew(ProcessCommands);
        }
        
        // new process commands
        public void ProcessCommands()
        {
            // Try to connect the Flight Gear
            try
            {
                connect("127.0.0.1", 5402);
                write("data\r\n");

            } catch (Exception e)
            {
                e.ToString();
                return;
            }
            // Iterate through the async commands in the queue.
            foreach(AsyncCommand asyncCommand in queue.GetConsumingEnumerable())
            {
                Result result;
                var command = asyncCommand.Command;
                // Send the async commands to the Flight Gear by calling startFromSimulator with the specific command.
                try
                {
                    startFromSimulator(command);
                    result = Result.Ok;
                    // Set the async command's result as Ok.
                    asyncCommand.Completion.SetResult(result);
                } catch (Exception)
                {
                    // In a case of exception set the the async command's result as Not OK.
                    result = Result.NotOk;
                    asyncCommand.Completion.SetResult(result);
                }
            }
        }

        // The 4th properties of the Flight Gear's rudders:
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

        // The following function sets & gets the Flight Gear's values of the 4th rudders.
        public void startFromSimulator(Command command)
        {
                try
                {
                       string s;

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
                    e.ToString();
                }

        }
    }
}

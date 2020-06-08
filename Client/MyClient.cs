using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace FlightMobileApp.Client
{
    public class MyClient : IClient
    {
        // Class fields
        private TcpClient _client;
        private NetworkStream _ns;

        // A property.
        private Boolean isc = false;
        public Boolean isConnect
        {
            get
            {
                return isc;
            }
            set
            {
                isc = value;
            }

        }
        public void connect(string ip, int port)
        {
            // Using try & catch to deal with a case of error in connection.
            try
            {
                _client = new TcpClient("localhost", port);
                _ns = _client.GetStream();
                isc = true;

            }
            // catch the exception.
            catch (Exception e)
            {
                e.ToString();

            }
        }

        public void disconnect()
        {
            _ns.Close();
            _client.Close();
        }

        public string read()
        {
            try
            {
                string retval;
                byte[] bytes = new byte[1024];
                _ns.ReadTimeout = 2000;
                int bytesRead = _ns.Read(bytes, 0, bytes.Length);
                retval = Encoding.ASCII.GetString(bytes, 0, bytesRead);
                return retval;
            }
            catch (Exception e)
            {
                return e.ToString();
                //Console.WriteLine("time out read");
                //return "Time Out 10s";
            }
        }

        public void write(string command)
        {
            try
            {
                byte[] bytes = new byte[1024];
                bytes = Encoding.ASCII.GetBytes(command);
                _ns.WriteTimeout = 2000;
                _ns.Write(bytes, 0, bytes.Length);
            }
            catch (Exception e)
            {
                e.ToString();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightMobileApp.Client;
using FlightMobileApp.Manager;
using FlightMobileApp.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlightMobileApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommandController : ControllerBase
    {
        // Field of a commandManager.
        public CommandManager commandManager;
        IClient client;

        public CommandController(CommandManager manger, IClient c)
        {
            this.client = c;
            this.commandManager = manger;
            commandManager = new CommandManager(client);
            commandManager.connect("localhost", 5402);
            commandManager.write("data\r\n");
        }

        // POST /api/command
        [HttpPost]
        public StatusCodeResult Post([FromBody] Command c)
        {
            try
            {
                this.commandManager.write(c.ToString());
                return StatusCode(StatusCodes.Status200OK);
            }
            catch(Exception e)
            {
                e.ToString();
                return StatusCode(StatusCodes.Status400BadRequest);
            }
        }

        // Get HOST:PORT/screenshot
        [HttpGet]
        public StatusCodeResult Get()
        {
            // TODO - think about the implementation.
            try
            {
                this.commandManager.write("Get HOST:PORT/screenshot");
                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception e)
            {
                e.ToString();
                return StatusCode(StatusCodes.Status400BadRequest);
            }
        }
    }
}
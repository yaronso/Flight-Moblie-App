using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FlightMoblie.Client;
using FlightMoblie.Manager;
using FlightMoblie.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlightMoblie.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommandController : ControllerBase
    {
        // Fields.
        public CommandManager commandManager;
        public IClient client;

        // CTR.
        public CommandController(CommandManager manger, IClient c)
        {
            client = c;
            commandManager = manger;
            commandManager = new CommandManager(client);
            commandManager.connect("localhost", 5402);
            commandManager.write("data\r\n");
        }

        // POST: /api/command
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Command cmd)
        {
            try
            {
                //Debug.WriteLine("aileron from controller " + cmd.aileron);
                this.commandManager.startFromSimulator(cmd);
                return Ok();
            }
            catch (Exception e)
            {
                e.ToString();
                return BadRequest();
            }
        }

        // GET:  localhost:44347/screenshot
        [HttpGet]
        public async Task<IActionResult> GetScreenShot()
        {
            Debug.WriteLine("Get Screen Shot debug 1");
            var client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync("http://localhost:8080/screenshot");
            Debug.WriteLine("Get Screen Shot debug 2");
            var image = await response.Content.ReadAsByteArrayAsync();
            Debug.WriteLine("Get Screen Shot debug 3");
            return File(image, "Image/jpg");
        }
    }
}
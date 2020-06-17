using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FlightMobile.Model;
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
            //commandManager.connect("127.0.0.1", 5402);
            //commandManager.write("data\r\n");
        }

        // POST: http://127.0.0.1:50658/api/command
        [HttpPost]
        public IActionResult Post([FromBody] Command cmd)
        {
            Result result;
            try
            {
                result = commandManager.Execute(cmd).Result;
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            if (result == Result.NotOk)
            {
                return BadRequest("Problem in sending data to the simulator!");
            }
            return Ok();
        }

        // GET:  http://127.0.0.1:50658/api/command
        [HttpGet]
        public async Task<IActionResult> GetScreenShot()
        {
            var client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync("http://localhost:8080/screenshot");
            var image = await response.Content.ReadAsByteArrayAsync();
            return File(image, "Image/jpg");
        }
    }
}
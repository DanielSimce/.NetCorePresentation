using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentaiton.Models;

namespace Presentaiton.Controllers
{
    
    [Authorize(Roles = "Admin")]
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private readonly DatabaseContext _context;
        public PlayerController(DatabaseContext context)
        {
            _context = context;
        }

        [HttpPost]
        public ActionResult<Player> Post(Player player)
        {
            _context.Add(player);
            _context.SaveChanges();

            return Ok(player);
        }

        [HttpPut]
        public ActionResult<Player> Put(Player player)
        {
            var playerInDb = _context.Players.FirstOrDefault(x => x.Id == player.Id);

            if (playerInDb == null)
            {
                return BadRequest("Does not exist in the database ");
            }

            playerInDb.Name = player.Name;
            playerInDb.Country = player.Country;
            playerInDb.Url = player.Url;


            _context.SaveChanges();

            return Ok(player);
        }

        [HttpGet]
        public ActionResult<List<Player>> All()
        {
            return Ok(_context.Players.ToList());
        }

        [Route("{id}")]
        [HttpGet]
        public ActionResult<Player> GetPlayerById(int id)
        {
            var playerInDb = _context.Players.FirstOrDefault( x => x.Id == id);

            if (playerInDb == null)
            {
                return BadRequest("Does not exist in the database ");
            }

            return Ok(playerInDb);
        }

        [Route("{id}")]
        [HttpDelete]
        public ActionResult<Player> DeletePlayerById(int id)
        {
            var playerInDb = _context.Players.FirstOrDefault(x => x.Id == id);

            _context.Players.Remove(playerInDb);
            _context.SaveChanges();

            return Ok(playerInDb);
        }


    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Presentaiton.Models;

namespace Presentaiton.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : ControllerBase
    {
        public readonly DatabaseContext _context;

        public TeamsController(DatabaseContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin")]
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Teams>> Post(Teams teams)
        {
            await _context.Teams.AddAsync(teams);
            await _context.SaveChangesAsync();

            return teams;
        }

        [HttpPut]
        public async Task<ActionResult<Teams>> Put(Teams teams)
        {
            
            var playerInDb = await _context.Teams.FirstOrDefaultAsync(x => x.Id == teams.Id);
            playerInDb.Name = teams.Name;
            playerInDb.Url = teams.Url;

            await _context.SaveChangesAsync();

            return playerInDb;


        }

        [HttpGet]
        public async Task<ActionResult<List<Teams>>> All()
        {
            return await _context.Teams.ToListAsync();
        }

        [Route("{id}")]
        [HttpGet]
        public async Task<ActionResult<Teams>> TeamsById(int id)
        {
            var playerInDb = await _context.Teams.FirstOrDefaultAsync(x => x.Id == id);
            if (playerInDb == null)
            {
                return BadRequest("Do not Exict!!!");
            }

            return playerInDb;
        }

        [Authorize(Roles = "Admin")]
        [Authorize]
        [Route("{id}")]
        [HttpDelete]
        public async Task<ActionResult<Teams>> DeleteById(int id)
        {
            var playerInDb = await _context.Teams.FirstOrDefaultAsync(x => x.Id == id);

             _context.Teams.Remove(playerInDb);
            await _context.SaveChangesAsync();

            return playerInDb;
        }

    }
}
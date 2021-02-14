using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Presentaiton.Models;
using Presentaiton.Repository;

namespace Presentaiton.Controllers
{
    [Authorize(Roles = "Manager")]
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class NbaController : ControllerBase
    {
        private readonly INbaplayer _nbaPlayer;
        public NbaController(INbaplayer nbaplayer)
        {
            _nbaPlayer = nbaplayer;
        }

        [HttpPost]
        public ActionResult<NbaPlayer> Post(NbaPlayer nbaPlayer)
        {
            _nbaPlayer.Create(nbaPlayer);
            _nbaPlayer.Save();

            return Ok(nbaPlayer);
        }

        [HttpPut]
        public ActionResult<NbaPlayer> Put(NbaPlayer nbaPlayer)
        {

            _nbaPlayer.Update(nbaPlayer);
            _nbaPlayer.Save();

            return Ok(nbaPlayer);
        }

        [HttpGet]
        public ActionResult<NbaPlayer> Get()
        {
            return Ok(_nbaPlayer.AllPlayers());
        }

        [Route("{id}")]
        [HttpGet]
        public ActionResult<NbaPlayer> GetPlayerById(int id)
        {
            var player = _nbaPlayer.PlayerById(id);

            if (player == null)
            {
                return BadRequest("Does not exist in the database");
            }

            return player;
        }

        [Route("{id}")]
        [HttpDelete]
        public ActionResult<NbaPlayer> Delete(int id)
        {
            var playerInDb = _nbaPlayer.PlayerById(id);

            _nbaPlayer.Delete(playerInDb);
            _nbaPlayer.Save();

            return Ok(playerInDb);
        }


    }
}
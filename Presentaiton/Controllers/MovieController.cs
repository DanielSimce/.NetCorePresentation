using System;
using System.Collections.Generic;
using System.Composition.Convention;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Presentaiton.Models;

namespace Presentaiton.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly DatabaseContext _context;
        public MovieController(DatabaseContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin")]
        [Authorize]
        [HttpPost]
        public ActionResult<Movie> Post(Movie movie)
        {
            _context.Add(movie);
            _context.SaveChanges();

            return Ok(movie);
        }

        [HttpPut]
        public ActionResult<Movie> Put(Movie movie)
        {
            var movieInDb = _context.Movies.FirstOrDefault(x => x.Id == movie.Id);
            movieInDb.Name = movie.Name;
            movieInDb.GenreId = movie.GenreId;
            movieInDb.Col = movie.Col;

            return Ok(movieInDb);
        }

        [HttpGet]
        public ActionResult<List<Movie>> All()
        {
            return Ok(_context.Movies.ToList());
        }

        [Route("{id}")]
        [HttpGet]
        public ActionResult<Movie> GetById(int id)
        {
            var movie = _context.Movies.FirstOrDefault(x => x.Id == id);
            if(movie == null)
            {
                return BadRequest("Movie Not Exict");
            }

            return Ok(movie);
        }

        [Authorize(Roles = "Admin")]
        [Authorize]
        [Route("{id}")]
        [HttpDelete]
        public ActionResult<Movie> DeleteById(int id)
        {
            var movie = _context.Movies.FirstOrDefault(x => x.Id == id);

            _context.Remove(movie);
            _context.SaveChanges();


            return Ok(movie);
        }

        [Route("Rent/{id}")]
        [HttpPost]
        public ActionResult<Movie> Rent(int id)
        {
            var movie = _context.Movies.FirstOrDefault(x => x.Id == id);

            if (movie == null)
            {
                return BadRequest("Movie not exict!!!");
            }

            if (movie.Col > 0)
            {
                movie.Col -= 1;
                _context.SaveChanges();

                return Ok(movie);
            }
            else
            {
                return BadRequest("out of stock");
            }
           
        }

        [Authorize(Roles ="Admin")]
        [Authorize]
        [Route("Add/{id}")]
        [HttpPost]
        public ActionResult<Movie> Add(int id)
        {
            var movie = _context.Movies.FirstOrDefault(x => x.Id == id);

            
            
                movie.Col += 1;
                _context.SaveChanges();

                return Ok(movie);
            
           

        }

        [HttpGet("Genres")]
        public ActionResult<Genre> AllGenres()
        {
            return Ok(_context.Genres.ToList());
        }

        [HttpGet("Genres/{id}")]
        public ActionResult<Genre> GenreById(int id)
        {
            var genre = _context.Genres.FirstOrDefault(x => x.Id == id).Name;

            return Ok(genre);
        }


    }
}
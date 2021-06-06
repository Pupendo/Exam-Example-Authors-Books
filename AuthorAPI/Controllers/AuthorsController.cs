using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AuthorAPI.DataAccess;
using AuthorAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthorAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthorsController : ControllerBase
    {
        private BookDBContext _bookDbContext;

        public AuthorsController(BookDBContext bookDbContext)
        {
            _bookDbContext = bookDbContext;
        }
        
        [HttpGet]
        public async Task<ActionResult<IList<Author>>> GetAuthorsAsync()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                IList<Author> authors = await _bookDbContext.Authors.Include(a => a.Books)
                    .ToListAsync();
                return Ok(authors);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }
        
        [HttpPost]
        public async Task<ActionResult<Author>> AddAdultAsync([FromBody] Author author)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                    _bookDbContext.Authors.Add(author);
                    await _bookDbContext.SaveChangesAsync();
                    return Created($"/{author.Id}", author);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }
    }
}
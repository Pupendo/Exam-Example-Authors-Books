using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthorAPI.DataAccess;
using AuthorAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthorAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BooksController : ControllerBase
    {
        private BookDBContext dbContext;

        public BooksController(BookDBContext bookDbContext)
        {
            dbContext = bookDbContext;
        }
        [HttpGet]
        public async Task<ActionResult<IList<Book>>> GetBooksAsync()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                IList<Book> books = await dbContext.Books.ToListAsync();
                return Ok(books);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }
        
        [HttpPost]
        [Route("{id:int}")]
        public async Task<ActionResult<Book>> AddAdultAsync([FromRoute] int id, [FromBody] Book book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                
                    dbContext.Books.Add(book);
                    var author = dbContext.Authors.Include(a => a.Books).
                        First(a => a.Id == id);
                    author.Books.Add(book);
                    dbContext.Update(author);
                    await dbContext.SaveChangesAsync();
                    return Created($"/{id}", book);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }
        
        [HttpDelete]
        [Route("{isbn:int}")]
        public async Task<ActionResult> DeleteAdultAsync([FromRoute] int isbn)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var book = dbContext.Books.First(b => b.ISBN == isbn);
                if (book == null)
                {
                    return NotFound(isbn);
                }
                var b = dbContext.Books.Remove(book);
                await dbContext.SaveChangesAsync();
                return Ok(b);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }

    }
}
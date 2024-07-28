using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Assignment_3.Data;
using Assignment_3.Models;

namespace Assignment_3.Controllers
{
    [ApiController] // This attribute makes sure the controller responds to API requests.
    [Route("api/[controller]")]
    public class CommentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CommentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Comment
        [HttpGet]
        public async Task<IActionResult> GetComments()
        {
            var comments = await _context.Comments
                .Include(c => c.Product)
                .Include(c => c.User)
                .ToListAsync();
            return Ok(comments); // Returns comments as JSON
        }

        // GET: api/Comment/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetComment(int id)
        {
            var comment = await _context.Comments
                .Include(c => c.Product)
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (comment == null)
            {
                return NotFound(); // Returns 404 Not Found if comment is not found
            }
            return Ok(comment); // Returns comment as JSON
        }

        // POST: api/Comment
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Comment comment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(comment);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetComment), new { id = comment.Id }, comment); // Returns 201 Created with the created comment
            }
            return BadRequest(ModelState); // Returns 400 Bad Request if model state is invalid
        }

        // PUT: api/Comment/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Comment comment)
        {
            if (id != comment.Id)
            {
                return BadRequest(); // Returns 400 Bad Request if the IDs don't match
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(comment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentExists(comment.Id))
                    {
                        return NotFound(); // Returns 404 Not Found if comment doesn't exist
                    }
                    else
                    {
                        throw;
                    }
                }
                return NoContent(); // Returns 204 No Content on successful update
            }
            return BadRequest(ModelState); // Returns 400 Bad Request if model state is invalid
        }

        // DELETE: api/Comment/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound(); // Returns 404 Not Found if comment is not found
            }

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            return NoContent(); // Returns 204 No Content on successful deletion
        }

        private bool CommentExists(int id)
        {
            return _context.Comments.Any(e => e.Id == id);
        }
    }
}

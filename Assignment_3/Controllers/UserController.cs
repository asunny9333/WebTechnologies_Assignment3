using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Assignment_3.Data;
using Assignment_3.Models;

namespace Assignment_3.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/User
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await _context.Users.ToListAsync();
            return Ok(users);
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound(); // Returns 404 Not Found if user is not found
            }
            return Ok(user); // Returns user as JSON
        }

        // POST: api/User
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(Details), new { id = user.Id }, user); // Returns 201 Created with the created user
            }
            return BadRequest(ModelState); // Returns 400 Bad Request if model state is invalid
        }

        // PUT: api/User/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] User user)
        {
            if (id != user.Id)
            {
                return BadRequest(); // Returns 400 Bad Request if the IDs don't match
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound(); // Returns 404 Not Found if user doesn't exist
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

        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound(); // Returns 404 Not Found if user is not found
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return NoContent(); // Returns 204 No Content on successful deletion
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound(); // Returns 404 Not Found if user is not found
            }
            return Ok(user); // Returns user as JSON
        }
    }
}

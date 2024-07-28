using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Assignment_3.Data;
using Assignment_3.Models;

namespace Assignment_3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Cart
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cart>>> GetCarts()
        {
            var carts = await _context.Carts
                .Include(c => c.CartItems) // Ensure CartItems are included
                .ThenInclude(ci => ci.Product) // Optional: Include Product details if needed
                .Include(c => c.User) // Optional: Include User details if needed
                .ToListAsync();

            return Ok(carts); // Returns JSON data
        }

        // GET: api/Cart/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cart>> GetCart(int id)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems) // Ensure CartItems are included
                .ThenInclude(ci => ci.Product) // Optional: Include Product details if needed
                .Include(c => c.User) // Optional: Include User details if needed
                .FirstOrDefaultAsync(m => m.CartId == id);

            if (cart == null)
            {
                return NotFound(); // Returns 404 Not Found if cart is not found
            }

            return Ok(cart); // Returns JSON data
        }

        // POST: api/Cart
        [HttpPost]
        public async Task<ActionResult<Cart>> PostCart(Cart cart)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Returns 400 Bad Request if the model state is invalid
            }

            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCart), new { id = cart.CartId }, cart); // Returns 201 Created with the location of the new resource
        }

        // PUT: api/Cart/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCart(int id, Cart cart)
        {
            if (id != cart.CartId)
            {
                return BadRequest(); // Returns 400 Bad Request if the ID in the URL doesn't match the ID in the body
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Returns 400 Bad Request if the model state is invalid
            }

            _context.Entry(cart).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CartExists(id))
                {
                    return NotFound(); // Returns 404 Not Found if the cart doesn't exist
                }
                else
                {
                    throw;
                }
            }

            return NoContent(); // Returns 204 No Content on successful update
        }

        // DELETE: api/Cart/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCart(int id)
        {
            var cart = await _context.Carts.FindAsync(id);
            if (cart == null)
            {
                return NotFound(); // Returns 404 Not Found if the cart is not found
            }

            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();

            return NoContent(); // Returns 204 No Content on successful deletion
        }

        private bool CartExists(int id)
        {
            return _context.Carts.Any(e => e.CartId == id);
        }
    }
}

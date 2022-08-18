using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;  
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GardenCenter.Models;
// using GardenCenter.Services;

namespace GardenCenter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public ProductController(DatabaseContext context)
        {
            _context = context;
        }

        // private readonly ProductService productService;
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts(string? sku, string? type, string? name, string? manufacturer, decimal price)
    {
        if (_context.Products == null)
        {
            return NotFound();
        }

        var products = await _context.Products.ToListAsync();

        foreach (var p in products.ToList())
        {
            if (sku != null && sku != p.Sku)
            {
                products.Remove(p);
            }
            if (type != null && type != p.Type)
            {
                products.Remove(p);
            }
            if (name != null && name != p.Name)
            {
                products.Remove(p);
            }
            if (manufacturer != null && manufacturer != p.Manufacturer)
            {
                products.Remove(p);
            }
            if (price != 0 && price > 0 && price != p.Price)
            {
                products.Remove(p);
            }
        }

        return products;
    }

        // GET: api/Product/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
          if (_context.Products == null)
          {
              return NotFound();
          }
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // PUT: api/Product/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest("ID in query does not match product being altered");
            }

            _context.Entry(product).State = EntityState.Modified;

            var products = await _context.Products.ToListAsync();
            bool validPrice = false;
            Regex priceRegex = new Regex(@"^[0-9]{0,}\.[0-9]{2}$");

            foreach (var p in products)
            {
                if (p.Sku == product.Sku && p.Id != product.Id)
                {
                    return Conflict("Sku has already been taken, use another sku number");
                }
            }
            
            if (priceRegex.IsMatch(product.Price.ToString()))
            {
                validPrice = true;
            } else
            {
                return BadRequest("Price must have 2 decimal places");
            }

            if (validPrice)
            {
                _context.Entry(product).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok();
            }

            return BadRequest("Product is invalid, try again");
        }

        // POST: api/Product
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
          if (_context.Products == null)
          {
              return Problem("Entity set 'DatabaseContext.Products'  is null.");
          }

            var products = await _context.Products.ToListAsync();
            bool validPrice = false;
            Regex priceRegex = new Regex(@"^[0-9]{0,}\.[0-9]{2}$");
            foreach (var p in products)
            {
                if (p.Sku == product.Sku)
                {
                    return Conflict("Sku has already been taken, use another sku number");
                }
            }

            if (priceRegex.IsMatch(product.Price.ToString()))
            {
                validPrice = true;
            } else
            {
                return BadRequest("Price must have 2 decimal places");
            }

            if (validPrice)
            {
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
            }

            return BadRequest("Product is invalid, try again");
        }

        // DELETE: api/Product/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if (_context.Products == null)
            {
                return NotFound();
            }
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(int id)
        {
            return (_context.Products?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

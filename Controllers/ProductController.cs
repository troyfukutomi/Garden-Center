using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;  
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GardenCenter.Models;
using GardenCenter.Validation;

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
        // public void ProductValidation(){}

        // private readonly ProductService productService;
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts(string? sku, string? type, string? name, string? manufacturer, decimal price)
    {
        if (_context.Products == null)
        {
            return NotFound();
        }

        ProductValidation productValidation = new ProductValidation(_context);
        var products = await _context.Products.ToListAsync();

        return productValidation.getProducts(sku, type, name, manufacturer, price, products);
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

            ProductValidation productValidation = new ProductValidation(_context);
            var products = await _context.Products.ToListAsync();
            bool uniqueSku = productValidation.uniqueSku(product, products);
            bool validPrice = productValidation.priceIsProperFormat(product.Price);
            bool matchingIds = productValidation.matchingIds(id, product);
            Regex priceRegex = new Regex(@"^[0-9]{0,}\.[0-9]{2}$");

            if (!matchingIds)
            {
                return BadRequest("ID in query does not match order being altered");
            }

            if (!uniqueSku)
            {
                return Conflict("Sku has already been taken, use another sku number");
            }
            
            if (!validPrice)
            {
                return BadRequest("Price must have 2 decimal places");
            }

            if (validPrice &&  uniqueSku && matchingIds)
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

            ProductValidation productValidation = new ProductValidation(_context);
            var products = await _context.Products.ToListAsync();
            bool uniqueSku = productValidation.uniqueSku(product, products);
            bool validPrice = productValidation.priceIsProperFormat(product.Price);
            Regex priceRegex = new Regex(@"^[0-9]{0,}\.[0-9]{2}$");

            if (!uniqueSku)
            {
                return Conflict("Sku has already been taken, use another sku number");
            }
            
            if (!validPrice)
            {
                return BadRequest("Price must have 2 decimal places");
            }

            if (validPrice && uniqueSku)
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
                return NotFound("No Product with this ID exists");
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

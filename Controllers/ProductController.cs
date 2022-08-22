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
    /// <summary>
    /// Controller for all products, includes get, get by an id, put, post, and delete methods
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly DatabaseContext _context;

        /// <summary>
        ///  Defines the database context as _context for future use in all the methods
        /// </summary>
        /// <param name="context"></param>
        public ProductController(DatabaseContext context)
        {
            _context = context;
        }

    /// <summary>
    /// Get method for products, can take in paramters to query if desired
    /// </summary>
    /// <param name="sku">sku number of the product</param>
    /// <param name="type">type of  product</param>
    /// <param name="name">name of the product</param>
    /// <param name="manufacturer">manufacturer of the product</param>
    /// <param name="price">price of product</param>
    /// <returns>List of products</returns>
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

        /// <summary>
        /// Get method for a single product
        /// </summary>
        /// <param name="id">id of the product being fetched</param>
        /// <returns>a single product</returns>
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

        /// <summary>
        /// Put method for products
        /// </summary>
        /// <param name="id">Id of the product being updated</param>
        /// <param name="product">product with updated fields</param>
        /// <returns>No Content</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest("ID in query does not match product being altered");
            }

            //Validation checks, validation methods are located in the validation
            //folder and called here.
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

            //all validation checks must pass before being updated
            if (validPrice &&  uniqueSku && matchingIds)
            {
                _context.Entry(product).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok();
            }

            return BadRequest("Product is invalid, try again");
        }

        /// <summary>
        /// Post method for products
        /// </summary>
        /// <param name="product">new product being added to database</param>
        /// <returns>CreatedAtActionResult</returns>
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
          if (_context.Products == null)
          {
              return Problem("Entity set 'DatabaseContext.Products'  is null.");
          }

            //Validation checks, validation methods are located in the validation
            //folder and called here.
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

            //all validation checks must pass before being posted
            if (validPrice && uniqueSku)
            {
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
            }

            return BadRequest("Product is invalid, try again");
        }

        /// <summary>
        /// Delete method for products
        /// </summary>
        /// <param name="id">Id of the product being deleted</param>
        /// <returns>No content</returns>
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

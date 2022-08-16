using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;  
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GardenCenter.Models;

namespace GardenCenter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public OrderController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Order
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders(int customerId, string? date, decimal orderTotal, int productId, int quantity)
        {
          if (_context.Orders == null)
          {
              return NotFound();
          }

            var orders = await _context.Orders.ToListAsync();
            // var customers = await _context.Customers.ToListAsync();
            var items = await _context.Items.ToListAsync();
            // var products = await _context.Products.ToListAsync();

            foreach (var o in orders.ToList())
            {
                if (customerId != 0 && customerId > 0 && customerId != o.CustomerId)
                    {
                        orders.Remove(o);
                    }
                if (date != null && date != o.Date)
                    {
                        orders.Remove(o);
                    }
                if (orderTotal != 0 && orderTotal > 0 && orderTotal != o.OrderTotal)
                    {
                        orders.Remove(o);
                    }
                if (productId != 0 && productId > 0 && productId != o.Items.ProductId)
                    {
                        orders.Remove(o);
                    }
                if (quantity != 0 && quantity > 0 && quantity != o.Items.Quantity)
                    {
                        orders.Remove(o);
                    }

            }

            return orders;
        }

        // GET: api/Order/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
          if (_context.Orders == null)
          {
              return NotFound();
          }
            var order = await _context.Orders.FindAsync(id);
            var items = await _context.Items.ToListAsync();

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        // PUT: api/Order/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, Order order)
        {
            if (id != order.Id)
            {
                return BadRequest("ID in query does not match order being altered");
            }

            var customers = await _context.Customers.ToListAsync();
            var items = await _context.Items.ToListAsync();
            var products = await _context.Products.ToListAsync();
            bool customerExists = false;
            bool productExists = false;
            bool validDate = false;
            bool validTotal = false;
            Regex dateRegex = new Regex(@"^\d{4}-((0[1-9])|(1[012]))-((0[1-9]|[12]\d)|3[01])$");
            Regex totalRegex = new Regex(@"^[0-9]{0,}\.[0-9]{2}$");

            if (!OrderExists(id))
            {
                return NotFound("No orders with that Id exist. Try Again"); 
            } 

            if (order.Items.Quantity !<= 0)
            {
                return BadRequest("Quantity must be a positive number.");
            } 

            // Check customer exists in database
            foreach (var c in customers.ToList())
            {
                if (order.CustomerId == c.Id)
                {
                    customerExists = true;
                }
            } 

            if (!customerExists)
            {
                return BadRequest("Customer does not exist in database");
            }

            //Check product exists in database
            foreach (var p in products)
            {
                if (order.Items.ProductId == p.Id)
                {
                    productExists = true;
                }
            }

            if(!productExists)
            {
                return BadRequest("Product does not exist in database");
            }

            // Check that date is valid 
            if (dateRegex.IsMatch(order.Date))
            {
                validDate = true;
            } else
            {
                return BadRequest("Date does not match yyyy-MM-dd format");
            }

            // Check the decimal is 2 places 
            if (totalRegex.IsMatch(order.OrderTotal.ToString()))
            {
                validTotal = true;
            } else
            {
                return BadRequest("Order total must have 2 decimal places");
            }

           if (customerExists && productExists && validDate && validTotal)
           {
                _context.ChangeTracker.Clear();
                _context.Entry(order).State = EntityState.Modified;
                _context.Entry(order.Items).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok();
           }
            return BadRequest("Order is invalid. Try again");
        }

        // POST: api/Order
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(Order order)
        {
          if (_context.Orders == null)
          {
              return Problem("Entity set 'DatabaseContext.Orders'  is null.");
          }

            // _context.Orders.Add(order);
            // await _context.SaveChangesAsync();
            var customers = await _context.Customers.ToListAsync();
            var items = await _context.Items.ToListAsync();
            var products = await _context.Products.ToListAsync();
            bool customerExists = false;
            bool productExists = false;
            bool validDate = false;
            bool validTotal = false;
            Regex dateRegex = new Regex(@"^\d{4}-((0[1-9])|(1[012]))-((0[1-9]|[12]\d)|3[01])$");
            Regex totalRegex = new Regex(@"^[0-9]{0,}\.[0-9]{2}$");
        
            if (order.Items.Quantity !<= 0)
          {
                return BadRequest("Quantity must be a positive number.");
          } 

        // Check if Customer exists in DB
          foreach (var c in customers.ToList())
          {
            if (order.CustomerId == c.Id)
            {
                customerExists = true;
            }
          } 

          if (!customerExists)
          {
                return BadRequest("Customer does not exist in database");
          }
        // check if product exists in DB
          foreach (var p in products)
          {
            if (order.Items.ProductId == p.Id)
            {
                productExists = true;
            }
          }

          if(!productExists)
          {
                return BadRequest("Product does not exist in database");
          }

        // Check that date is valid 
          if (dateRegex.IsMatch(order.Date))
          {
                validDate = true;
          } else
          {
               return BadRequest("Date does not match yyyy-MM-dd format");
          }

        //     check the decimal is 2 places 
          if (totalRegex.IsMatch(order.OrderTotal.ToString()))
          {
                validTotal = true;
          } else
          {
                return BadRequest("Order total must have 2 decimal places");
          }

          if (customerExists && productExists && validDate && validTotal)
          {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
          }

            return BadRequest("Order is invalid. Try Again ");

        }

        // DELETE: api/Order/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            if (_context.Orders == null)
            {
                return NotFound();
            }
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderExists(int id)
        {
            return (_context.Orders?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

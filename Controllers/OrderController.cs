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
    public class OrderController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public OrderController(DatabaseContext context)
        {
            _context = context;
        }

        // public void OrderValidation(){}
        // GET: api/Order
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders(int customerId, string? date, decimal orderTotal, int productId, int quantity)
        {
          if (_context.Orders == null)
          {
              return NotFound();
          }

            OrderValidation orderValidation = new OrderValidation(_context);
            var orders = await _context.Orders.ToListAsync();
            var items = await _context.Items.ToListAsync();

            return orderValidation.getOrders(customerId, date, orderTotal, productId, quantity, orders);

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

            OrderValidation orderValidation = new OrderValidation(_context);
            ProductValidation productValidation = new ProductValidation(_context);
            CustomerValidation customerValidation = new CustomerValidation(_context);
            var customers = await _context.Customers.ToListAsync();
            var items = await _context.Items.ToListAsync();
            var products = await _context.Products.ToListAsync();
            bool matchingIds = orderValidation.matchingIds(id, order);
            bool orderExists = orderValidation.orderExists(id);
            bool customerExists = customerValidation.customerExists(order.CustomerId);
            bool productExists = productValidation.productExists(order.Items!.ProductId);
            bool validDate = orderValidation.validDate(order.Date!);
            bool validTotal = orderValidation.validTotal(order.OrderTotal);
            bool validQuantity = orderValidation.validQuantity(order.Items.Quantity);
            Regex dateRegex = new Regex(@"^\d{4}-((0[1-9])|(1[012]))-((0[1-9]|[12]\d)|3[01])$");
            Regex totalRegex = new Regex(@"^[0-9]{0,}\.[0-9]{2}$");

            if (!matchingIds)
            {
                return BadRequest("ID in query does not match order being altered");
            }

            if (!orderExists)
            {
                return NotFound("No orders with that Id exist. Try Again"); 
            } 
            
            if (!customerExists)
            {
                return BadRequest("Customer does not exist in database");
            }

            if(!productExists)
            {
                return BadRequest("Product does not exist in database");
            }

            // Check that date is valid 
            if (!validDate)
            {
                return BadRequest("Date does not match yyyy-MM-dd format");
            } 

            // Check the decimal is 2 places 
            if (!validTotal)
            {
                return BadRequest("Order total must have 2 decimal places");
            }

            if (!validQuantity)
            {
                return BadRequest("Quantity must be a positive number.");
            } 

           if (matchingIds && orderExists && customerExists && productExists && validDate && validTotal && validQuantity)
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
        // 
        // 
        // 
        // REFACTOR POST/DELETE FOR ORDERS AND ALL FOR USERS THERN REFACTOR DONE!!!!!!!!!
        // 
        // 
        // 
        // 
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(Order order)
        {
          if (_context.Orders == null)
          {
              return Problem("Entity set 'DatabaseContext.Orders'  is null.");
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
        
            if (order.Items!.Quantity !<= 0)
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
          if (dateRegex.IsMatch(order.Date!))
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
    }
}

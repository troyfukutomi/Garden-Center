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
    /// The controller for all orders, includes get, get by an id, put, post, and delete methods
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly DatabaseContext _context;

        /// <summary>
        /// Defines the database context as _context for future use in all the methods
        /// </summary>
        /// <param name="context"></param>
        public OrderController(DatabaseContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get method for all orders, can take in parameters to query.
        /// </summary>
        /// <param name="customerId">Id of the customer who owns this order</param>
        /// <param name="date"> date the order was made</param>
        /// <param name="orderTotal">total price of the order</param>
        /// <param name="productId">productId of the item being ordered</param>
        /// <param name="quantity">the quantity of the product being ordered</param>
        /// <returns>List of Orders</returns>
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

        /// <summary>
        /// Get method for a single order
        /// </summary>
        /// <param name="id">Id of the order being returned</param>
        /// <returns>A single order</returns>
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

        /// <summary>
        /// Put method for orders, to update an existing order, the new updated fields must
        /// be valid, errors will be thrown if order is invalid.
        /// </summary>
        /// <param name="id">Id of the order being altered</param>
        /// <param name="order">the new order being returned to the database</param>
        /// <returns>No content</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, Order order)
        {
            if (id != order.Id)
            {
                return BadRequest("ID in query does not match order being altered");
            }

            //Validation checks, validation methods are located in the validation
            //folder and called here.
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

            //all validation checks must pass before being updated
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

        /// <summary>
        /// Post method for orders, each order must pass various validation tests before being pushed
        /// will throw error if order is invalid.
        /// </summary>
        /// <param name="order">new order that is being pushed to the database</param>
        /// <returns>CreatedAtActionResult</returns>
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(Order order)
        {
            if (_context.Orders == null)
            {
                return Problem("Entity set 'DatabaseContext.Orders'  is null.");
            }

            //Validation checks, validation methods are located in the validation
            //folder and called here.
            OrderValidation orderValidation = new OrderValidation(_context);
            ProductValidation productValidation = new ProductValidation(_context);
            CustomerValidation customerValidation = new CustomerValidation(_context);
            var customers = await _context.Customers.ToListAsync();
            var items = await _context.Items.ToListAsync();
            var products = await _context.Products.ToListAsync();
            bool customerExists = customerValidation.customerExists(order.CustomerId);
            bool productExists = productValidation.productExists(order.Items!.ProductId);
            bool validDate = orderValidation.validDate(order.Date!);
            bool validTotal = orderValidation.validTotal(order.OrderTotal);
            bool validQuantity = orderValidation.validQuantity(order.Items.Quantity);
            Regex dateRegex = new Regex(@"^\d{4}-((0[1-9])|(1[012]))-((0[1-9]|[12]\d)|3[01])$");
            Regex totalRegex = new Regex(@"^[0-9]{0,}\.[0-9]{2}$");

        
            if (!validQuantity)
            {
                return BadRequest("Quantity must be a positive number.");
            } 

            if (!customerExists)
            {
                return BadRequest("Customer does not exist in database");
            }
        // check if product exists in DB

            if(!productExists)
            {
                return BadRequest("Product does not exist in database");
            }

        // Check that date is valid 
            if (!validDate)
            {
                return BadRequest("Date does not match yyyy-MM-dd format");
            } 

        //     check the decimal is 2 places 
            if (!validTotal)
            {
                return BadRequest("Order total must have 2 decimal places");
            }

            //all validation checks must pass before being posted
            if (customerExists && productExists && validDate && validTotal && validQuantity)
            {
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
            }

            return BadRequest("Order is invalid. Try Again ");

        }

        /// <summary>
        /// Delete method for orders
        /// </summary>
        /// <param name="id">Id of the order being deleted</param>
        /// <returns>No content</returns>
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

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
    /// The controller for all customers, includes get, get by ID, put, post and delete methods
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    
    public class CustomerController : ControllerBase
    {
        private readonly DatabaseContext _context;

        /// <summary>
        /// Defines the database context as _context for future use in all the methods
        /// </summary>
        /// <param name="context"></param>
        public CustomerController(DatabaseContext context)
        {
            _context = context;
        }
        
        /// <summary>
        /// Get method for customer entity. All parameters are optional but can be queried
        /// </summary>
        /// <param name="name">Name of the customer</param>
        /// <param name="email">Email of the customer</param>
        /// <param name="city">City customer lives in (pulled from Address)</param>
        /// <param name="state">State customer lives in (pulled from Address) </param>
        /// <param name="zipcode">Zipcode of customer (pulled from Address)</param>
        /// <param name="street">Street name of Customer (pulled from Address)</param>
        /// <returns>A list of customers</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers(string? name, string? email, string? city, string? state, string? zipcode, string? street)
        {
            if (_context.Customers == null)
            {
                return NotFound();
            }

            CustomerValidation customerValidation = new CustomerValidation(_context);
            // addresses is not directly used here but is required, without it  
            // all customers returned after a get request will have null addresses
            var addresses = await _context.Addresses.ToListAsync();
            var customers = await _context.Customers.ToListAsync(); 
            
            return customerValidation.getCustomers(name, email, city, state, zipcode, street, customers);
        }

        /// <summary>
        /// A get method for a single customer 
        /// </summary>
        /// <param name="id">Id of the customer to be returned</param>
        /// <returns>A siingle customer</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(long id)
        {
            if (_context.Customers == null)
            {
                return NotFound();
            }
            var customer = await _context.Customers.FindAsync(id);
            var addresses = await _context.Addresses.ToListAsync();
            
            if (customer == null)
            {
                return NotFound();
            }

            return customer;
        }

       /// <summary>
       /// Put method for customers
       /// </summary>
       /// <param name="id">Id of the customer being updated </param>
       /// <param name="customer">cusotmer with updated fields</param>
       /// <returns>No content</returns>
        public async Task<IActionResult> PutCustomer(long id, Customer customer)
        {
            //Validation checks, validation methods are located in the validation
            //folder and called here.
            var customers = await _context.Customers.ToListAsync();
            CustomerValidation customerValidation = new CustomerValidation(_context);
            Regex stateregex = new Regex(@"^(?-i:A[LKSZRAEP]|C[AOT]|D[EC]|F[LM]|G[AU]|HI|I[ADLN]|K[SY]|LA|M[ADEHINOPST]|N[CDEHJMVY]|O[HKR]|P[ARW]|RI|S[CD]|T[NX]|UT|V[AIT]|W[AIVY])$");
            Regex emailRegex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Regex zipcodeRegex = new Regex(@"^[0-9]{5}(?:-[0-9]{4})?$");
            bool matchingIds = customerValidation.matchingIds(id, customer);
            bool uniqueEmail = customerValidation.emailIsUnique(customer,customers);
            bool validEmail = customerValidation.emailIsProperFormat(customer.Email!);
            bool validState = customerValidation.stateIsProperFormat(customer.Address!.State!);
            bool validZip = customerValidation.zipcodeIsProperFormat(customer.Address!.Zipcode!);
            bool customerExists = customerValidation.customerExists(id);

            if (!customerExists)
            {
                return NotFound("No orders with that Id exist. Try Again"); 
            } 

            if (!matchingIds)
            {
                return BadRequest("ID in query does not match order being altered");
            }

            if (!uniqueEmail)
            {
                return Conflict("Email has already been taken, choose another email.");
            }

            if (!validEmail)
            {
                return BadRequest("Email must be in proper email format");
            } 

            if (!validState)
            {
                return BadRequest("Must be a valid US state abbreviation");
            } 

            if (!validZip)
            {
                return BadRequest("Zipcode must have 5 digits or 9 digits. xxxxx or xxxxx-xxxx");
            } 

            //all validation checks must pass before being updated
            if (validEmail && validState && validZip && uniqueEmail && matchingIds)
            {
                _context.ChangeTracker.Clear();
                _context.Entry(customer).State = EntityState.Modified;
                _context.Entry(customer.Address).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok();
            }

            return BadRequest("Customer is invalid. Try again.");
        }

        /// <summary>
        /// Post request for new customers 
        /// </summary>
        /// <param name="customer">the customer that is being added to the database</param>
        /// <returns>CreatedAtActionResult</returns>
        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomer(Customer customer)
        {
            if (_context.Customers == null)
            {
                return Problem("Entity set 'DatabaseContext.Customers'  is null.");
            }
            CustomerValidation customerValidation = new CustomerValidation(_context);
            var customers = await _context.Customers.ToListAsync();
            var addresses = await _context.Addresses.ToListAsync();
            bool uniqueEmail = customerValidation.emailIsUnique(customer,customers);
            bool validEmail = customerValidation.emailIsProperFormat(customer.Email!);
            bool validState = customerValidation.stateIsProperFormat(customer.Address!.State!);
            bool validZip = customerValidation.zipcodeIsProperFormat(customer.Address!.Zipcode!);
            Regex stateregex = new Regex(@"^(?-i:A[LKSZRAEP]|C[AOT]|D[EC]|F[LM]|G[AU]|HI|I[ADLN]|K[SY]|LA|M[ADEHINOPST]|N[CDEHJMVY]|O[HKR]|P[ARW]|RI|S[CD]|T[NX]|UT|V[AIT]|W[AIVY])$");
            Regex emailRegex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Regex zipcodeRegex = new Regex(@"^[0-9]{5}(?:-[0-9]{4})?$");
            
            if (!uniqueEmail)
            {
                return Conflict("Email has already been taken, choose another email.");
            }

            if (!validEmail)
            {
                return BadRequest("Email must be in proper email format");
            } 

            if (!validState)
            {
                return BadRequest("Must be a valid US state abbreviation");
            } 

            if (!validZip)
            {
                return BadRequest("Zipcode must have 5 digits or 9 digits. xxxxx or xxxxx-xxxx");
            } 

            
            if (uniqueEmail && validEmail && validState && validZip)
            {
                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetCustomer), new { id = customer.Id }, customer);
            }

            return BadRequest("Customer is invalid. Try again");
        }

        /// <summary>
        /// Delete method for customers
        /// </summary>
        /// <param name="id">Id of the customer being deleted</param>
        /// <returns>No content</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(long id)
        {
            if (_context.Customers == null)
            {
                return NotFound();
            }
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound("No Customer with this ID exists");
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

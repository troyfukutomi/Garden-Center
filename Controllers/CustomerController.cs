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
using GardenCenter.Logging;

namespace GardenCenter.Controllers
{
    /// <summary>
    /// The controller for all customers, includes get, get by ID, put, post and delete methods
    /// </summary>
    [Route("[controller]s")]
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

        GardenCenter.Logging.Logger logger = new GardenCenter.Logging.Logger();

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
        /// <response code="200">Returns list of customers </response> 
        /// <response code="404">If the customer database is empty</response>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers(string? name, string? email, string? city, string? state, string? zipcode, string? street)
        {

            if (_context.Customers == null)
            {
                logger.Log("Customer database is empty");
                return NotFound();
            }

            CustomerValidation customerValidation = new CustomerValidation(_context);
        
            // addresses is not directly used here but is required, without it  
            // all customers returned after a get request will have null addresses
            var addresses = await _context.Addresses.ToListAsync();
            var customers = await _context.Customers.ToListAsync(); 
            
            return Ok(customerValidation.getCustomers(name, email, city, state, zipcode, street, customers));
        }

        /// <summary>
        /// A get method for a single customer 
        /// </summary>
        /// <param name="id">Id of the customer to be returned</param>
        /// <returns>A single customer</returns>
        /// <response code="200">Returns a single customer </response> 
        /// <response code="400">If the customer database is empty</response>
        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Customer>> GetCustomer(long id)
        {
            if (_context.Customers == null)
            {
                logger.Log("Customer database is empty");
                return NotFound("Database is empty");
            }
            var customer = await _context.Customers.FindAsync(id);
            var addresses = await _context.Addresses.ToListAsync();
            
            if (customer == null)
            {
                logger.Log("Customer was not found");
                return NotFound("No Customer with that Id has been found");
            }

            return Ok(customer);
        }

        /// <summary>
        /// Put method for customers
        /// </summary>
        /// <param name="id">Id of the customer being updated </param>
        /// <param name="customer">cusotmer with updated fields</param>
        /// <returns>No content</returns>
        /// <response code="204">Returns a single customer that was updated </response> 
        /// <response code="400">If the any validation checks fail</response>
        /// <response code="404">If the customer doesn't exist</response>
        /// <response code="409">If the customer email is not unique</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
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
            bool customerExists = customerValidation.customerExists(id, customers);

            if (!customerExists)
            {
                return NotFound("No customers with that Id exist. Try Again"); 
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

            logger.Log("Customer being updated is invalid");
            return BadRequest("Customer is invalid. Try again.");
        }

        /// <summary>
        /// Post request for new customers 
        /// </summary>
        /// <param name="customer">the customer that is being added to the database</param>
        /// <returns>CreatedAtActionResult</returns>
        /// <response code="201">Creates a new customer </response> 
        /// <response code="400">If the any validation checks fail</response>
        /// <response code="404">If the customer database is empty</response>
        /// <response code="409">If the customer email is not unique</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<Customer>> PostCustomer(Customer customer)
        {
            if (_context.Customers == null)
            {
                logger.Log("Customer database is empty");
                return NotFound("Entity set 'DatabaseContext.Customers'  is null.");
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

            logger.Log("Customer being created is invalid");
            return BadRequest("Customer is invalid. Try again");
        }

        /// <summary>
        /// Delete method for customers
        /// </summary>
        /// <param name="id">Id of the customer being deleted</param>
        /// <returns>No content</returns>
        /// <response code="204">Successfullly deleted customer</response>
        /// <response code="404">If the customer database is empty or if customer is empty</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCustomer(long id)
        {
            if (_context.Customers == null)
            {
                logger.Log("Customer database is empty");
                return NotFound();
            }
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                logger.Log("Particular customer does not exist");
                return NotFound("No Customer with this ID exists");
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

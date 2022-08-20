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
    public class CustomerController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public CustomerController(DatabaseContext context)
        {
            _context = context;
        }
        // public void CustomerValidation(){}

        // GET: api/Customer
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

        // GET: api/Customer/5
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

        // PUT: api/Customer/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(long id, Customer customer)
        {
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

        // POST: api/Customer
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
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

        // DELETE: api/Customer/5
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

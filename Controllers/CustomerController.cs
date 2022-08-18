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
    public class CustomerController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public CustomerController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Customer
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers(string? name, string? email, string? city, string? state, string? zipcode, string? street)
        {
          if (_context.Customers == null)
          {
              return NotFound();
          }
            // addresses is not directly used here but is required, without it  
            // all customers returned after a get request will have null addresses
            var addresses = await _context.Addresses.ToListAsync();
            var customers = await _context.Customers.ToListAsync(); 
            
          foreach (var c in customers.ToList())
          {
            if (name != null && name != c.Name)
                {
                    customers.Remove(c);  
                }
            if (email != null && email != c.Email)
                {
                    customers.Remove(c);
                }
            if (city != null && city != c.Address!.City)
                {
                    customers.Remove(c);
                }
            if (state != null && state != c.Address!.State)
                {
                    customers.Remove(c);
                }
            if (zipcode != null && zipcode != c.Address!.Zipcode)
                {
                    customers.Remove(c);
                }
            if (street != null && street != c.Address!.Street)
                {
                    customers.Remove(c);
                }
            }
            
            return customers;
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
            // int customerId = (int)id;
            // var address = await _context.Addresses.FindAsync(customerId);
            // address = customer.Address;
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
            if (id != customer.Id)
            {
                return BadRequest("ID in query does not match customer being altered");
            }

            var customers = await _context.Customers.ToListAsync();
            bool validState = false;
            bool validEmail = false;
            bool validZip = false;
            Regex stateregex = new Regex(@"^(?-i:A[LKSZRAEP]|C[AOT]|D[EC]|F[LM]|G[AU]|HI|I[ADLN]|K[SY]|LA|M[ADEHINOPST]|N[CDEHJMVY]|O[HKR]|P[ARW]|RI|S[CD]|T[NX]|UT|V[AIT]|W[AIVY])$");
            Regex emailRegex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Regex zipcodeRegex = new Regex(@"^[0-9]{5}(?:-[0-9]{4})?$");

            foreach (var c in customers)
            {
                if (c.Email == customer.Email && c.Id != customer.Id)
                {
                    return Conflict("Email has already been taken, choose another email.");
                }
            }

            if (emailRegex.IsMatch(customer.Email!))
            {
                validEmail = true;
            } else 
            {
                return BadRequest("Email must be in proper email format");
            }

            if (zipcodeRegex.IsMatch(customer.Address!.Zipcode!))
            {
                validZip = true;
            } else
            {
                return BadRequest("Zipcode must have 5 digits or 9 digits. xxxxx or xxxxx-xxxx");
            }

            if (stateregex.IsMatch(customer.Address!.State!))
            {
                validState = true;
            } else
            {
                return BadRequest("Must be a valid US state abbreviation");
            }

            if (validEmail && validState && validZip)
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

            var customers = await _context.Customers.ToListAsync();
            var addresses = await _context.Addresses.ToListAsync();
            bool validState = false;
            bool validZip = false;
            bool validEmail = false;
            Regex stateregex = new Regex(@"^(?-i:A[LKSZRAEP]|C[AOT]|D[EC]|F[LM]|G[AU]|HI|I[ADLN]|K[SY]|LA|M[ADEHINOPST]|N[CDEHJMVY]|O[HKR]|P[ARW]|RI|S[CD]|T[NX]|UT|V[AIT]|W[AIVY])$");
            Regex emailRegex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Regex zipcodeRegex = new Regex(@"^[0-9]{5}(?:-[0-9]{4})?$");

            foreach (var c in customers)
            {
                if (c.Email == customer.Email)
                {
                    return Conflict("Email has already been taken, try again");
                }
            }

            if (emailRegex.IsMatch(customer.Email!))
            {
                validEmail = true;
            } else 
            {
                return BadRequest("Email must be in proper email format");
            }

            if (zipcodeRegex.IsMatch(customer.Address!.Zipcode!))
            {
                validZip = true;
            } else
            {
                return BadRequest("Zipcode must have 5 digits or 9 digits");
            }

            if (stateregex.IsMatch(customer.Address!.State!))
            {
                validState = true;
            } else
            {
                return BadRequest("Must be a valid US state abbreviation");
            }

            if (validEmail && validState && validZip)
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
                return NotFound();
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CustomerExists(long id)
        {
            return (_context.Customers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

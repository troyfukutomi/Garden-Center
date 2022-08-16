using GardenCenter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions; 
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace GardenCenter.Services;

public class CustomerService : ControllerBase
{
    private readonly DatabaseContext _context;
    public CustomerService(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers(string? name, string? email, string? city, string? state, string? zipcode, string? street)
    {
        if (_context.Customers == null)
        {
            return NotFound();
        }
        // addresses is not directly used here but is required, without it  
        // all customers returned after a get request will have null addresses
        // var addresses = await _context.Addresses.ToListAsync();
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
        if (city != null && city != c.Address.City)
            {
                customers.Remove(c);
            }
        if (state != null && state != c.Address.State)
            {
                customers.Remove(c);
            }
        if (zipcode != null && zipcode != c.Address.Zipcode)
            {
                customers.Remove(c);
            }
        if (street != null && street != c.Address.Street)
            {
                customers.Remove(c);
            }
        }
        
        return customers;
    }

    public async Task<ActionResult<Customer>> GetCustomerID(long id)
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

}

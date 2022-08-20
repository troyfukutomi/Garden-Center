using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions; 
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GardenCenter.Models;


namespace GardenCenter.Validation
{
    public class CustomerValidation
    {
        private readonly DatabaseContext _context;

        public CustomerValidation(DatabaseContext context)
        {
            _context = context;
        }

        public List<Customer> getCustomers(string? name, string? email, string? city, string? state, string? zipcode, string? street, List<Customer> customers)
        {   
              
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

        public bool matchingIds(long id, Customer customer)
        {
            if (id == customer.Id)
            {
                return true;
            } else
            {
                return false;
            }
        }
        public bool emailIsUnique(Customer customer,List<Customer> customers)
        {

            foreach (var c in customers.ToList())
            {
                if (c.Email == customer.Email && c.Id != customer.Id )
                {
                    return false;
                }
            }
            return true;
        }

        public bool emailIsProperFormat(string email)
        {
            Regex emailRegex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            if (emailRegex.IsMatch(email))
            {
                return true;
            } else
            {
                return false;
            }
        }

        public bool zipcodeIsProperFormat(string zipcode)
        {
            Regex zipcodeRegex = new Regex(@"^[0-9]{5}(?:-[0-9]{4})?$");
            if (zipcodeRegex.IsMatch(zipcode))
            {
                return true;
            } else 
            {
                return false;
            }
        }

        public bool stateIsProperFormat(string state)
        {
            Regex stateregex = new Regex(@"^(?-i:A[LKSZRAEP]|C[AOT]|D[EC]|F[LM]|G[AU]|HI|I[ADLN]|K[SY]|LA|M[ADEHINOPST]|N[CDEHJMVY]|O[HKR]|P[ARW]|RI|S[CD]|T[NX]|UT|V[AIT]|W[AIVY])$");
            if (stateregex.IsMatch(state))
            {
                return true;
            } else
            {
                return false;
            }
        }

        public bool customerExists(long id)
        {
            return (_context.Customers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
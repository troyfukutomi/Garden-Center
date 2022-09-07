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
    /// <summary>
    /// Validation methods for customers are stored here
    /// </summary>
    public class CustomerValidation
    {
        private readonly DatabaseContext _context;

        /// <summary>
        /// Brings in the database for us to use in our validation class
        /// </summary>
        public CustomerValidation(DatabaseContext context)
        {
            _context = context;
        }

        GardenCenter.Logging.Logger logger = new GardenCenter.Logging.Logger();

        /// <summary>
        /// Gets customers, takes in optional queries
        /// </summary>
        /// <param name="name">name of the customer</param>
        /// <param name="email">email of customer</param>
        /// <param name="city">city customer lives in </param>
        /// <param name="state">state customer lives in</param>
        /// <param name="zipcode">customer's zipcode</param>
        /// <param name="street">street customer lives on</param>
        /// <param name="customers">list of customers</param>
        /// <returns>list of customers</returns>
        public List<Customer> getCustomers(string? name, string? email, string? city, string? state, string? zipcode, string? street, List<Customer> customers)
        {   
              
            foreach (var c in customers.ToList())
            {
                if (!string.IsNullOrEmpty(name) && name != c.Name)
                {
                    customers.Remove(c);  
                }
                if (!string.IsNullOrEmpty(email) && email != c.Email)
                {
                    customers.Remove(c);
                }
                if (!string.IsNullOrEmpty(city) && city != c.Address!.City)
                {
                    customers.Remove(c);
                }
                if (!string.IsNullOrEmpty(state) && state != c.Address!.State)
                {
                    customers.Remove(c);
                }
                if (!string.IsNullOrEmpty(zipcode) && zipcode != c.Address!.Zipcode)
                {
                    customers.Remove(c);
                }
                if (!string.IsNullOrEmpty(street) && street != c.Address!.Street)
                {
                    customers.Remove(c);
                }
            }
            return customers;

        }

        /// <summary>
        /// This method checks to make sure the id ion the query is the same as the customer being updated
        /// </summary>
        /// <param name="id">id in the query</param>
        /// <param name="customer">customer being updated</param>
        /// <returns>true if id's match</returns>
        public bool matchingIds(long id, Customer customer)
        {
            if (id == customer.Id)
            {
                return true;
            } else
            {
                logger.Log("Error: Id's did not match");
                return false;
            }
        }

        /// <summary>
        /// checks to make sure email isn't already take by another customer
        /// </summary>
        /// <param name="customer">customer being created or updated</param>
        /// <param name="customers">list of customers to check emails against</param>
        /// <returns>true if email is unique</returns>
        public bool emailIsUnique(Customer customer,List<Customer> customers)
        {

            foreach (var c in customers.ToList())
            {
                if (c.Email == customer.Email && c.Id != customer.Id)
                {
                    logger.Log("Error: Email is already taken");
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// checks to make sure email is in the correct format using a regex
        /// </summary>
        /// <param name="email">email being checked</param>
        /// <returns>true if emial is proper format</returns>
        public bool emailIsProperFormat(string email)
        {
            Regex emailRegex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            if (emailRegex.IsMatch(email))
            {
                return true;
            } else
            {
                logger.Log("Error: Email is not in the proper format");
                return false;
            }
        }

        /// <summary>
        /// checks to make sure zipcode is in correct format using a regex
        /// </summary>
        /// <param name="zipcode">zipcode being checked</param>
        /// <returns>true is zipcode is proper format</returns>
        public bool zipcodeIsProperFormat(string zipcode)
        {
            Regex zipcodeRegex = new Regex(@"^[0-9]{5}(?:-[0-9]{4})?$");
            if (zipcodeRegex.IsMatch(zipcode))
            {
                return true;
            } else 
            {
                logger.Log("Error: Zipcode is not in the proper format");
                return false;
            }
        }

        /// <summary>
        /// check to make sure state is in correct format/is indeed a valid state using a regex
        /// </summary>
        /// <param name="state"></param>
        /// <returns>true if state is proper format</returns>
        public bool stateIsProperFormat(string state)
        {
            Regex stateregex = new Regex(@"^(?-i:A[LKSZRAEP]|C[AOT]|D[EC]|F[LM]|G[AU]|HI|I[ADLN]|K[SY]|LA|M[ADEHINOPST]|N[CDEHJMVY]|O[HKR]|P[ARW]|RI|S[CD]|T[NX]|UT|V[AIT]|W[AIVY])$");
            if (stateregex.IsMatch(state))
            {
                return true;
            } else
            {
                logger.Log("Error: State is not a proper US state abbreviation");
                return false;
            }
        }

        /// <summary>
        /// making sure the customer exists in the database before updating
        /// </summary>
        /// <param name="id">id used to check if customer exists</param>
        /// <param name="customers">list of all customers</param>
        /// <returns>true if the customer is found in the database</returns>
        public bool customerExists(long id, List<Customer> customers)
        {
            foreach (var c in customers)
            {
                if (id == c.Id)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
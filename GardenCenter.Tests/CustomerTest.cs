using Xunit;
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

namespace GardenCenter.Tests;

public class CustomerTest
{

    private readonly DatabaseContext _context;
    CustomerValidation customerValidation;
    public CustomerTest()
    {
        customerValidation = new CustomerValidation(_context);
    }
    
    [Fact]
    public void TestGetCustomers()
    {
        string name = null!;
        string email = null!;
        string city = null!;
        string state = null!;
        string zipcode = null!;
        string street = null!;
        List<Customer> customers = new List<Customer>();
        Address address1 = new Address()
        {
            City = "Oxnard",
            State = "CA",
            Zipcode = "55555",
            Street = "Main St.",

        };
        Customer customer1 = new Customer()
        {
            Name = "Troy",
            Email = "tf@gmail.com",
            Address = address1
        };
        Address address2 = new Address()
        {
            City = "Vista",
            State = "CA",
            Zipcode = "22222",
            Street = "North St.",

        };
        Customer customer2 = new Customer()
        {
            Name = "Lewis",
            Email = "lh44@gmail.com",
            Address = address2
        };
        customers.Add(customer1);
        customers.Add(customer2);
        List<Customer> expected = customers;
        var actual = customerValidation.getCustomers(name, email, city, state, zipcode, street, customers);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void TestGetCustomersByName()
    {
        string name = "Lewis";
        string email = null!;
        string city = null!;
        string state = null!;
        string zipcode = null!;
        string street = null!;
        List<Customer> customers = new List<Customer>();
        Address address1 = new Address()
        {
            Id = 1,
            City = "Oxnard",
            State = "CA",
            Zipcode = "55555",
            Street = "Main St.",

        };
        Customer customer1 = new Customer()
        {
            Id = 1,
            Name = "Troy",
            Email = "tf@gmail.com",
            Address = address1
        };
        Address address2 = new Address()
        {
            Id = 2,
            City = "Vista",
            State = "CA",
            Zipcode = "22222",
            Street = "North St.",

        };
        Customer customer2 = new Customer()
        {
            Id = 2,
            Name = "Lewis",
            Email = "lh44@gmail.com",
            Address = address2
        };
        customers.Add(customer1);
        customers.Add(customer2);
        List<Customer> expected = new List<Customer>();
        expected.Add(customer2);
        var actual = customerValidation.getCustomers(name, email, city, state, zipcode, street, customers);
        Assert.Equal(expected, actual);
    }

    
    [Fact]
    public void TestGetCustomersByEmail()
    {
        string name = null!;
        string email = "lh44@gmail.com";
        string city = null!;
        string state = null!;
        string zipcode = null!;
        string street = null!;
        List<Customer> customers = new List<Customer>();
        Address address1 = new Address()
        {
            Id = 1,
            City = "Oxnard",
            State = "CA",
            Zipcode = "55555",
            Street = "Main St.",

        };
        Customer customer1 = new Customer()
        {
            Id = 1,
            Name = "Troy",
            Email = "tf@gmail.com",
            Address = address1
        };
        Address address2 = new Address()
        {
            Id = 2,
            City = "Vista",
            State = "CA",
            Zipcode = "22222",
            Street = "North St.",

        };
        Customer customer2 = new Customer()
        {
            Id = 2,
            Name = "Lewis",
            Email = "lh44@gmail.com",
            Address = address2
        };
        customers.Add(customer1);
        customers.Add(customer2);
        List<Customer> expected = new List<Customer>();
        expected.Add(customer2);
        var actual = customerValidation.getCustomers(name, email, city, state, zipcode, street, customers);
        Assert.Equal(expected, actual);
    }

    
    [Fact]
    public void TestGetCustomersByCity()
    {
        string name = null!;
        string email = null!;
        string city = "Oxnard";
        string state = null!;
        string zipcode = null!;
        string street = null!;
        List<Customer> customers = new List<Customer>();
        Address address1 = new Address()
        {
            Id = 1,
            City = "Oxnard",
            State = "CA",
            Zipcode = "55555",
            Street = "Main St.",

        };
        Customer customer1 = new Customer()
        {
            Id = 1,
            Name = "Troy",
            Email = "tf@gmail.com",
            Address = address1
        };
        Address address2 = new Address()
        {
            Id = 2,
            City = "Vista",
            State = "CA",
            Zipcode = "22222",
            Street = "North St.",

        };
        Customer customer2 = new Customer()
        {
            Id = 2,
            Name = "Lewis",
            Email = "lh44@gmail.com",
            Address = address2
        };
        customers.Add(customer1);
        customers.Add(customer2);
        List<Customer> expected = new List<Customer>();
        expected.Add(customer1);
        var actual = customerValidation.getCustomers(name, email, city, state, zipcode, street, customers);
        Assert.Equal(expected, actual);
    }

    
    [Fact]
    public void TestGetCustomersByState()
    {
        string name = null!;
        string email = null!;
        string city = null!;
        string state = "CA";
        string zipcode = null!;
        string street = null!;
        List<Customer> customers = new List<Customer>();
        Address address1 = new Address()
        {
            Id = 1,
            City = "Oxnard",
            State = "CA",
            Zipcode = "55555",
            Street = "Main St.",

        };
        Customer customer1 = new Customer()
        {
            Id = 1,
            Name = "Troy",
            Email = "tf@gmail.com",
            Address = address1
        };
        Address address2 = new Address()
        {
            Id = 2,
            City = "Vista",
            State = "CA",
            Zipcode = "22222",
            Street = "North St.",

        };
        Customer customer2 = new Customer()
        {
            Id = 2,
            Name = "Lewis",
            Email = "lh44@gmail.com",
            Address = address2
        };
        customers.Add(customer1);
        customers.Add(customer2);
        List<Customer> expected = new List<Customer>();
        expected.Add(customer1);
        expected.Add(customer2);
        var actual = customerValidation.getCustomers(name, email, city, state, zipcode, street, customers);
        Assert.Equal(expected, actual);
    }

    
    [Fact]
    public void TestGetCustomersByZipcode()
    {
        string name = null!;
        string email = null!;
        string city = null!;
        string state = null!;
        string zipcode = "55555";
        string street = null!;
        List<Customer> customers = new List<Customer>();
        Address address1 = new Address()
        {
            Id = 1,
            City = "Oxnard",
            State = "CA",
            Zipcode = "55555",
            Street = "Main St.",

        };
        Customer customer1 = new Customer()
        {
            Id = 1,
            Name = "Troy",
            Email = "tf@gmail.com",
            Address = address1
        };
        Address address2 = new Address()
        {
            Id = 2,
            City = "Vista",
            State = "CA",
            Zipcode = "22222",
            Street = "North St.",

        };
        Customer customer2 = new Customer()
        {
            Id = 2,
            Name = "Lewis",
            Email = "lh44@gmail.com",
            Address = address2
        };
        customers.Add(customer1);
        customers.Add(customer2);
        List<Customer> expected = new List<Customer>();
        expected.Add(customer1);
        var actual = customerValidation.getCustomers(name, email, city, state, zipcode, street, customers);
        Assert.Equal(expected, actual);
    }

    
    [Fact]
    public void TestGetCustomersByStreet()
    {
        string name = null!;
        string email = null!;
        string city = null!;
        string state = null!;
        string zipcode = null!;
        string street = "Main St.";
        List<Customer> customers = new List<Customer>();
        Address address1 = new Address()
        {
            Id = 1,
            City = "Oxnard",
            State = "CA",
            Zipcode = "55555",
            Street = "Main St.",

        };
        Customer customer1 = new Customer()
        {
            Id = 1,
            Name = "Troy",
            Email = "tf@gmail.com",
            Address = address1
        };
        Address address2 = new Address()
        {
            Id = 2,
            City = "Vista",
            State = "CA",
            Zipcode = "22222",
            Street = "North St.",

        };
        Customer customer2 = new Customer()
        {
            Id = 2,
            Name = "Lewis",
            Email = "lh44@gmail.com",
            Address = address2
        };
        customers.Add(customer1);
        customers.Add(customer2);
        List<Customer> expected = new List<Customer>();
        expected.Add(customer1);
        var actual = customerValidation.getCustomers(name, email, city, state, zipcode, street, customers);
        Assert.Equal(expected, actual);
    }

    
    [Fact]
    public void TestEmailFormat()
    {
        string email = "lh44@gmail.com";
        var expected = true;
        var actual = customerValidation.emailIsProperFormat(email);
        Assert.Equal(expected, actual);
    } 

    [Fact]
    public void TestEmailFormat2()
    {
        string email = "lh44gmail.com";
        var expected = false;
        var actual = customerValidation.emailIsProperFormat(email);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void TestZipcodeFormat()
    {
        string zipcode = "23145-1111";
        var expected = true;
        var actual = customerValidation.zipcodeIsProperFormat(zipcode);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void TestZipcodeFormat2()
    {
        string zipcode = "23111";
        var expected = true;
        var actual = customerValidation.zipcodeIsProperFormat(zipcode);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void TestZipcodeFormat3()
    {
        string zipcode = "1234";
        var expected = false;
        var actual = customerValidation.zipcodeIsProperFormat(zipcode);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void TestStateFormat()
    {
        string state = "NY";
        var expected = true;
        var actual = customerValidation.stateIsProperFormat(state);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void TestStateFormat2()
    {
        string state = "Hello";
        var expected = false;
        var actual = customerValidation.stateIsProperFormat(state);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void TestCustomerExists()
    {
        List<Customer> customers = new List<Customer>();
        Address address1 = new Address()
        {
            Id = 1,
            City = "Oxnard",
            State = "CA",
            Zipcode = "55555",
            Street = "Main St.",

        };
        Customer customer = new Customer()
        {
            Id = 1,
            Name = "Troy",
            Email = "tf@gmail.com",
            Address = address1
        };
        customers.Add(customer);
        var expected = true;
        var actual = customerValidation.customerExists(customer.Id, customers);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void TestCustomerExists2()
    {
        List<Customer> customers = new List<Customer>();
        Address address1 = new Address()
        {
            Id = 1,
            City = "Oxnard",
            State = "CA",
            Zipcode = "55555",
            Street = "Main St.",

        };
        Customer customer = new Customer()
        {
            Id = 1,
            Name = "Troy",
            Email = "tf@gmail.com",
            Address = address1
        };
        customers.Add(customer);
        var expected = false;
        var actual = customerValidation.customerExists(41, customers);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void TestMatchingIds()
    {
        Address address = new Address()
        {
            Id = 1,
            City = "Oxnard",
            State = "CA",
            Zipcode = "55555",
            Street = "Main St.",

        };
        Customer customer = new Customer()
        {
            Id = 1,
            Name = "Troy",
            Email = "tf@gmail.com",
            Address = address
        };
        int id = 1;
        var expected = true;
        var actual = customerValidation.matchingIds(id, customer);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void TestMatchingIds2()
    {
        Address address = new Address()
        {
            Id = 1,
            City = "Oxnard",
            State = "CA",
            Zipcode = "55555",
            Street = "Main St.",

        };
        Customer customer = new Customer()
        {
            Id = 1,
            Name = "Troy",
            Email = "tf@gmail.com",
            Address = address
        };
        int id = 7231;
        var expected = false;
        var actual = customerValidation.matchingIds(id, customer);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void TestEmailIsUnique()
    {
        List<Customer> customers = new List<Customer>();
        Address address1 = new Address()
        {
            City = "Oxnard",
            State = "CA",
            Zipcode = "55555",
            Street = "Main St.",

        };
        Customer customer1 = new Customer()
        {
            Name = "Troy",
            Email = "tf@gmail.com",
            Address = address1
        };
        Address address2 = new Address()
        {
            City = "Vista",
            State = "CA",
            Zipcode = "22222",
            Street = "North St.",

        };
        Customer customer2 = new Customer()
        {
            Name = "Lewis",
            Email = "lh44@gmail.com",
            Address = address2
        };

        Address address3 = new Address()
        {
            City = "Seattle",
            State = "WA",
            Zipcode = "33333",
            Street = "Rain Ave.",

        };
        Customer customer3 = new Customer()
        {
            Name = "Daniel",
            Email = "DR3@gmail.com",
            Address = address3
        };

        customers.Add(customer1);
        customers.Add(customer2);

        var expected = true;
        var actual = customerValidation.emailIsUnique(customer3, customers);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void TestEmailIsUnique2()
    {
        List<Customer> customers = new List<Customer>();
        Address address1 = new Address()
        {
            City = "Oxnard",
            State = "CA",
            Zipcode = "55555",
            Street = "Main St.",

        };
        Customer customer1 = new Customer()
        {
            Id = 1,
            Name = "Troy",
            Email = "tf@gmail.com",
            Address = address1
        };
        Address address2 = new Address()
        {
            City = "Vista",
            State = "CA",
            Zipcode = "22222",
            Street = "North St.",

        };
        Customer customer2 = new Customer()
        {
            Id = 2,
            Name = "Lewis",
            Email = "lh44@gmail.com",
            Address = address2
        };

        Address address3 = new Address()
        {
            City = "Seattle",
            State = "WA",
            Zipcode = "33333",
            Street = "Rain Ave.",

        };
        Customer customer3 = new Customer()
        {
            Id = 3,
            Name = "Daniel",
            Email = "tf@gmail.com",
            Address = address3
        };

        customers.Add(customer1);
        customers.Add(customer2);
        // customers.Add(customer3);

        var expected = false;
        var actual = customerValidation.emailIsUnique(customer3, customers);
        Assert.Equal(expected, actual);
    }
}
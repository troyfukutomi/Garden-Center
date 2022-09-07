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

public class OrderTest
{
    private readonly DatabaseContext _context;

    OrderValidation orderValidation;

    public OrderTest()
    {
        orderValidation = new OrderValidation(_context);
    }

    [Fact]
    public void TestGetOrders()
    {
        int customerId = new int();
        string date = null!;
        decimal orderTotal = new decimal();
        int productId = new int();
        int quantity = new int();
        List<Order> orders = new List<Order>();

        Product product1 = new Product()
        {
            Id = 1,
            Sku = "12345",
            Type = "Putter",
            Name = "M Craft Type II",
            Description = "Latest design from Mizuno",
            Manufacturer = "Mizuno",
            Price = 299.99m
        };

        Item item1 = new Item()
        {
            Id = 1,
            ProductId = 1,
            Quantity = 1
        };

        Order order1 = new Order()
        {
            Id = 1,
            CustomerId = 1,
            Date = "2020-10-04",
            Items = item1,
            OrderTotal = 299.99m
        };

        Product product2 = new Product()
        {
            Id = 2,
            Sku = "22222",
            Type = "Driver",
            Name = "King F9",
            Description = "Longest Driver of 2019",
            Manufacturer = "Cobra",
            Price = 299.99m
        };

        Item item2 = new Item()
        {
            Id = 2,
            ProductId = 2,
            Quantity = 2
        };

        Order order2 = new Order()
        {
            Id = 2,
            CustomerId = 2,
            Date = "2019-12-25",
            Items = item1,
            OrderTotal = 99.99m
        };

        orders.Add(order1);
        orders.Add(order2);
        List<Order> expected = orders;
        var actual = orderValidation.getOrders(customerId, date, orderTotal, productId, quantity, orders);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void TestGetOrdersByCustomerId()
    {
        int customerId = 2;
        string date = null!;
        decimal orderTotal = new decimal();
        int productId = new int();
        int quantity = new int();
        List<Order> orders = new List<Order>();

        Product product1 = new Product()
        {
            Id = 1,
            Sku = "12345",
            Type = "Putter",
            Name = "M Craft Type II",
            Description = "Latest design from Mizuno",
            Manufacturer = "Mizuno",
            Price = 299.99m
        };

        Item item1 = new Item()
        {
            Id = 1,
            ProductId = 1,
            Quantity = 1
        };

        Order order1 = new Order()
        {
            Id = 1,
            CustomerId = 1,
            Date = "2020-10-04",
            Items = item1,
            OrderTotal = 299.99m
        };

        Product product2 = new Product()
        {
            Id = 2,
            Sku = "22222",
            Type = "Driver",
            Name = "King F9",
            Description = "Longest Driver of 2019",
            Manufacturer = "Cobra",
            Price = 299.99m
        };

        Item item2 = new Item()
        {
            Id = 2,
            ProductId = 2,
            Quantity = 2
        };

        Order order2 = new Order()
        {
            Id = 2,
            CustomerId = 2,
            Date = "2019-12-25",
            Items = item1,
            OrderTotal = 99.99m
        };

        orders.Add(order1);
        orders.Add(order2);
        List<Order> expected = new List<Order>();
        expected.Add(order2);
        var actual = orderValidation.getOrders(customerId, date, orderTotal, productId, quantity, orders);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void TestGetOrdersByDate()
    {
        int customerId = new int();
        string date = "2020-10-04";
        decimal orderTotal = new decimal();
        int productId = new int();
        int quantity = new int();
        List<Order> orders = new List<Order>();

        Product product1 = new Product()
        {
            Id = 1,
            Sku = "12345",
            Type = "Putter",
            Name = "M Craft Type II",
            Description = "Latest design from Mizuno",
            Manufacturer = "Mizuno",
            Price = 299.99m
        };

        Item item1 = new Item()
        {
            Id = 1,
            ProductId = 1,
            Quantity = 1
        };

        Order order1 = new Order()
        {
            Id = 1,
            CustomerId = 1,
            Date = "2020-10-04",
            Items = item1,
            OrderTotal = 299.99m
        };

        Product product2 = new Product()
        {
            Id = 2,
            Sku = "22222",
            Type = "Driver",
            Name = "King F9",
            Description = "Longest Driver of 2019",
            Manufacturer = "Cobra",
            Price = 299.99m
        };

        Item item2 = new Item()
        {
            Id = 2,
            ProductId = 2,
            Quantity = 2
        };

        Order order2 = new Order()
        {
            Id = 2,
            CustomerId = 2,
            Date = "2019-12-25",
            Items = item1,
            OrderTotal = 99.99m
        };

        orders.Add(order1);
        orders.Add(order2);
        List<Order> expected = new List<Order>();
        expected.Add(order1);
        var actual = orderValidation.getOrders(customerId, date, orderTotal, productId, quantity, orders);
        Assert.Equal(expected, actual);
    }
/// 
/// 
/// 
///
///Test Broken
///
///
///
///
///
    [Fact]
    public void TestGetOrdersByTotal()
    {
        int customerId = new int();
        string date = null!;
        decimal orderTotal = 299.99m;
        int productId = new int();
        int quantity = new int();
        List<Order> orders = new List<Order>();

        Product product1 = new Product()
        {
            Id = 1,
            Sku = "12345",
            Type = "Putter",
            Name = "M Craft Type II",
            Description = "Latest design from Mizuno",
            Manufacturer = "Mizuno",
            Price = 299.99m
        };

        Item item1 = new Item()
        {
            Id = 1,
            ProductId = 1,
            Quantity = 1
        };

        Order order1 = new Order()
        {
            Id = 1,
            CustomerId = 1,
            Date = "2020-10-04",
            Items = item1,
            OrderTotal = 299.99m
        };

        Product product2 = new Product()
        {
            Id = 2,
            Sku = "22222",
            Type = "Driver",
            Name = "King F9",
            Description = "Longest Driver of 2019",
            Manufacturer = "Cobra",
            Price = 99.99m
        };

        Item item2 = new Item()
        {
            Id = 2,
            ProductId = 2,
            Quantity = 2
        };

        Order order2 = new Order()
        {
            Id = 2,
            CustomerId = 2,
            Date = "2019-12-25",
            Items = item2,
            OrderTotal = 99.99m
        };

        orders.Add(order1);
        orders.Add(order2);
        List<Order> expected = new List<Order>();
        expected.Add(order1);
        var actual = orderValidation.getOrders(customerId, date, orderTotal, productId, quantity, orders);
        Assert.Equal(expected, actual);
    }    
/// 
/// 
/// 
///
///Test Broken
///
///
///
///
///
    [Fact]
    public void TestGetOrdersByProductId()
    {
        int customerId = new int();
        string date = null!;
        decimal orderTotal = new decimal();
        int productId = 1;
        int quantity = new int();
        List<Order> orders = new List<Order>();

        Product product1 = new Product()
        {
            Id = 1,
            Sku = "12345",
            Type = "Putter",
            Name = "M Craft Type II",
            Description = "Latest design from Mizuno",
            Manufacturer = "Mizuno",
            Price = 299.99m
        };

        Item item1 = new Item()
        {
            Id = 1,
            ProductId = 1,
            Quantity = 1
        };

        Order order1 = new Order()
        {
            Id = 1,
            CustomerId = 1,
            Date = "2020-10-04",
            Items = item1,
            OrderTotal = 299.99m
        };

        Product product2 = new Product()
        {
            Id = 2,
            Sku = "22222",
            Type = "Driver",
            Name = "King F9",
            Description = "Longest Driver of 2019",
            Manufacturer = "Cobra",
            Price = 99.99m
        };

        Item item2 = new Item()
        {
            Id = 2,
            ProductId = 2,
            Quantity = 2
        };

        Order order2 = new Order()
        {
            Id = 2,
            CustomerId = 2,
            Date = "2019-12-25",
            Items = item2,
            OrderTotal = 299.99m
        };

        orders.Add(order1);
        orders.Add(order2);
        List<Order> expected = new List<Order>();
        expected.Add(order1);
        var actual = orderValidation.getOrders(customerId, date, orderTotal, productId, quantity, orders);
        Assert.Equal(expected, actual);
    }
/// 
/// 
/// 
///
///Test Broken
///
///
///
///
///
    [Fact]
    public void TestGetOrdersByQuantity()
    {
        int customerId = new int();
        string date = null!;
        decimal orderTotal = new decimal();
        int productId = new int();
        int quantity = 2;
        List<Order> orders = new List<Order>();

        Product product1 = new Product()
        {
            Id = 1,
            Sku = "12345",
            Type = "Putter",
            Name = "M Craft Type II",
            Description = "Latest design from Mizuno",
            Manufacturer = "Mizuno",
            Price = 299.99m
        };

        Item item1 = new Item()
        {
            Id = 1,
            ProductId = 1,
            Quantity = 1
        };

        Order order1 = new Order()
        {
            Id = 1,
            CustomerId = 1,
            Date = "2020-10-04",
            Items = item1,
            OrderTotal = 299.99m
        };

        Product product2 = new Product()
        {
            Id = 2,
            Sku = "22222",
            Type = "Driver",
            Name = "King F9",
            Description = "Longest Driver of 2019",
            Manufacturer = "Cobra",
            Price = 99.99m
        };

        Item item2 = new Item()
        {
            Id = 2,
            ProductId = 2,
            Quantity = 2
        };

        Order order2 = new Order()
        {
            Id = 2,
            CustomerId = 2,
            Date = "2019-12-25",
            Items = item2,
            OrderTotal = 299.99m
        };

        orders.Add(order1);
        orders.Add(order2);
        List<Order> expected = new List<Order>();
        expected.Add(order2);
        var actual = orderValidation.getOrders(customerId, date, orderTotal, productId, quantity, orders);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void TestMatchingIds()
    {
        Product product = new Product()
        {
            Id = 1,
            Sku = "12345",
            Type = "Putter",
            Name = "M Craft Type II",
            Description = "Latest design from Mizuno",
            Manufacturer = "Mizuno",
            Price = 299.99m
        };

        Item item = new Item()
        {
            Id = 1,
            ProductId = 1,
            Quantity = 1
        };

        Order order = new Order()
        {
            Id = 1,
            CustomerId = 1,
            Date = "2020-10-04",
            Items = item,
            OrderTotal = 299.99m
        };

        int id = 1;
        var expected = true;
        var actual = orderValidation.matchingIds(id, order);
        Assert.Equal(expected, actual);
    } 

    [Fact]
    public void TestMatchingIds2()
    {
        Product product = new Product()
        {
            Id = 1,
            Sku = "12345",
            Type = "Putter",
            Name = "M Craft Type II",
            Description = "Latest design from Mizuno",
            Manufacturer = "Mizuno",
            Price = 299.99m
        };

        Item item = new Item()
        {
            Id = 1,
            ProductId = 1,
            Quantity = 1
        };

        Order order = new Order()
        {
            Id = 1,
            CustomerId = 1,
            Date = "2020-10-04",
            Items = item,
            OrderTotal = 299.99m
        };

        int id = 876234;
        var expected = false;
        var actual = orderValidation.matchingIds(id, order);
        Assert.Equal(expected, actual);
    }   

    [Fact]
    public void TestDateFormat()
    {
        string date = "2020-10-17";
        var expected = true;
        var actual = orderValidation.validDate(date);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void TestDateFormat2()
    {
        string date = "202010-17";
        var expected = false;
        var actual = orderValidation.validDate(date);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void TestTotalFormat()
    {
        decimal total = 499.99m;
        var expected = true;
        var actual = orderValidation.validTotal(total);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void TestTotalFormat2()
    {
        decimal total = 49.2349m;
        var expected = false;
        var actual = orderValidation.validTotal(total);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void TestQuantity()
    {
        int quantity = 54;
        var expected = true;
        var actual = orderValidation.validQuantity(quantity);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void TestQuantity2()
    {
        int quantity = -7;
        var expected = false;
        var actual = orderValidation.validQuantity(quantity);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void TestOrderExists()
    {
        List<Order> orders = new List<Order>();
        Product product = new Product()
        {
            Id = 1,
            Sku = "12345",
            Type = "Putter",
            Name = "M Craft Type II",
            Description = "Latest design from Mizuno",
            Manufacturer = "Mizuno",
            Price = 299.99m
        };

        Item item = new Item()
        {
            Id = 1,
            ProductId = 1,
            Quantity = 1
        };

        Order order1 = new Order()
        {
            Id = 1,
            CustomerId = 1,
            Date = "2020-10-04",
            Items = item,
            OrderTotal = 299.99m
        };

        orders.Add(order1);
        var expected = true;
        var acutal = orderValidation.orderExists(order1.Id, orders);
        Assert.Equal(expected, acutal);
    }

    [Fact]
    public void TestOrderExists2()
    {
        List<Order> orders = new List<Order>();
        Product product = new Product()
        {
            Id = 1,
            Sku = "12345",
            Type = "Putter",
            Name = "M Craft Type II",
            Description = "Latest design from Mizuno",
            Manufacturer = "Mizuno",
            Price = 299.99m
        };

        Item item = new Item()
        {
            Id = 1,
            ProductId = 1,
            Quantity = 1
        };

        Order order1 = new Order()
        {
            Id = 1,
            CustomerId = 1,
            Date = "2020-10-04",
            Items = item,
            OrderTotal = 299.99m
        };

        orders.Add(order1);
        var expected = false;
        var acutal = orderValidation.orderExists(55, orders);
        Assert.Equal(expected, acutal);
    }
}
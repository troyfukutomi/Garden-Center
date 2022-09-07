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

public class ProductTest
{
    private readonly DatabaseContext _context;

    ProductValidation productValidation;

    public ProductTest()
    {
        productValidation = new ProductValidation(_context);
    }

    [Fact]
    public void TestGetProducts()
    {
        string sku = null!;
        string type = null!;
        string name = null!;
        string manufacturer = null!;
        decimal price = new decimal();
        List<Product> products = new List<Product>();

        Product product1 = new Product()
        {
            Id = 1,
            Sku = "12121",
            Type = "Putter",
            Name = "M Craft Type II",
            Description = "Best of the Best",
            Manufacturer = "Mizuno",
            Price = 299.99m
        };

        Product product2 = new Product()
        {
            Id = 2,
            Sku = "33333",
            Type = "Iron",
            Name = "Apex",
            Description = "Players Iron",
            Manufacturer = "Callaway",
            Price = 1499.99m
        };

        products.Add(product1);
        products.Add(product2);
        List<Product> expected = products;
        var actual = productValidation.getProducts(sku, type, name, manufacturer, price, products);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void TestGetProductsBySku()
    {
        string sku = "12121";
        string type = null!;
        string name = null!;
        string manufacturer = null!;
        decimal price = new decimal();
        List<Product> products = new List<Product>();

        Product product1 = new Product()
        {
            Id = 1,
            Sku = "12121",
            Type = "Putter",
            Name = "M Craft Type II",
            Description = "Best of the Best",
            Manufacturer = "Mizuno",
            Price = 299.99m
        };

        Product product2 = new Product()
        {
            Id = 2,
            Sku = "33333",
            Type = "Iron",
            Name = "Apex",
            Description = "Players Iron",
            Manufacturer = "Callaway",
            Price = 1499.99m
        };

        products.Add(product1);
        products.Add(product2);
        List<Product> expected = new List<Product>();
        expected.Add(product1);
        var actual = productValidation.getProducts(sku, type, name, manufacturer, price, products);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void TestGetProductsByType()
    {
        string sku = null!;
        string type = "Iron";
        string name = null!;
        string manufacturer = null!;
        decimal price = new decimal();
        List<Product> products = new List<Product>();

        Product product1 = new Product()
        {
            Id = 1,
            Sku = "12121",
            Type = "Putter",
            Name = "M Craft Type II",
            Description = "Best of the Best",
            Manufacturer = "Mizuno",
            Price = 299.99m
        };

        Product product2 = new Product()
        {
            Id = 2,
            Sku = "33333",
            Type = "Iron",
            Name = "Apex",
            Description = "Players Iron",
            Manufacturer = "Callaway",
            Price = 1499.99m
        };

        products.Add(product1);
        products.Add(product2);
        List<Product> expected = new List<Product>();
        expected.Add(product2);
        var actual = productValidation.getProducts(sku, type, name, manufacturer, price, products);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void TestGetProductsByName()
    {
        string sku = null!;
        string type = null!;
        string name = "Apex";
        string manufacturer = null!;
        decimal price = new decimal();
        List<Product> products = new List<Product>();

        Product product1 = new Product()
        {
            Id = 1,
            Sku = "12121",
            Type = "Putter",
            Name = "M Craft Type II",
            Description = "Best of the Best",
            Manufacturer = "Mizuno",
            Price = 299.99m
        };

        Product product2 = new Product()
        {
            Id = 2,
            Sku = "33333",
            Type = "Iron",
            Name = "Apex",
            Description = "Players Iron",
            Manufacturer = "Callaway",
            Price = 1499.99m
        };

        products.Add(product1);
        products.Add(product2);
        List<Product> expected = new List<Product>();
        expected.Add(product2);
        var actual = productValidation.getProducts(sku, type, name, manufacturer, price, products);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void TestGetProductsByManufacturer()
    {
        string sku = null!;
        string type = null!;
        string name = null!;
        string manufacturer = "Mizuno";
        decimal price = new decimal();
        List<Product> products = new List<Product>();

        Product product1 = new Product()
        {
            Id = 1,
            Sku = "12121",
            Type = "Putter",
            Name = "M Craft Type II",
            Description = "Best of the Best",
            Manufacturer = "Mizuno",
            Price = 299.99m
        };

        Product product2 = new Product()
        {
            Id = 2,
            Sku = "33333",
            Type = "Iron",
            Name = "Apex",
            Description = "Players Iron",
            Manufacturer = "Callaway",
            Price = 1499.99m
        };

        products.Add(product1);
        products.Add(product2);
        List<Product> expected = new List<Product>();
        expected.Add(product1);
        var actual = productValidation.getProducts(sku, type, name, manufacturer, price, products);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void TestGetProductsByprice()
    {
        string sku = null!;
        string type = null!;
        string name = null!;
        string manufacturer = null!;
        decimal price = 299.99m;
        List<Product> products = new List<Product>();

        Product product1 = new Product()
        {
            Id = 1,
            Sku = "12121",
            Type = "Putter",
            Name = "M Craft Type II",
            Description = "Best of the Best",
            Manufacturer = "Mizuno",
            Price = 299.99m
        };

        Product product2 = new Product()
        {
            Id = 2,
            Sku = "33333",
            Type = "Iron",
            Name = "Apex",
            Description = "Players Iron",
            Manufacturer = "Callaway",
            Price = 1499.99m
        };

        products.Add(product1);
        products.Add(product2);
        List<Product> expected = new List<Product>();
        expected.Add(product1);
        var actual = productValidation.getProducts(sku, type, name, manufacturer, price, products);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void TestMatchingIds()
    {
        Product product1 = new Product()
        {
            Id = 1,
            Sku = "12121",
            Type = "Putter",
            Name = "M Craft Type II",
            Description = "Best of the Best",
            Manufacturer = "Mizuno",
            Price = 299.99m
        };
        int id = 1;
        var expected = true;
        var actual = productValidation.matchingIds(id, product1);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void TestMatchingIds2()
    {
        Product product1 = new Product()
        {
            Id = 1,
            Sku = "12121",
            Type = "Putter",
            Name = "M Craft Type II",
            Description = "Best of the Best",
            Manufacturer = "Mizuno",
            Price = 299.99m
        };
        int id = 331;
        var expected = false;
        var actual = productValidation.matchingIds(id, product1);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void TestPriceFormat()
    {
        decimal price = 19.99m;
        var expected = true;
        var actual = productValidation.priceIsProperFormat(price);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void TestPriceFormat2()
    {
        decimal price = 19.9m;
        var expected = false;
        var actual = productValidation.priceIsProperFormat(price);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void TestUniqueSku()
    {
        List<Product> proudcts = new List<Product>();
        Product product1 = new Product()
        {
            Id = 1,
            Sku = "12121",
            Type = "Putter",
            Name = "M Craft Type II",
            Description = "Best of the Best",
            Manufacturer = "Mizuno",
            Price = 299.99m
        };

        Product product2 = new Product()
        {
            Id = 2,
            Sku = "33333",
            Type = "Iron",
            Name = "Apex",
            Description = "Players Iron",
            Manufacturer = "Callaway",
            Price = 1499.99m
        };
        Product product3 = new Product()
        {
            Id = 3,
            Sku = "99999",
            Type = "Wedge",
            Name = "MG3",
            Description = "Full faced wedge",
            Manufacturer = "TaylorMade",
            Price = 149.99m
        };

        proudcts.Add(product1);
        proudcts.Add(product2);

        var expected = true;
        var actual = productValidation.uniqueSku(product3, proudcts);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void TestUniqueSku2()
    {
        List<Product> proudcts = new List<Product>();
        Product product1 = new Product()
        {
            Id = 1,
            Sku = "12121",
            Type = "Putter",
            Name = "M Craft Type II",
            Description = "Best of the Best",
            Manufacturer = "Mizuno",
            Price = 299.99m
        };

        Product product2 = new Product()
        {
            Id = 2,
            Sku = "33333",
            Type = "Iron",
            Name = "Apex",
            Description = "Players Iron",
            Manufacturer = "Callaway",
            Price = 1499.99m
        };
        Product product3 = new Product()
        {
            Id = 3,
            Sku = "33333",
            Type = "Wedge",
            Name = "MG3",
            Description = "Full faced wedge",
            Manufacturer = "TaylorMade",
            Price = 149.99m
        };

        proudcts.Add(product1);
        proudcts.Add(product2);

        var expected = false;
        var actual = productValidation.uniqueSku(product3, proudcts);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void TestProductExists()
    {
        List<Product> proudcts = new List<Product>();
        Product product1 = new Product()
        {
            Id = 1,
            Sku = "12121",
            Type = "Putter",
            Name = "M Craft Type II",
            Description = "Best of the Best",
            Manufacturer = "Mizuno",
            Price = 299.99m
        };

        proudcts.Add(product1);
        var expected = true;
        var actual = productValidation.productExists(product1.Id, proudcts);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void TestProductExists2()
    {
        List<Product> proudcts = new List<Product>();
        Product product1 = new Product()
        {
            Id = 1,
            Sku = "12121",
            Type = "Putter",
            Name = "M Craft Type II",
            Description = "Best of the Best",
            Manufacturer = "Mizuno",
            Price = 299.99m
        };

        proudcts.Add(product1);
        var expected = false;
        var actual = productValidation.productExists(13, proudcts);
        Assert.Equal(expected, actual);
    }
}
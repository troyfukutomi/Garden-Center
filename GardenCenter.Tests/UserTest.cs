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

public class UserTest
{
    private readonly DatabaseContext _context;

    UserValidation userValidation;

    public UserTest()
    {
        userValidation = new UserValidation(_context);
    }

    [Fact]
    public void TestGetUsers()
    {
        string name = null!;
        string title = null!;
        string email = null!;
        string password = null!;
        List<User> users = new List<User>();
        Role role1 = new Role()
        {
            Id = 1,
            Admin = true,
            Employee = true,
        };

        User user1 = new User()
        {
            Id = 1,
            Name = "Troy",
            Title = "Dev",
            Roles = role1,
            Email = "tf@gmail.com",
            Password = "12345678"

        };
        
        Role role2 = new Role()
        {
            Id = 1,
            Admin = true,
            Employee = true,
        };

        User user2 = new User()
        {
            Id = 1,
            Name = "Lewis",
            Title = "Driver",
            Roles = role2,
            Email = "lh44@gmail.com",
            Password = "44444444"
        };

        users.Add(user1);
        users.Add(user2);
        List<User> expected = users;
        var actual = userValidation.getUsers(name, title, email, password, users);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void TestGetCustomersByName()
    {
        string name = "Lewis";
        string title = null!;
        string email = null!;
        string password = null!;
        List<User> users = new List<User>();
        Role role1 = new Role()
        {
            Id = 1,
            Admin = true,
            Employee = true,
        };

        User user1 = new User()
        {
            Id = 1,
            Name = "Troy",
            Title = "Dev",
            Roles = role1,
            Email = "tf@gmail.com",
            Password = "12345678"
        };
        
        Role role2 = new Role()
        {
            Id = 1,
            Admin = true,
            Employee = true,
        };

        User user2 = new User()
        {
            Id = 1,
            Name = "Lewis",
            Title = "Driver",
            Roles = role2,
            Email = "lh44@gmail.com",
            Password = "44444444"
        };

        users.Add(user1);
        users.Add(user2);
        List<User> expected = new List<User>();
        expected.Add(user2);
        var actual = userValidation.getUsers(name, title, email, password, users);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void TestGetUsersByTitle()
    {
        string name = null!;
        string title = "Driver";
        string email = null!;
        string password = null!;
        List<User> users = new List<User>();
        Role role1 = new Role()
        {
            Id = 1,
            Admin = true,
            Employee = true,
        };

        User user1 = new User()
        {
            Id = 1,
            Name = "Troy",
            Title = "Dev",
            Roles = role1,
            Email = "tf@gmail.com",
            Password = "12345678"
        };
        
        Role role2 = new Role()
        {
            Id = 1,
            Admin = true,
            Employee = true,
        };

        User user2 = new User()
        {
            Id = 1,
            Name = "Lewis",
            Title = "Driver",
            Roles = role2,
            Email = "lh44@gmail.com",
            Password = "44444444"
        };

        users.Add(user1);
        users.Add(user2);
        List<User> expected = new List<User>();
        expected.Add(user2);
        var actual = userValidation.getUsers(name, title, email, password, users);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void TestGetCustomersByEmail()
    {
        string name = null!;
        string title = null!;
        string email = "tf@gmail.com";
        string password = null!;
        List<User> users = new List<User>();
        Role role1 = new Role()
        {
            Id = 1,
            Admin = true,
            Employee = true,
        };

        User user1 = new User()
        {
            Id = 1,
            Name = "Troy",
            Title = "Dev",
            Roles = role1,
            Email = "tf@gmail.com",
            Password = "12345678"
        };
        
        Role role2 = new Role()
        {
            Id = 1,
            Admin = true,
            Employee = true,
        };

        User user2 = new User()
        {
            Id = 1,
            Name = "Lewis",
            Title = "Driver",
            Roles = role2,
            Email = "lh44@gmail.com",
            Password = "44444444"
        };

        users.Add(user1);
        users.Add(user2);
        List<User> expected = new List<User>();
        expected.Add(user1);
        var actual = userValidation.getUsers(name, title, email, password, users);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void TestGetUsersByPassword()
    {
        string name = null!;
        string title = null!;
        string email = null!;
        string password = "12345678";
        List<User> users = new List<User>();
        Role role1 = new Role()
        {
            Id = 1,
            Admin = true,
            Employee = true,
        };

        User user1 = new User()
        {
            Id = 1,
            Name = "Troy",
            Title = "Dev",
            Roles = role1,
            Email = "tf@gmail.com",
            Password = "12345678"
        };
        
        Role role2 = new Role()
        {
            Id = 1,
            Admin = true,
            Employee = true,
        };

        User user2 = new User()
        {
            Id = 1,
            Name = "Lewis",
            Title = "Driver",
            Roles = role2,
            Email = "lh44@gmail.com",
            Password = "44444444"
        };

        users.Add(user1);
        users.Add(user2);
        List<User> expected = new List<User>();
        expected.Add(user1);
        var actual = userValidation.getUsers(name, title, email, password, users);
        Assert.Equal(expected, actual);
    }

        [Fact]
    public void TestGetUsersByAdmin()
    {

        List<User> users = new List<User>();
        Role role1 = new Role()
        {
            Id = 1,
            Admin = true,
            Employee = true,
        };

        User user1 = new User()
        {
            Id = 1,
            Name = "Troy",
            Title = "Dev",
            Roles = role1,
            Email = "tf@gmail.com",
            Password = "12345678"
        };
        
        Role role2 = new Role()
        {
            Id = 2,
            Admin = false,
            Employee = true,
        };

        User user2 = new User()
        {
            Id = 2,
            Name = "Lewis",
            Title = "Driver",
            Roles = role2,
            Email = "lh44@gmail.com",
            Password = "44444444"
        };

        users.Add(user1);
        users.Add(user2);
        List<User> expected = new List<User>();
        expected.Add(user1);
        var actual = userValidation.getUsersAdmin(true, users);
        Assert.Equal(expected, actual);
    }
    
    [Fact]
    public void TestGetUsersByEmployee()
    {

        List<User> users = new List<User>();
        Role role1 = new Role()
        {
            Id = 1,
            Admin = true,
            Employee = true,
        };

        User user1 = new User()
        {
            Id = 1,
            Name = "Troy",
            Title = "Dev",
            Roles = role1,
            Email = "tf@gmail.com",
            Password = "12345678"
        };
        
        Role role2 = new Role()
        {
            Id = 2,
            Admin = false,
            Employee = true,
        };

        User user2 = new User()
        {
            Id = 2,
            Name = "Lewis",
            Title = "Driver",
            Roles = role2,
            Email = "lh44@gmail.com",
            Password = "44444444"
        };

        users.Add(user1);
        users.Add(user2);
        List<User> expected = new List<User>();
        expected.Add(user2);
        var actual = userValidation.getUsersAdmin(false, users);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void TestPassword()
    {
        string password = "12345678";
        var expected = true;
        var actual = userValidation.validPassword(password);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void TestPassword2()
    {
        string password = "1234";
        var expected = false;
        var actual = userValidation.validPassword(password);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void TestEmail()
    {
        string email= "tf@gmail.com";
        var expected = true;
        var actual = userValidation.validEmail(email);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void TestEmail2()
    {
        string email = "tfgmail.h";
        var expected = false;
        var actual = userValidation.validEmail(email);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void TestUniqueEmail()
    {
        List<User> users = new List<User>();
         Role role1 = new Role()
        {
            Id = 1,
            Admin = true,
            Employee = true,
        };

        User user1 = new User()
        {
            Id = 1,
            Name = "Troy",
            Title = "Dev",
            Roles = role1,
            Email = "tf@gmail.com",
            Password = "12345678"

        };
        
        Role role2 = new Role()
        {
            Id = 1,
            Admin = true,
            Employee = true,
        };

        User user2 = new User()
        {
            Id = 1,
            Name = "Lewis",
            Title = "Driver",
            Roles = role2,
            Email = "lh44@gmail.com",
            Password = "44444444"
        };

        Role role3 = new Role()
        {
            Id = 3,
            Admin = true,
            Employee = true,
        };

        User user3 = new User()
        {
            Id = 3,
            Name = "Daniel",
            Title = "Driver",
            Roles = role3,
            Email = "dr3@gmail.com",
            Password = "44444444"
        };

        users.Add(user1);
        users.Add(user2);
        var expected = true;
        var actual = userValidation.emailIsUnique(user3, users);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void TestUniqueEmail2()
    {
        List<User> users = new List<User>();
         Role role1 = new Role()
        {
            Id = 1,
            Admin = true,
            Employee = true,
        };

        User user1 = new User()
        {
            Id = 1,
            Name = "Troy",
            Title = "Dev",
            Roles = role1,
            Email = "tf@gmail.com",
            Password = "12345678"

        };
        
        Role role2 = new Role()
        {
            Id = 1,
            Admin = true,
            Employee = true,
        };

        User user2 = new User()
        {
            Id = 1,
            Name = "Lewis",
            Title = "Driver",
            Roles = role2,
            Email = "lh44@gmail.com",
            Password = "44444444"
        };

        Role role3 = new Role()
        {
            Id = 3,
            Admin = true,
            Employee = true,
        };

        User user3 = new User()
        {
            Id = 3,
            Name = "Daniel",
            Title = "Driver",
            Roles = role3,
            Email = "lh44@gmail.com",
            Password = "44444444"
        };

        users.Add(user1);
        users.Add(user2);
        var expected = false;
        var actual = userValidation.emailIsUnique(user3, users);
        Assert.Equal(expected, actual);
    }    

    [Fact]
    public void TestMatchingIds()
    {
        List<User> users = new List<User>();
         Role role1 = new Role()
        {
            Id = 1,
            Admin = true,
            Employee = true,
        };

        User user1 = new User()
        {
            Id = 1,
            Name = "Troy",
            Title = "Dev",
            Roles = role1,
            Email = "tf@gmail.com",
            Password = "12345678"

        };

        int id = 1;
        var expected = true;
        var actual = userValidation.matchingIds(id, user1);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void TestMatchingIds2()
    {
        List<User> users = new List<User>();
         Role role1 = new Role()
        {
            Id = 1,
            Admin = true,
            Employee = true,
        };

        User user1 = new User()
        {
            Id = 1,
            Name = "Troy",
            Title = "Dev",
            Roles = role1,
            Email = "tf@gmail.com",
            Password = "12345678"

        };

        int id = 864;
        var expected = false;
        var actual = userValidation.matchingIds(id, user1);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void TestUserExists()
    {
        List<User> users = new List<User>();
        Role role1 = new Role()
        {
            Id = 1,
            Admin = true,
            Employee = true,
        };

        User user1 = new User()
        {
            Id = 1,
            Name = "Troy",
            Title = "Dev",
            Roles = role1,
            Email = "tf@gmail.com",
            Password = "12345678"

        };

        users.Add(user1);
        var expected = true;
        var actual = userValidation.UserExists(user1.Id, users);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void TestUserExists2()
    {
        List<User> users = new List<User>();
        Role role1 = new Role()
        {
            Id = 1,
            Admin = true,
            Employee = true,
        };

        User user1 = new User()
        {
            Id = 1,
            Name = "Troy",
            Title = "Dev",
            Roles = role1,
            Email = "tf@gmail.com",
            Password = "12345678"

        };

        users.Add(user1);
        var expected = false;
        var actual = userValidation.UserExists(55, users);
        Assert.Equal(expected, actual);
    }

}
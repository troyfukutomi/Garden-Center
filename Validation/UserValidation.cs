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
    public class UserValidation
    {
        private readonly DatabaseContext _context;

        public UserValidation(DatabaseContext context)
        {
            _context = context;
        }

        public List<User> getUsers(string? name, string? title, string? email, string? password, List<User> users)
        {
            foreach (var u in users.ToList())
            {
                if (name != null && name != u.Name)
                {
                    users.Remove(u);
                }
                if (title != null && title != u.Title)
                {
                    users.Remove(u);
                }
                if (email != null && email != u.Email)
                {
                    users.Remove(u);
                }
                if (password!= null && password != u.Password)
                {
                    users.Remove(u);
                }
            }
            return users;
        }

        public List<User> getUsersAdmin(bool admin, List<User> users)
        {
            
            foreach (var u in users.ToList())
            {
                if (admin != u.Roles!.Admin)
                {
                    users.Remove(u);
                }
            }  

            return users;
        }

        public List<User> getUsersEmployee(bool employee, List<User> users)
        {
            
            foreach (var u in users.ToList())
            {
                if (employee != u.Roles!.Employee)
                {
                    users.Remove(u);
                }
            }  

            return users;
        }

        public bool validPassword(string password)
        {
            if (password.Length >= 8)
            {
                return true;
            } else 
            {
                return false;
            }
        }

        public bool validEmail(string email)
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
        
        public bool emailIsUnique(User user,List<User> users)
        {

            foreach (var c in users.ToList())
            {
                if (c.Email == user.Email && c.Id != user.Id)
                {
                    return false;
                }
            }
            return true;
        }

        public bool matchingIds(long id, User user)
        {
            if (id == user.Id)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool UserExists(int id)
        {
            return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
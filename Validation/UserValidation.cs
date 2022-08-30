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
    public class UserValidation
    {
        private readonly DatabaseContext _context;

        /// <summary>
        /// Brings in the database for us to use in our validation class
        /// </summary>
        /// <param name="context"></param>
        public UserValidation(DatabaseContext context)
        {
            _context = context;
        }

        GardenCenter.Logging.Logger logger = new GardenCenter.Logging.Logger();

        /// <summary>
        /// gets all users, can take on optional parameters to query by
        /// </summary>
        /// <param name="name">name of the user</param>
        /// <param name="title">user's title</param>
        /// <param name="email">user's email</param>
        /// <param name="password">user's password</param>
        /// <param name="users">list of users</param>
        /// <returns>list of users</returns>
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

        /// <summary>
        /// gets all users via admin status
        /// </summary>
        /// <param name="admin">user's admin status</param>
        /// <param name="users">list of users</param>
        /// <returns>list of users </returns>
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

        /// <summary>
        /// gets all users via employee status
        /// </summary>
        /// <param name="employee">usr's employee status</param>
        /// <param name="users">list of users</param>
        /// <returns>list of users</returns>
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

        /// <summary>
        /// checks to make sure a given password is 8 characters or more, any less will result in an error
        /// </summary>
        /// <param name="password">password being checked</param>
        /// <returns>true is password is 8 or more characters</returns>
        public bool validPassword(string password)
        {
            if (password.Length >= 8)
            {
                return true;
            } else 
            {
                logger.Log("Error: Password must be 8 or more characters");
                return false;
            }
        }

        /// <summary>
        /// checks to make sure email is properlyu formatted
        /// </summary>
        /// <param name="email">email being checked</param>
        /// <returns>true if email is in proper format</returns>
        public bool validEmail(string email)
        {
            Regex emailRegex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            if (emailRegex.IsMatch(email))
            {
                return true;
            } else
            {
                logger.Log("Error: Email is not in proper format");
                return false;
            }
        }
        
        /// <summary>
        /// checks to make sure there are no duplicate emails in the database
        /// </summary>
        /// <param name="user">user being updated</param>
        /// <param name="users">list of users top check against</param>
        /// <returns>true if email is one of a kind</returns>
        public bool emailIsUnique(User user,List<User> users)
        {

            foreach (var c in users.ToList())
            {
                if (c.Email == user.Email && c.Id != user.Id)
                {
                    logger.Log("Error: Email has already been taken");
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// checks to make sure id in query matches the is of the user being updated
        /// </summary>
        /// <param name="id">id from the path</param>
        /// <param name="user">user being updated</param>
        /// <returns>true if both ids match</returns>
        public bool matchingIds(long id, User user)
        {
            if (id == user.Id)
            {
                return true;
            }
            else
            {
                logger.Log("Error: Id's did not match");
                return false;
            }
        }

        /// <summary>
        /// checks the databse to make sure the user does exist
        /// </summary>
        /// <param name="id">id of the user being checked</param>
        /// <returns>true if user exists</returns>
        public bool UserExists(int id)
        {
            return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
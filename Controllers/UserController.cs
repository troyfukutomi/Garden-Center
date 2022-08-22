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

namespace GardenCenter.Controllers
{
    /// <summary>
    /// Controller for the User entity
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DatabaseContext _context;

        /// <summary>
        ///  Defines the database context as _context for future use in all the methods
        /// </summary>
        public UserController(DatabaseContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get method for users that can include parameters that can be queried 
        /// </summary>
        /// <param name="name">Name of the user</param>
        /// <param name="title">User's title</param>
        /// <param name="email">User's email</param>
        /// <param name="password">User's password</param>
        /// <returns>List of users</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers(string? name, string? title, string? email, string? password)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }

            //roles is also called here as it is a nested object
            //without the call, roles will be empty whenever users is called.
            UserValidation userValidation = new UserValidation(_context);
            var users = await _context.Users.ToListAsync();
            var roles = await _context.Roles!.ToListAsync();

            return userValidation.getUsers(name, title, email, password, users);
        }

        /// <summary>
        /// Get method for users, queries by User's admin status
        /// </summary>
        /// <param name="admin">Admin status being queried</param>
        /// <returns>List of users who are active admins</returns>
        [HttpGet("Roles/Admin/{admin?}")]
        public async Task<ActionResult<IEnumerable<User>>> GetUserAdmin(bool admin = true)
        {
            //roles is also called here as it is a nested object
            //without the call, roles will be empty whenever users is called.
            UserValidation userValidation = new UserValidation(_context);
            var users = await _context.Users.ToListAsync();
            var roles = await _context.Roles!.ToListAsync();

            return userValidation.getUsersAdmin(admin, users);
        }

        /// <summary>
        /// Get method for users, queries by User's employyee status
        /// </summary>
        /// <param name="employee">Employee status being queried</param>
        /// <returns>List of users who are active employees</returns>
        [HttpGet("Roles/Employee/{employee?}")]
        public async Task<ActionResult<IEnumerable<User>>> GetUserEmployee(bool employee = false)
        {
            //roles is also called here as it is a nested object
            //without the call, roles will be empty whenever users is called.
            UserValidation userValidation = new UserValidation(_context);
            var users = await _context.Users.ToListAsync();
            var roles = await _context.Roles!.ToListAsync();

            return userValidation.getUsersAdmin(employee, users);
        }

        /// <summary>
        /// Get method for users by their Id
        /// </summary>
        /// <param name="id">Id of the user being fetched</param>
        /// <returns>Single User</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
          if (_context.Users == null)
          {
              return NotFound();
          }
            //roles is also called here as it is a nested object
            //without the call, roles will be empty whenever users is called.
            var user = await _context.Users.FindAsync(id);
            var roles = await _context.Roles!.ToListAsync();

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        /// <summary>
        /// Put method for users
        /// </summary>
        /// <param name="id">Id of the user being updated</param>
        /// <param name="user">user with new updated fields</param>
        /// <returns>No content</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
          
            //Validation checks, validation methods are located in the validation
            //folder and called here.
            UserValidation userValidation = new UserValidation(_context);
            var users = await _context.Users.ToListAsync();
            bool matchingIds = userValidation.matchingIds(id, user);
            bool validEmail = userValidation.validEmail(user.Email!);
            bool validPassword = userValidation.validPassword(user.Password!);
            bool uniqueEmail = userValidation.emailIsUnique(user, users);
            bool UserExists = userValidation.UserExists(user.Id);

            if (_context.Users == null)
            {
                return NotFound();
            }
            
            if (!matchingIds)
            {
                return BadRequest("ID in query does not match order being altered");
            }

            if (!UserExists)
            {
                return NotFound("ID in query does not match user being altered");
            }

            if (!validPassword)
            {
                return BadRequest("Password must have 8 or more characters");
            } 

            if (!validEmail)
            {
                return BadRequest("Email must be in proper email format");
            } 

            if (!uniqueEmail)
            {
                return Conflict("Email has already been taken, choose another email.");
            }

            //all validation checks must pass before being updated
            if (matchingIds && UserExists && validEmail && validPassword && uniqueEmail)
            {
                _context.ChangeTracker.Clear();
                _context.Entry(user).State = EntityState.Modified;
                _context.Entry(user.Roles).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok();
            }

            return BadRequest("User is invalid. Try again.");
        }

        /// <summary>
        /// Post method for users
        /// </summary>
        /// <param name="user">new user being added to the database</param>
        /// <returns>CreatedAtActionResult</returns>
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            
            //Validation checks, validation methods are located in the validation
            //folder and called here.
            UserValidation userValidation = new UserValidation(_context);
            var roles = await _context.Roles!.ToListAsync();
            var users = await _context.Users.ToListAsync();
            bool validEmail = userValidation.validEmail(user.Email!);
            bool validPassword = userValidation.validPassword(user.Password!);
            bool uniqueEmail = userValidation.emailIsUnique(user, users);
            bool UserExists = userValidation.UserExists(user.Id);

            if (!uniqueEmail)
            {
                return Conflict("Email has already been taken, choose another email.");
            }

            if (!validPassword)
            {
                return BadRequest("Password must have 8 or more characters");
            } 

            if (!validEmail)
            {
                return BadRequest("Email must be in proper email format");
            } 

            //all validation checks must pass before being posted
            if (uniqueEmail && validEmail && validPassword)
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
            }
            
            return BadRequest("User is invalid. Try again");
        }

        /// <summary>
        /// Delete method for users
        /// </summary>
        /// <param name="id">Id of the user being deleted</param>
        /// <returns>No content</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions; 
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GardenCenter.Models;

namespace GardenCenter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public UserController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: api/User
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers(string? name, string? title, string? email, string? password)
        {
          if (_context.Users == null)
          {
              return NotFound();
          }

          var users = await _context.Users.ToListAsync();
          var roles = await _context.Roles.ToListAsync();

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

        [HttpGet("Roles/Admin/{admin?}")]
        public async Task<ActionResult<IEnumerable<User>>> GetUserAdmin(bool admin = true)
        {
          var users = await _context.Users.ToListAsync();
          var roles = await _context.Roles.ToListAsync();

          foreach (var u in users.ToList())
          {
            if (admin != u.Roles.Admin)
            {
                users.Remove(u);
            }
          }  

          return users;
        }

        [HttpGet("Roles/Employee/{employee?}")]
        public async Task<ActionResult<IEnumerable<User>>> GetUserEmployee(bool employee = false)
        {
          var users = await _context.Users.ToListAsync();
          var roles = await _context.Roles.ToListAsync();

          foreach (var u in users.ToList())
          {
            if (employee != u.Roles.Employee)
            {
                users.Remove(u);
            }
          }  

          return users;
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
          if (_context.Users == null)
          {
              return NotFound();
          }
            var user = await _context.Users.FindAsync(id);
            var roles = await _context.Roles.ToListAsync();

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/User/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest("ID in query does not match order being altered");
            }

            var users = await _context.Users.ToListAsync();
            
            bool validEmail = false;
            bool validPassword = false;
            Regex emailRegex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");

            if (_context.Users == null)
            {
                return Problem("Entity set 'DatabaseContext.Users'  is null.");
            }

            if (!UserExists(id))
            {
                return NotFound("ID in query does not match user being altered");
            }

            if (user.Password.Length >= 8)
            {
                validPassword = true;
            } else
            {
                return BadRequest("Password must have 8 or more characters");    
            }

            if (emailRegex.IsMatch(user.Email))
            {
                validEmail = true;
            } else
            {
                return BadRequest("Email must be in proper email format");
            }

            foreach (var u in users)
            {
                if (u.Email == user.Email && u.Id != user.Id)
                {
                    return Conflict("Email has already been taken, try again");
                }
            }

            if (validEmail && validPassword)
            {
                _context.ChangeTracker.Clear();
                _context.Entry(user).State = EntityState.Modified;
                _context.Entry(user.Roles).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok();
            }

            return BadRequest("User is invalid. Try again.");
        }

        // POST: api/User
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'DatabaseContext.Users'  is null.");
            }

            var users = await _context.Users.ToListAsync();
            var roles = await _context.Roles.ToListAsync();
            bool validEmail = false;
            bool validPassword = false;
            Regex emailRegex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            foreach (var u in users)
            {
                if (u.Email == user.Email)
                {
                    return Conflict("Email has already been taken, try again");
                }
            }

            if (user.Password.Length >= 8)
            {
                validPassword = true;
            } else
            {
                return BadRequest("Password must have 8 or more characters");    
            }

            if (emailRegex.IsMatch(user.Email))
            {
                validEmail = true;
            } else
            {
                return BadRequest("Email must be in proper email format");
            }

            if (validEmail && validPassword)
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
            }
            
                return BadRequest("User is invalid. Try again");
        }

        // DELETE: api/User/5
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

        private bool UserExists(int id)
        {
            return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

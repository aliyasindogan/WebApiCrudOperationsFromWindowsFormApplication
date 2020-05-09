using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserContext _context;

        public UsersController(UserContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet("getall")]
        public IActionResult GetList()
        {
            var result = _context.Users.ToList();
            if (result.Count > 0)
            {
                return Ok(result);
            }
            return BadRequest(Messsages.MessageError);
        }

        [HttpGet("getbyid")]
        public IActionResult GetById(int id)
        {
            var result = _context.Users.Find(id);
            if (result != null)
            {
                return Ok(result);
            }

            return BadRequest(Messsages.MessageError);
        }

        [HttpPost("add")]
        public IActionResult Add(User user)
        {
            var result = _context.Users.Add(user);
            _context.SaveChanges();
            if (result != null)
            {
                return Ok(Messsages.UserAdded);
            }

            return BadRequest(Messsages.MessageError);
        }

        [HttpPost("add2")]
        public IActionResult Add2(User user)
        {
            var result = _context.Users.Add(user);
            _context.SaveChanges();
            if (user != null)
            {
                return Ok(user);
            }

            return BadRequest(Messsages.MessageError);
        }

        [HttpPost("update")]
        public IActionResult Update(User user)
        {
            var result = _context.Users.Update(user);
            _context.SaveChanges();
            if (result != null)
            {
                return Ok(Messsages.UserUpdated);
            }

            return BadRequest(Messsages.MessageError);
        }

        [HttpPost("delete")]
        public IActionResult Delete(User user)
        {
            var result = _context.Users.Remove(user);
            _context.SaveChanges();
            if (result != null)
            {
                return Ok(Messsages.UserDeleted);
            }

            return BadRequest(Messsages.MessageError);
        }
    }
}
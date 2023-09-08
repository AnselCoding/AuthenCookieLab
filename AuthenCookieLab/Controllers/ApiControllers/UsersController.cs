using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuthenCookieLab.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace AuthenCookieLab.Controllers.ApiControllers
{
    //[Authorize(Roles = "Admin, User")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly LabDBContext _context;

        public UsersController(LabDBContext context)
        {
            _context = context;
        }

        // POST: api/Users/Login
        [HttpPost]
        public async Task<ActionResult<User>> Login(User tempUser)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }

            //登入失敗： 回傳 404
            // 使用者不存在
            // 密碼錯誤
            var user = await _context.Users.FirstOrDefaultAsync(x=> x.Name == tempUser.Name);
            if (user == null ||
                user.Password != tempUser.Password)
            {
                return NotFound();
            }

            // 登入成功： 回傳 200，使用者物件
            // 建立 Cookie 認證

            return Ok(user);
        }

        // GET: api/Users/AdminCheck
        //[Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult AdminCheck()
        {
            // 取 cookie 使用者資料
            //var userId = User.FindFirstValue("Id");
            //return Ok(userId);
            return Ok();
        }

        // GET: api/Users/UserCheck
        [HttpGet]
        public ActionResult UserCheck()
        {
            // 取 cookie 使用者資料
            //var userId = User.FindFirstValue("Id");
            //return Ok(userId);
            return Ok();
        }

        // GET: api/Users/Logout
        [HttpGet]
        public async Task<ActionResult> Logout()
        {
            //await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return Ok();
        }



        //// PUT: api/Users/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutUser(int id, User user)
        //{
        //    if (id != user.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(user).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!UserExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //// POST: api/Users
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<User>> PostUser(User user)
        //{
        //  if (_context.Users == null)
        //  {
        //      return Problem("Entity set 'LabDBContext.Users'  is null.");
        //  }
        //    _context.Users.Add(user);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetUser", new { id = user.Id }, user);
        //}

        //// DELETE: api/Users/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteUser(int id)
        //{
        //    if (_context.Users == null)
        //    {
        //        return NotFound();
        //    }
        //    var user = await _context.Users.FindAsync(id);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Users.Remove(user);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        //private bool UserExists(int id)
        //{
        //    return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        //}
    }
}

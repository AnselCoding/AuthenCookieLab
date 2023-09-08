using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuthenCookieLab.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;

namespace AuthenCookieLab.Controllers.ApiControllers
{
    [Authorize(Roles = "Admin, User")]
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
        [AllowAnonymous]
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
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Name == tempUser.Name);
            if (user == null ||
                user.Password != tempUser.Password)
            {
                return NotFound();
            }

            // 登入成功： 回傳 200，使用者物件
            // 建立 Cookie 認證
            await ContextSignIn(user);

            return Ok(user);
        }

        private async Task ContextSignIn(User? user)
        {
            var claims = new List<Claim>()
            {
                new Claim("Id", user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.MobilePhone, user.MobilePhone),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }

        // GET: api/Users/AdminCheck
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult AdminCheck()
        {
            // 取 cookie 使用者資料
            var userId = User.FindFirstValue(ClaimTypes.Email);
            return Ok(userId);
        }

        // GET: api/Users/UserCheck
        [HttpGet]
        public ActionResult UserCheck()
        {
            // 取 cookie 使用者資料
            var userId = User.FindFirstValue("Id");
            return Ok(userId);
        }

        // GET: api/Users/Logout
        [HttpGet]
        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return Ok();
        }        
    }
}

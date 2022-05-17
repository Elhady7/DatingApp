using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using APi.Data;
using APi.DTOs;
using APi.Entities;
using Microsoft.AspNetCore.Mvc;

namespace APi.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        public AccountController(DataContext context)
        {
            _context = context;

        }

        [HttpPost("register")]
        public async Task<ActionResult<AppUser>> Register(RegisterDTO registerDTO )
        {
            using var hmac = new HMACSHA512();

            var user = new AppUser()
            {
                UserName = registerDTO.UserName,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDTO.password)),
                SaltHash = hmac.Key
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;

        }
        
    }
}
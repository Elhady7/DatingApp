using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using APi.Data;
using APi.DTOs;
using APi.Entities;
using APi.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APi.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;
        public AccountController(DataContext context, ITokenService tokenService)
        {
            _tokenService = tokenService;
            _context = context;

        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDTO)
        {
            if (await ExistUser(registerDTO.UserName))
                return BadRequest("This User Name is taken");
            using var hmac = new HMACSHA512();

            var user = new AppUser()
            {
                UserName = registerDTO.UserName.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDTO.password)),
                SaltHash = hmac.Key
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return new UserDTO 
            {
                UserName = user.UserName,
                Token=  _tokenService.CreateToken(user)

            } ;

        }
        [HttpPost("login")]

        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == loginDTO.UserName);
            if (user == null)
                return Unauthorized("Invalid User");
            var hmac = new HMACSHA512(user.SaltHash);
            var ComputedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDTO.password));
            for (int i = 0; i < ComputedHash.Length; i++)
            {
                if (ComputedHash[i] != user.PasswordHash[i])
                    return Unauthorized("password not valid ");
            }
            return new UserDTO 
            {
                UserName=user.UserName,
                Token=_tokenService.CreateToken(user)

            };

        }
        public async Task<bool> ExistUser(string UserName)
        {
            return await _context.Users.AnyAsync(x => x.UserName == UserName.ToLower());
        }

    }
}
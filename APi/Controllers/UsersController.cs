using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APi.Data;
using APi.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APi.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class UsersController : ControllerBase
    {
        private readonly DataContext _context;
        public UsersController(DataContext context)
        {
            _context = context;

        }
 [HttpGet]
  public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers ()
  {
      return await _context.Users.ToListAsync();
  }
    
    [HttpGet("{id}")]
  public async Task<ActionResult<AppUser>> GetUser (int id)
  {
      
      return await  _context.Users.FindAsync(id);
  }
    
 
}
}
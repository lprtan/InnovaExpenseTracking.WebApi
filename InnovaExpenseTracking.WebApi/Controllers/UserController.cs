using InnovaExpenseTracking.WebApi.Context;
using InnovaExpenseTracking.WebApi.Dtos;
using InnovaExpenseTracking.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;

namespace InnovaExpenseTracking.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]

    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles ="admin")]
        [HttpGet]
        public async Task<IActionResult> GetUser(CancellationToken cancellationToken)
        {
            List<User> users = new List<User>();

            try
            {
                await _context.ExecuteTransactionAsync(async () =>
                {
                    users = await _context.Users.ToListAsync(cancellationToken);
                }, cancellationToken);
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = ex.Message });
            }
        }

        [Authorize]
        [HttpDelete("{email}")]
        public async Task<IActionResult> DeleteUser(string email, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest(new { Message = "Lütfen bir E mail giriniz" });
            }

            var user = await _context.Users.FirstOrDefaultAsync(p => p.Email == email);

            if (user == null)
            {
                return BadRequest(new { Message = "Böyle bir kullanıcı bulunamadı" });
            }

            _context.Users.Remove(user);

            await _context.SaveChangesAsync(cancellationToken);

            return Ok(user);
        }

        [Authorize]
        [HttpPut("{email}")]
        public async Task<IActionResult> UpdateUser(UserDto request, string email, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest(new { Message = "Lütfen bir E-mail giriniz" });
            }

            var user = await _context.Users.FirstOrDefaultAsync(p => p.Email == email);

            if (user == null)
            {
                return BadRequest(new { Message = "Böyle bir kullanıc bulunamadı" });
            }

            user.Name = request.Name;
            user.Email = request.Email;
            user.Password = request.Password;

            _context.Update(user);
            await _context.SaveChangesAsync(cancellationToken);

            return Ok(user);
        }
    }
}

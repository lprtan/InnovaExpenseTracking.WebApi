using InnovaExpenseTracking.WebApi.Context;
using InnovaExpenseTracking.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace InnovaExpenseTracking.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TotalExpenditureController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TotalExpenditureController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetTotalExpenditure(CancellationToken cancellationToken)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null || !Guid.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized();
            }

            var totalExpenditure = await _context.Transactions
                .Where(p => p.UserId == userId)
                .SumAsync(p => p.Amount, cancellationToken);

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return NotFound(new { Message = "Kullanıcı Bulunamadı" });
            }

            user.TotalExpenditure = totalExpenditure;
            await _context.SaveChangesAsync(cancellationToken);

            return Ok(new { TotalExpenditure = totalExpenditure.ToString("F2") });

        }
    }
}

using InnovaExpenseTracking.WebApi.Context;
using InnovaExpenseTracking.WebApi.Dtos;
using InnovaExpenseTracking.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace InnovaExpenseTracking.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TransactionController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetTransaction(CancellationToken cancellationToken)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null || !Guid.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized();
            }

            List<Transaction> transactions =
                await _context
                .Transactions.Where(p => p.UserId == userId)
                .OrderBy(p => p.ExpenditureDate)
                .ToListAsync(cancellationToken);

            return Ok(transactions);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddTransaction(TransactionDto request, CancellationToken cancellationToken)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null || !Guid.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized();
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
            if (user == null)
            {
                return Unauthorized();
            }

            Transaction transaction = new()
            {
                UserId = userId,
                Amount = request.Amount,
                Name = request.Name,
                ExpenditureDescription = request.ExpenditureDescription,
                ExpenditureDate = DateTime.UtcNow,
                User = user
            };

            user.TotalExpenditure += request.Amount;

            await _context.AddAsync(transaction, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return Ok(transaction);
        }

        [Authorize]
        [HttpDelete("{name}")]
        public async Task<IActionResult> DeleteTransaction(string name, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest(new { Message = "Lütfen bir işlem adı giriniz" });
            }

            var transaction = await _context.Transactions.FirstOrDefaultAsync(p => p.Name == name);

            if (transaction == null)
            {
                return BadRequest(new { Message = "Böyle bir işlem bulunamadı" });
            }

            _context.Transactions.Remove(transaction);

            await _context.SaveChangesAsync(cancellationToken);

            return Ok(transaction);
        }

        [Authorize]
        [HttpPut("{name}")]
        public async Task<IActionResult> UpdateTransaction(TransactionDto request, string name, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest(new { Message = "Lütfen bir işlem adı giriniz" });
            }

            var transaction = await _context.Transactions.FirstOrDefaultAsync(p => p.Name == name);

            if (transaction == null)
            {
                return BadRequest(new { Message = "Böyle bir işlem bulunamadı" });
            }

            transaction.Amount = request.Amount;
            transaction.Name = request.Name;
            transaction.ExpenditureDescription = request.ExpenditureDescription;
            transaction.ExpenditureDate = DateTime.UtcNow;

            _context.Update(transaction);
            await _context.SaveChangesAsync(cancellationToken);

            return Ok(transaction);
        }
    }
}

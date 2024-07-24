using Hangfire;
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
    public class HangfireController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public HangfireController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet]
        public async Task<decimal> GetUserTotalExpenses(CancellationToken cancellationToken)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null || !Guid.TryParse(userIdClaim, out var userId))
            {
                return 0;
            }

            var user = await _context.Users.FindAsync(userId, cancellationToken);
            if (user == null)
            {
                return 0;
            }

            var totalExpenditure = await _context.Transactions
                .Where(p => p.UserId == userId)
                .SumAsync(p => p.Amount, cancellationToken);

            user.TotalExpenditure = totalExpenditure;

            _context.Users.Update(user);
            await _context.SaveChangesAsync(cancellationToken);

            return totalExpenditure;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AggregateUserExpensesDaily(CancellationToken cancellationToken)
        {
            var totalExpenditure = await GetUserTotalExpenses(cancellationToken);

            RecurringJob.AddOrUpdate(
        "aggregate-user-expenses-job",
        () => Console.WriteLine($"Kullanıcının Günlük Toplam Masrafı: {totalExpenditure}"),
        Cron.Daily
        );

            return Ok(new { TotalExpenditure = totalExpenditure });

        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AggregateUserExpensesWeekly(CancellationToken cancellationToken)
        {
            var totalExpenditure = await GetUserTotalExpenses(cancellationToken);
            
            RecurringJob.AddOrUpdate(
        "aggregate-user-expenses-job",
        () => Console.WriteLine($"Kullanıcının Günlük Toplam Masrafı: {totalExpenditure}"),
        Cron.Weekly
        );

            return Ok(new { TotalExpenditure = totalExpenditure });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AggregateUserExpensesMonthly(CancellationToken cancellationToken)
        {
            var totalExpenditure = await GetUserTotalExpenses(cancellationToken);

            RecurringJob.AddOrUpdate(
         "aggregate-user-expenses-job",
          () => Console.WriteLine($"Kullanıcının Günlük Toplam Masrafı: {totalExpenditure}"),
          Cron.Monthly
         );

            return Ok(new { TotalExpenditure = totalExpenditure });
        }
    }
}

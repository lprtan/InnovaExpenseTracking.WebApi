using System.Text.Json.Serialization;

namespace InnovaExpenseTracking.WebApi.Models
{
    public sealed class Transaction
    {
        public Transaction()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public DateTime ExpenditureDate { get; set; }
        public string ExpenditureDescription { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        [JsonIgnore]
        public User User { get; set; }
    }
}

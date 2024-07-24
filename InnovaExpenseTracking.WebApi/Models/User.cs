namespace InnovaExpenseTracking.WebApi.Models
{
    public sealed class User
    {
        public User()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime CreatedTime { get; set; }
        public decimal TotalExpenditure { get; set; }
        public ICollection<Transaction> Transactions { get; set; }
    }
}

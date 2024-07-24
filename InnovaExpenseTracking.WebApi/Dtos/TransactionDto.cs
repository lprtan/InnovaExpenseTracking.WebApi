namespace InnovaExpenseTracking.WebApi.Dtos
{
    public sealed class TransactionDto
    {
        public decimal Amount { get; set; }
        public string Name { get; set; }
        public string ExpenditureDescription { get; set; } = string.Empty;

        public TransactionDto(decimal amount, string name, string expenditureDescription) 
        { 
            this.Amount = amount;
            this.Name = name;
            this.ExpenditureDescription = expenditureDescription;
        }
    }
}

namespace InnovaExpenseTracking.WebApi.Dtos
{
    public sealed class RegisterDto : BaseUserDto
    {
        public int Role { get; set; }

        public RegisterDto(string name, string email, string password, int role)
       : base(name, email, password)
        {
            this.Role = role;
        }
    }
}

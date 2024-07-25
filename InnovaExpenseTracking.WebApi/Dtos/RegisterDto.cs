namespace InnovaExpenseTracking.WebApi.Dtos
{
    public sealed class RegisterDto : BaseUserDto
    {
        public RegisterDto(string name, string email, string password)
       : base(name, email, password)
        {
        }
    }
}

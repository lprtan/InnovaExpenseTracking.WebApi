namespace InnovaExpenseTracking.WebApi.Dtos
{
    public sealed class UserDto : BaseUserDto
    {
        public UserDto(string name, string email, string password)
        : base(name, email, password)
        {
        }
    }
}

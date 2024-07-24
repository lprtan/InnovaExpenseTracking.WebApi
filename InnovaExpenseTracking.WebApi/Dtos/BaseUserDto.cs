namespace InnovaExpenseTracking.WebApi.Dtos
{
    public abstract class BaseUserDto
    {
        public string Name { get; init; }
        public string Email { get; init; }
        public string Password { get; init; }

        protected BaseUserDto(string name, string email, string password)
        {
            Name = name;
            Email = email;
            Password = password;
        }
    }
}

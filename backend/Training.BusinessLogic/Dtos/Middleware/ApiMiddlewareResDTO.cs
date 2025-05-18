using Training.DataAccess.Entities;

namespace Training.BusinessLogic.Dtos.Middleware
{
    public class ApiMiddlewareResDTO
    {
        public string Email { get; set; }
        public Role Role { get; set; }  

        public ApiMiddlewareResDTO(string email, Role role)
        {
            Email = email;
            Role = role;
        }
    }
}

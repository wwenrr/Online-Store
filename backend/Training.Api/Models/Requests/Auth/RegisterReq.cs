using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

namespace Training.Api.Models.Requests.Auth
{
    public class RegisterReq
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string RepeatPassword { get; set; }
    }
}

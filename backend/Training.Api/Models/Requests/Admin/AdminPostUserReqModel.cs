namespace Training.Api.Models.Requests.Admin
{
    public class AdminPostUserReqModel
    {
        public required string FirstName { get; set; }

        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required int Role { get; set; }
    }
}

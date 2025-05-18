namespace Training.Api.Models.Requests.Admin
{
    public class AdminPatchUserReqModel
    {
        public required string Email { get; set; }
        public required string Key { get; set; }
        public required string Value { get; set; }
    }
}

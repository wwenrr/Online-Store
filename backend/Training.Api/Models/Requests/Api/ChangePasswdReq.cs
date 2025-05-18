namespace Training.Api.Models.Requests.Api
{
    public class ChangePasswdReq
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string RepeatNewPassword { get; set; }
    }
}

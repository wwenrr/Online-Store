namespace Training.Api.Models.Requests.Api;

public class PatchGetInfoReqModel
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string CivilianId { get; set; }
    public string PhoneNumber { get; set; }
    public DateTimeOffset DateOfBirth { get; set; }
}

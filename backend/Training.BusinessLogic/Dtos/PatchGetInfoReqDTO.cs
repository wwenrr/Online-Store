namespace Training.BusinessLogic.Dtos;

public class PatchGetInfoReqDTO
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string CivilianId { get; set; }
    public string PhoneNumber { get; set; }
    public DateTimeOffset DateOfBirth { get; set; }
}

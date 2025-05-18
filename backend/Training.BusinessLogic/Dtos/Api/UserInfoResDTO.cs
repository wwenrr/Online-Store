using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Training.BusinessLogic.Dtos.Api
{
    public class UserInfoResDTO {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CivilianId { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public Role Role { get; set; }
        public DateTimeOffset? DateOfBirth { get; set; }
    }
    public enum Role
    {
        Admin = 1,
        Clerk = 2,
        Customer = 3,
    }
}

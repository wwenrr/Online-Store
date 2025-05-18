
using Training.DataAccess.Entities;

namespace Training.BusinessLogic.Dtos.Admin
{
    public class AdminPatchUserReqDTO
    {
        public long Id { get; set; }
        public string? Queryable { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public Training.DataAccess.Entities.Role Role { get; set; }

        public Boolean checkAttr() {
            if(string.IsNullOrEmpty(FirstName)) {
                return false;
            }
            if(string.IsNullOrEmpty(LastName)) {
                return false;
            }
            if(string.IsNullOrEmpty(Email)) {
                return false;
            }
            if(Role == 0) {
                return false;
            }
            return true;
        }
    }
}

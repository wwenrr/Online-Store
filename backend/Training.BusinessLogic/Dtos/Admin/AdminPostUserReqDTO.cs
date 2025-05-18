using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Training.BusinessLogic.Dtos.Admin
{
    public class AdminPostUserReqDTO
    {
        public required string FirstName { get; set; }

        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required int Role { get; set; }

        public string Queryable { get; set; }
        public string? Password { get; set; }
    }
}

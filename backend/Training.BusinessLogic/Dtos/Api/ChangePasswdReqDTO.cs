using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Training.BusinessLogic.Dtos.Api
{
    public class ChangePasswdReqDTO
    {
        public string NewPassword { get; set; }
        public string OldPassword { get; set; }
    }
}

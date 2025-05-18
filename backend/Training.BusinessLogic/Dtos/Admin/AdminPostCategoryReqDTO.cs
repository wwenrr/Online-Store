using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Training.BusinessLogic.Dtos.Admin
{
    public class AdminPostCategoryReqDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public string FileName { get; set; }
    }
}

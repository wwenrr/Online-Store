using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Training.BusinessLogic.Dtos.Clerk
{
    public class PostProductReqDTO
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required string Thumbnail { get; set; }
        public required float UnitPrice { get; set; }
        public long CategoryId { get; set; }

        public string? Email { get; set; }
    }
}

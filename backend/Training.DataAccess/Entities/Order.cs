using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Training.DataAccess.Entities
{
    [Table("Order")]
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [ForeignKey("ClerkId")]

        public User? Clerk { get; set; }
        public long? ClerkId { get; set; }

        [ForeignKey("CustomerId")]
        public User Customer { get; set; }
        public long CustomerId { get; set; }

        public DateTimeOffset CreateAt { get; set; }

        public bool IsDeleted { get; set; }
    }
}

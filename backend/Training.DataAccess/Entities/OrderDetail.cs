using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Training.DataAccess.Entities
{
    [Table("OrderDetail")]
    public class OrderDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [ForeignKey("OrderId")]
        public Order Order { get; set; }
        public long OrderId { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }
        public long ProductId { get; set; }

        public float UnitPrice { get; set; }

        public int Quantity { get; set; }
    }
}

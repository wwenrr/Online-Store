using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Training.DataAccess.Entities
{
    [Table("StockEvent")]
    public class StockEvent
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [ForeignKey("StockID")]
        public Stock Stock { get; set; }

        public long StockID { get; set; }

        public Type Type { get; set; }

        public string Reason { get; set; }

        public int Quantity { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
    }

    public enum Type
    {
        In = 1,
        Out = 2
    }
}

 using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Training.DataAccess.IEntities;

namespace Training.DataAccess.Entities
{
    [Table("Product")]
    public class Product : IIsDeletedEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Thumbnail { get; set; }

        public float UnitPrice { get; set; }

        [ForeignKey("CreateBy")]
        public User UserCreateBy { get; set; }
        public long CreateBy { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        [ForeignKey("UpdateBy")]
        public User UserUpdatedBy { get; set; }
        public long UpdateBy { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }

        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
        public long CategoryId { get; set; }

        public bool IsDeleted { get; set; }
    }
}

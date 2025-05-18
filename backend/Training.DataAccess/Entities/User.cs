using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Training.DataAccess.IEntities;

namespace Training.DataAccess.Entities
{
    [Table("User")]
    public class User : IIsDeletedEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [MaxLength(100)]
        public string? FirstName { get; set; }

        [MaxLength(100)]
        public string? LastName { get; set; }

        [MaxLength(100)]
        public string? CivilianId { get; set; }

        [MaxLength(255)]
        [MinLength(5)]
        public string Email { get; set; }

        [MaxLength(255)]
        [MinLength(15)]
        public string Password { get; set; }

        [MaxLength(15)]
        public string? PhoneNumber { get; set; }

        public DateTimeOffset? DateOfBirth { get; set; }

        public Role Role { get; set; }

        public bool IsDeleted { get; set; }
    }

    public enum Role
    {
        Admin=1,  
        Clerk=2,  
        Customer=3, 
    }
}

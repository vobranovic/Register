using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Register.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class Club
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        [StringLength(50)]
        public string City { get; set; }
        [Required]
        [Display(Name = "Date Created")]
        public DateTime DateCreated { get; set; } = DateTime.Now;
        [NotMapped]
        public string? Administrator { get; set; }


        public string UserId { get; set; }
        [ForeignKey("ClubId")]
        public List<Team> Teams { get; set; } = new List<Team>();
    }
}

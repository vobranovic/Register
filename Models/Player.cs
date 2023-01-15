using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Register.Models
{
    [Index(nameof(RegistrationId), IsUnique = true)]
    public class Player
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required]
        [StringLength(10)]
        [Display(Name = "Registration ID")]
        public string RegistrationId { get; set; }
        [Required]
        public DateTime Birthday { get; set; }

        [ForeignKey("PlayerId")]
        public List<TeamPlayer> TeamPlayer { get; set; } = new List<TeamPlayer>();

        [NotMapped]
        public int NumberOfTeams { get; set; }
    }
}

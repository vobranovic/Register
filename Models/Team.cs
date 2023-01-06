using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Register.Models
{
    public class Team
    {
        public int Id { get; set; }
        [Required]
        public Category Category { get; set; }
        [NotMapped]
        public string? Season { get; set; }

        public string UserId { get; set; }
        public int SeasonId { get; set; }
        public int ClubId { get; set; }
        [ForeignKey("TeamId")]
        public List<TeamPlayer> TeamPlayer { get; set; } = new List<TeamPlayer>();
        [ForeignKey("TeamId")]
        public List<TeamPersonnel> TeamPersonnel { get; set; } = new List<TeamPersonnel>();

        [NotMapped]
        public string? ClubName { get; set; }
    }
}

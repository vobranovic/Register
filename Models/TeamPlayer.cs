using System.ComponentModel.DataAnnotations.Schema;

namespace Register.Models
{
    public class TeamPlayer
    {
        public int Id { get; set; }
        public int TeamId { get; set; }
        public int PlayerId { get; set; }

        [NotMapped]
        public string? PlayerName { get; set; }
        [NotMapped]
        public string PlayerRegistrationId { get; set; }
        [NotMapped]
        public DateTime PlayerBirthday { get; set; }

        [NotMapped]
        public Category TeamCategory { get; set; }
        [NotMapped]
        public string? TeamSeason { get; set; }
        [NotMapped]
        public string? ClubName { get; set; }

    }
}

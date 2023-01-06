using System.ComponentModel.DataAnnotations.Schema;

namespace Register.Models
{
    public class TeamPersonnel
    {
        public int Id { get; set; }
        public int TeamId { get; set; }
        public int PersonnelId { get; set; }

        [NotMapped]
        public string? PersonnelName { get; set; }
        [NotMapped]
        public string PersonnelRegistrationId { get; set; }
        [NotMapped]
        public DateTime PersonnelBirthday { get; set; }

    }
}

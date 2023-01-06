using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Register.Models
{
    [Index(nameof(Year), IsUnique = true)]
    public class Season
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(7)]
        public string Year { get; set; }

        public List<Team> Teams { get; set; } = new List<Team>();

    }
}

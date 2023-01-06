using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Register.Models
{
    public enum Role
    {
        Coach,
        [Display(Name = "Assistant Coach")]
        AssistantCoach,
        Physio,
        [Display(Name = "Club Representative")]
        ClubRepresentative
    }
}

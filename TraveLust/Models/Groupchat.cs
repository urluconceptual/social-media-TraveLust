using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TraveLust.Models
{
    public class Groupchat
    {
        [Key]
        public int GroupchatId { get; set; }

        public int? ItineraryId { get; set; }
        public virtual Itinerary? Itinerary { get; set; }


        [Required(ErrorMessage = "Groupchat name is mandatory!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Groupchat description is mandatory!")]
        public string Description { get; set; }

        public string? CreatorId { get; set; }
        public virtual ApplicationUser? Creator { get; set; }

        public virtual ICollection<UserInGroupchat>? UserInGroupchats { get; set; }
        public virtual ICollection<Message>? Messages { get; set; }

        [NotMapped]
        public IEnumerable<SelectListItem>? AllUsers { get; set; }
    }
}

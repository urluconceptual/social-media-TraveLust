using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;

namespace TraveLust.Models
{
    public class Itinerary
    {
        public int ItineraryId { get; set; }

        public int GroupchatId { get; set; }
        public virtual Groupchat? Groupchat { get; set; }

        public int StayingPeriod;
        public virtual ICollection<PostInItinerary>? PostInItineraries { get; set; }

        public double Budget { get; set; }

        [NotMapped]
        public IEnumerable<SelectListItem>? TopPosts { get; set; }
    }
}

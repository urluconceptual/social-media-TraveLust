using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TraveLust.Models
{
    public class PostInItinerary
    {
        public int? PostId { get; set; }

        [Required(ErrorMessage = "Please select the itinerary for this sight!")]
        public int? ItineraryId { get; set; }

        public virtual Post? Post { get; set; }
        public virtual Itinerary? Itinerary { get; set; }

        public string? Description { get; set; }

        [NotMapped]
        public IEnumerable<SelectListItem>? AllItineraries { get; set; }
    }
}

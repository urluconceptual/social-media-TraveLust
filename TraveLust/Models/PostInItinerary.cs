using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;

namespace TraveLust.Models
{
    public class PostInItinerary
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? PostId { get; set; }
        public int? ItineraryId { get; set; }

        public virtual Post? Post { get; set; }
        public virtual Itinerary? Itinerary { get; set; }

        public string? Description { get; set; }
    }
}

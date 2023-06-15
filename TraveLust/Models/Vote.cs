using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TraveLust.Models
{
    public class Vote
    {
        public string? UserId { get; set; }
        public virtual ApplicationUser? User { get; set; }

        public int? PostId { get; set; }

        public int? ItineraryId { get; set; }

        [ForeignKey("PostId, ItineraryId")]
        public virtual PostInItinerary? PostInItinerary { get; set; }
    }
}

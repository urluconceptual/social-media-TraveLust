namespace TraveLust.Models
{
    public class Itinerary
    {
        public int ItineraryID { get; set; }

        public int GroupchatId { get; set; }
        public virtual Groupchat? Groupchat { get; set; }

        public int StayingPeriod;
        public virtual ICollection<PostInItinerary>? PostInItineraries { get; set; }

        public double Buget { get; set; }
    }
}

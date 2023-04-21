using System.ComponentModel.DataAnnotations;

namespace TraveLust.Models
{
    public class Review
    {
        [Key]
        public int ReviewId { get; set; }

        [Required(ErrorMessage = "Please tell us what you liked/disliked about this sight!")]
        public string Text { get; set; }

        [Required(ErrorMessage = "Please leave a rating for this sight!")]
        [Range(1, 10, ErrorMessage = "The rating has to be a number from 1 to 10!")]
        public int Rating { get; set; }

        public string? UserId { get; set; }
        public virtual ApplicationUser? User { get; set; }

        public int? PostId { get; set; }
        public virtual Post? Post { get; set; }

        public DateTime Date { get; set; }
    }
}

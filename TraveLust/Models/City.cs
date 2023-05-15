using System.ComponentModel.DataAnnotations;

namespace TraveLust.Models
{
    public class City
    {
        [Key]
        public int CityId { get; set; }

        [Required(ErrorMessage = "Please provide the name of the city!")]
        public string CityName { get; set; }

        [Required(ErrorMessage = "Please provide the name of the country!")]
        public string Country { get; set; }

        // anyoane can add a new city, but only an admin can approve it
        // if the city is approved by an admin, then it will be displayed on the website
        public bool Approved { get; set; }

        public virtual ICollection<Post>? Posts { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace TraveLust.Models
{
    public class City
    {
        [Key]
        public int CityId { get; set; }

        [Required(ErrorMessage = "PLease provide the name of the city!")]
        public string CityName { get; set; }

        [Required(ErrorMessage = "Please provide the name of the country!")]
        public string Country { get; set; }

        public virtual ICollection<Post>? Posts { get; set; }
    }
}

using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;

namespace TraveLust.Models
{
    public class UserInGroupchat
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? UserId { get; set; }
        public int? GroupchatId { get; set; }
        public virtual ApplicationUser? User { get; set; }
        public virtual Groupchat? Groupchat { get; set; }
    }
}

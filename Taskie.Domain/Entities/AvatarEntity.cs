using System.ComponentModel.DataAnnotations;

namespace Taskie.Domain.Entities
{
    public class AvatarEntity : BaseEntity
    {
        [Required]
        public string Desciption { get; set; }

        [Required]
        public string Image { get; set; }
    }
}

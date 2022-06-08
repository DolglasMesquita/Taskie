using System.ComponentModel.DataAnnotations;

namespace Taskie.Domain.Dto.Avatar
{
    public class AvatarCreateDto
    {
        [Required(ErrorMessage = "Insira uma descrição para representar o Avatar")]
        public string Desciption { get; set; }

        [Required(ErrorMessage = "Insira uma imagem para representar o Avatar")]
        public string Image { get; set; }
    }
}

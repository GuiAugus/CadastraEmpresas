using System.ComponentModel.DataAnnotations;

namespace CadastraEmpresas.Models
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "O e-mail e obrigatorio")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "A senha e obrigatoria")]
        public required string Senha { get; set; }
    }
}
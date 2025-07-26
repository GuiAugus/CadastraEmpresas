using System.ComponentModel.DataAnnotations;

namespace CadastraEmpresas.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O Nome e obrigatorio.")]
        public required string Nome { get; set; }

        [Required(ErrorMessage = "O e-mail e obrigatorio.")]
        [EmailAddress(ErrorMessage = "Formato de e-mail invalido.")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "A senha e obrigatoria")]
        public required string SenhaHash { get; set; }
    }
}
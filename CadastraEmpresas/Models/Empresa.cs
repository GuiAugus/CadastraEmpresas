using System.ComponentModel.DataAnnotations;

namespace CadastraEmpresas.Models
{
    public class Empresa
    {
        public int Id { get; set; }

        [Required]
        public required string NomeEmpresarial { get; set; }
        public string? NomeFantasia { get; set; }

        [Required]
        public required string Cnpj { get; set; }

        public string? Situacao { get; set; }

        public string? Abertura { get; set; }

        public string? Tipo { get; set; }

        public string? NaturezaJuridica { get; set; }

        public string? AtividadePrincipal { get; set; }

        public string? Logradouro { get; set; }

        public string? Numero { get; set; }

        public string? Complemento { get; set; }

        public string? Bairro { get; set; }

        public string? Municipio { get; set; }

        public string? Uf { get; set; }

        public string? Cep { get; set; }

        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }
    }
}
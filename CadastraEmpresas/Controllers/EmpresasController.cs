using CadastraEmpresas.Data;
using CadastraEmpresas.Models;
using CadastraEmpresas.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace CadastraEmpresas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EmpresasController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ReceitaWSService _receitaWSService;

        public EmpresasController(AppDbContext context, ReceitaWSService receitaWSService)
        {
            _context = context;
            _receitaWSService = receitaWSService;
        }

        [HttpPost("cadastrar")]
        public async Task<IActionResult> CadastrarEmpresa([FromBody] string cnpj)
        {
            string cnpjLimpo = Regex.Replace(cnpj, @"[^\d]", "");

            if (string.IsNullOrWhiteSpace(cnpjLimpo) || cnpjLimpo.Length != 14)
            {
                return BadRequest("CNPJ invalido. Deve ter 14 digitos.");
            }

            var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var empresaExistente = await _context.Empresas
                .Where(e => e.Cnpj == cnpjLimpo && e.UsuarioId == usuarioId)
                .FirstOrDefaultAsync();

            if (empresaExistente != null)
            {
                return BadRequest("Esta empresa ja foi cadastrada por voce.");
            }

            var dadosEmpresa = await _receitaWSService.ConsultarCnpj(cnpjLimpo);

            if (dadosEmpresa == null || string.IsNullOrWhiteSpace(dadosEmpresa.Cnpj))
            {
                return NotFound("Empresa nao encontrada ou CNPJ invalido na Receita Federal.");
            }

            var novaEmpresa = new Empresa
            {
                NomeEmpresarial = dadosEmpresa.Nome!, 
                NomeFantasia = dadosEmpresa.Fantasia,
                Cnpj = dadosEmpresa.Cnpj,
                Situacao = dadosEmpresa.Situacao,
                Abertura = dadosEmpresa.Abertura,
                Tipo = dadosEmpresa.Tipo,
                NaturezaJuridica = dadosEmpresa.Natureza_juridica,
                
                AtividadePrincipal = dadosEmpresa.Atividade_principal?.FirstOrDefault()?.Text,
                
                Logradouro = dadosEmpresa.Endereco?.Logradouro,
                Numero = dadosEmpresa.Endereco?.Numero,
                Complemento = dadosEmpresa.Endereco?.Complemento,
                Bairro = dadosEmpresa.Endereco?.Bairro,
                Municipio = dadosEmpresa.Endereco?.Municipio,
                Uf = dadosEmpresa.Endereco?.Uf,
                Cep = dadosEmpresa.Endereco?.Cep,
                UsuarioId = usuarioId
            };
            
            _context.Empresas.Add(novaEmpresa);
            await _context.SaveChangesAsync();

            return Ok(novaEmpresa);
        }
        
        [HttpGet("listar")]
        public async Task<IActionResult> ListarEmpresas()
        {
            var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            
            var empresas = await _context.Empresas
                .Where(e => e.UsuarioId == usuarioId)
                .ToListAsync();

            if (!empresas.Any())
            {
                return NotFound("Nenhuma empresa cadastrada para este usuário.");
            }

            return Ok(empresas);
        }
    }
}
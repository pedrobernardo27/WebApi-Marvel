using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using WebApiMarvelService.Interface;

namespace WebApiMarvel.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]

    public class PersonagemController : Controller
    {
        private readonly IPersonagemService _personagemService;

        public PersonagemController(IPersonagemService personagemService)
        {
            _personagemService = personagemService;
        }

        [HttpGet("listaPersonagem")]
        public async ValueTask<IActionResult> BuscarPessoa(string nome)
        {
            try
            {
                if(nome == null)
                {
                    return BadRequest();
                }
                else
                {
                    var personagem = await _personagemService.BuscarPersonagem(nome);
                    return personagem.Any() ? Ok(personagem) : BadRequest();
                }                
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao efetuar a pesquisa. {ex.Message}");
            }
        }
    }
}

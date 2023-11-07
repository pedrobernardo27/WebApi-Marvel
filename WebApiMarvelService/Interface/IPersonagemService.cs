using WebApiMarvelService.Result;

namespace WebApiMarvelService.Interface
{
    public interface IPersonagemService
    {
        ValueTask<IEnumerable<PersonagemResult>> BuscarPersonagem(string nome);
    }
}

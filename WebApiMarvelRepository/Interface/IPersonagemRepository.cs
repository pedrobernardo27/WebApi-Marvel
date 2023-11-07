namespace WebApiMarvelRepository.Interface
{
    public interface IPersonagemRepository
    {
        ValueTask<string> BuscarPersonagem(string chavePublica, string datastring, string codigo, string nome);
        ValueTask<string> BuscarEvento(string chavePublica, string dataString, string codigo, int id);
    }
}

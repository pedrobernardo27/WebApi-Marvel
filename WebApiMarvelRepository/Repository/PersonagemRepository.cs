using WebApiMarvelRepository.Interface;
using Microsoft.Extensions.Configuration;
using Web.Api.Marvel.Domain.Entities.EventoDomain;

namespace WebApiMarvelRepository.Repository
{
    public class PersonagemRepository : IPersonagemRepository
    {
        private IConfiguration _config;
        public PersonagemRepository(IConfiguration config)
        {
            _config = config;
        }
        public async ValueTask<string> BuscarPersonagem(string chavePublica, string dataString, string codigo, string nome)
        {
            try
            {  
                using (var client = new HttpClient())
                {
                    var urlBase = _config["BaseURLPersonagem"];
                    var lstEventos = new List<Evento>();

                    string url = $"{urlBase}characters?ts={dataString}&apikey={chavePublica}&hash={codigo}&nameStartsWith={nome}";

                    HttpResponseMessage response = client.GetAsync(url).Result;

                    response.EnsureSuccessStatusCode();
                    string conteudo = response.Content.ReadAsStringAsync().Result;
                    return conteudo;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}", ex);
            }
        }

        public async ValueTask<string> BuscarEvento(string chavePublica, string dataString, string codigo, int id)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var urlBase = _config["BaaseURLEvento"];                    
                    string url = $"{urlBase}{id}/events?orderBy=startDate&ts={dataString}&apikey={chavePublica}&hash={codigo}";

                    HttpResponseMessage response = client.GetAsync(url).Result;

                    response.EnsureSuccessStatusCode();
                    string conteudo = response.Content.ReadAsStringAsync().Result;
                    return conteudo;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}", ex);
            }
        }
    }
}
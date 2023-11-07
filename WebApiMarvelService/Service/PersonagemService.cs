using System.Text;
using Newtonsoft.Json;
using WebApiMarvelService.Result;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using WebApiMarvelService.Interface;
using WebApiMarvelRepository.Interface;
using Microsoft.Extensions.Configuration;
using Web.Api.Marvel.Domain.Entities.EventoDomain;
using Web.Api.Marvel.Domain.Entities.PersonagemDomain;

namespace WebApiMarvelService.Service
{
    public class PersonagemService : IPersonagemService
    {
        #region Properties

        private IConfiguration _config;
        private readonly ILogger<PersonagemService> _logger;
        private readonly IPersonagemRepository _personagemRepository;

        #endregion

        #region Constructor

        public PersonagemService(IConfiguration config, ILogger<PersonagemService> logger, IPersonagemRepository personagemRepository)
        {
            _config = config;
            _logger = logger;
            _personagemRepository = personagemRepository;
        }

        #endregion

        #region Metodo principal

        public async ValueTask<IEnumerable<PersonagemResult>> BuscarPersonagem(string nome)
        {
            try
            {
                _logger.LogInformation("Inicio do método BuscarPersonagem");

                var lstPersonagens = new List<PersonagemResult>();
                var dataString = DateTime.Now.Ticks.ToString();
                var chavePublica = _config["ChavePublica"];
                var chavePrivada = _config["ChavePrivada"];
                var codigo = GerarCodigo(dataString, chavePublica, chavePrivada);

                var resultPersonagem = await _personagemRepository.BuscarPersonagem(chavePublica, dataString, codigo, nome);
                var personagensEncontrado = JsonConvert.DeserializeObject<Personagem>(resultPersonagem);

                foreach (var personagem in personagensEncontrado.data.results)
                {
                    var resultEvento = await _personagemRepository.BuscarEvento(chavePublica, dataString, codigo, personagem.id);
                    var eventoEncontrado = JsonConvert.DeserializeObject<Evento>(resultEvento);

                    PreencherDadosPersonagens(lstPersonagens, personagem, eventoEncontrado);
                }

                _logger.LogInformation("Fim do método BuscarPersonagem");

                return lstPersonagens;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao efetuar BuscarPersonagem {ex.Message}");
                throw new Exception($"Erro ao efetuar BuscarPersonagem {ex.Message}");
            }
        }

        #endregion

        #region Metodo GerarCodigo

        private static string GerarCodigo(string ts, string publicKey, string privateKey)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(ts + privateKey + publicKey);
            var gerador = MD5.Create();
            byte[] bytesHash = gerador.ComputeHash(bytes);
            return BitConverter.ToString(bytesHash).ToLower().Replace("-", String.Empty);
        }

        #endregion

        #region Metodo PreencherDadosPersonagens

        private static void PreencherDadosPersonagens(List<PersonagemResult> lstPersonagens, Web.Api.Marvel.Domain.Entities.PersonagemDomain.Result personagem, Evento? eventoEncontrado)
        {
            var novoPersonagem = new PersonagemResult
            {
                Id = personagem.id,
                Nome = personagem.name,
                Descricao = personagem.description,
                ImagemPersonagem = personagem.thumbnail.path + "." + personagem.thumbnail.extension
            };

            if (eventoEncontrado != null)
            {
                var strBuilder = new StringBuilder();

                foreach (var evento in eventoEncontrado.data.results)
                {
                    strBuilder.AppendLine($"{evento.id}/n" +
                        $"{evento.title}/n" + $"{evento.description}/n" +
                        $"{evento.thumbnail.path}.{evento.thumbnail.extension}&&");
                }

                novoPersonagem.EventoResult = strBuilder.ToString();
            }

            lstPersonagens.Add(novoPersonagem);
        }

        #endregion
    }
}
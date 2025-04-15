using APIEndereco.DataBase;
using Microsoft.AspNetCore.Mvc;

namespace APIEndereco.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EnderecoController : ControllerBase
    {
        private readonly ILogger<EnderecoController> _logger;
        public EnderecoController(ILogger<EnderecoController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetEndereco")]
        public IEnumerable<Endereco> Get()
        {

            var database = new DatabaseEndereco();
            var listaEnderecos = database.GetData();

            return listaEnderecos;
        }

        [HttpPost(Name = "PostEndereco")]
        public void Post(Endereco endereco)
        {
            var database = new DatabaseEndereco();

            database.PostData(endereco);
        }
    }
}

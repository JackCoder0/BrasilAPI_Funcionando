using Microsoft.AspNetCore.Mvc;
using Teste_BRASILAPI.Interfaces;
using Teste_BRASILAPI.Services;

namespace Teste_BRASILAPI.Controllers
{
    [ApiController]
    [Route("{controller}")]
    public class EnderecoController : Controller
    {
        private readonly EnderecoService _enderecoService;

        public EnderecoController(EnderecoService enderecoService)
        {
            _enderecoService = enderecoService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet("{action}")] 
        public async Task<IActionResult> EnderecoInfo(string cep)
        {
            var response = await _enderecoService.BuscarEndereco(cep);

           return View(response);
        }

  
    }
}

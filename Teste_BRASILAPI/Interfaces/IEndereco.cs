using Microsoft.AspNetCore.Mvc;
using Teste_BRASILAPI.Models;

namespace Teste_BRASILAPI.Interfaces
{
    public interface IEndereco
    {

         Task<EnderecoModel> BuscarEndereco(string cep);
        
    }
}

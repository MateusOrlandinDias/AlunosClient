using AlunosApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

/*
 * Em arquitetura de software, um controller é uma camada de abstração que atua como intermediário entre a 
 * camada de apresentação (normalmente uma interface gráfica de usuário) e a camada de persistência de dados. 
 * Em uma API (Application Programming Interface), o controller é responsável por gerenciar as solicitações HTTP 
 * que chegam à API e direcioná-las para os componentes corretos que irão processá-las.

Em uma API RESTful, os controllers são responsáveis por lidar com as requisições HTTP, como GET, POST, PUT e 
DELETE, e retornar as respostas adequadas ao cliente. Por exemplo, em uma aplicação de e-commerce, o controller 
pode ser responsável por recuperar uma lista de produtos, adicionar um produto ao carrinho de compras ou enviar 
uma solicitação de pagamento para o provedor de pagamento.

Em geral, um controller em uma API tem as seguintes responsabilidades:

-Validar as entradas dos usuários
-Chamar os serviços ou a camada de modelo da aplicação, que realiza as operações necessárias
-Preparar as respostas para os clientes, incluindo o código HTTP adequado (200 OK, 404 Not Found, etc.) e qualquer 
informação adicional, como os dados retornados ou mensagens de erro.

O uso de controllers é importante para manter a arquitetura da API clara e bem estruturada, facilitando a 
manutenção e o desenvolvimento de novas funcionalidades.
 */

namespace AlunosApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlunosController : ControllerBase
    {
        private IAlunoService _alunoService;

        public AlunosController(IAlunoService alunoService)
        {
            _alunoService = alunoService;
        }
    }
}

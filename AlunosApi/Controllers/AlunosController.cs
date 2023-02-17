using AlunosApi.Models;
using AlunosApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        /*
         * O objetivo com o controlador finalizado, é definir os end points dos serviços que a nossa API vai 
         * expor.
         * O ControllerBase referenciado acima deste comentario, nos auxilia com metodos de badRequest, 
         * entre outros. O recurso [ApiController] junto com o ControlerBase, permitem ao controlador assumir
         * a responsabilidade de fazer o REST. [Route("api/[controller]")] permite que no arquivo Startup.cs
         * possamos definir nossos endponts e routing.
         * 
         * Injetamos uma instancia do nosso serviço no controlador. Como nosso serviço usa o context,
         * com a instancia do nosso contrato, podemos fazer alterações no banco de dados.
         */
        private IAlunoService _alunoService;

        public AlunosController(IAlunoService alunoService)
        {
            _alunoService = alunoService;
        }

        [HttpGet]
        public async Task<ActionResult<IAsyncEnumerable<Aluno>>> GetAlunos()
        {
            /*
         * [HttpGet] annotation para referenciar o verbo GET
         * 
         * -O Task, assim como o async quer dizer que é uma operação assincrona
         * -O ActionResult permite que eu retorne algum tipo especifico ou um tipo de response de API
         * -IAsyncEnumerable pois se fosse apenas o IEnumerable poderia trazer problemas especificos de um metodo
         * sincrono, por mais que tivesse Task e async.
         * 
         * Para efeitos de aula, vamos criar try catches dentro dos metodos. O certo seria criar uma tratativa de 
         * erros global.
         * 
         * Com o Action Result podemos retornar os status codes daquela forma que estamos fazendo 
         * OU 
         * Podemos usar o recurso do namespace Microsoft.AspNetCore.Mvc -> nos annotations como 
         * [ProducesResponseType(StatusCodes.Status200OK)]
         * 
         * Se o cliente quiser o retorno não em JSON como estamos fazendo, mas quiser em XML, podemos
         * deixar explicito com [Produces(application/xml)] nas annotations da classe do controller
         */
            try
            {
                var alunos = await _alunoService.GetAlunos();
                return Ok(alunos);
            }
            catch
            {
                //return BadRequest("Request inválido");
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao obter alunos");
            }
        }

        [HttpGet("AlunosPorNome")]
        public async Task<ActionResult<IAsyncEnumerable<Aluno>>> GetAlunosByNome([FromQuery] string nome)
        {
            /*
             * Para este segundo método de GET, tivemos um erro no swagger pois não podem ter 2 HttpGet
             * passando na mesma rota... É preciso definir outra rota.
             * Esta rota: [HttpGet("AlunosPorNome")]
            A outra rota [HttpGet] pega a rota de Alunos
             * 
             * O atributo [FromQuery] é usado para especificar a origem dos dados de uma requisição HTTP e 
             * é uma parte do sistema de model binding do ASP.NET Core.
             */
            try
            {
                var alunos = await _alunoService.GetAlunosByNome(nome);
                    if(alunos==null)
                        return NotFound($"Não existem alunos com o critério {nome}");

                return Ok(alunos);
            }
            catch
            {
                return BadRequest("Request inválido");
            }
        }

        //Defini o Name= GetAluno no end point abaixo para usar no post 
        [HttpGet("{id:int}", Name="GetAluno")]
        public async Task<ActionResult<Aluno>> GetAluno(int id)
        {
            try
            {
                var aluno = await _alunoService.GetAluno(id);
                if (aluno==null)
                    return NotFound($"Não existe aluno com o id {id}");

                return Ok(aluno);
            }
            catch
            {
                return BadRequest("Request inválido");
            }
        }

        [HttpPost]
        public async Task<ActionResult> Create( Aluno aluno)
        {
            /*
         * Agora preciso definir um end point para criar um recurso, ou seja, o verbo http POST
         * -Deve-se retornar o código de status 201 para indicar que o recurso foi criado com sucesso
         * -Deve-se retornar o recurso que eu criei
         * -Deve-se adicionar um cabeçalho location no response que vai especificar a URI do objeto Aluno
         * recem criado
         * 
         * CreatedAtRoute-> Status 201
         * nameof(GetAluno)-> Usar o nameof buscando pelo nome do metodo é uma boa pratica pois se buscasse
         * "GetAluno" que é o que foi colocado no annotations do GetAluno() e eu mudasse, nao seria dinamico.
         * Ele usa o GetAluno para trazer os dados do aluno recem criado.
         * new { id = aluno.Id }-> passa o novo id criado para o GetAluno buscar
         * aluno-> retorna o aluno novo no CreatedAtRout junto com seu codigo certo 201.
         */
            try
            {
                await _alunoService.CreateAluno(aluno);
                return CreatedAtRoute(nameof(GetAluno), new { id = aluno.Id }, aluno);
            }
            catch
            {
                //return BadRequest("Request inválido");
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao obter alunos");
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Edit(int id, [FromBody] Aluno aluno)
        {
            /*
             * Aqui foi pedido de entrada o id do aluno e no corpo do json deve entrar os dados do Aluno
             * 
             * No PUT posso retornar:
             * 204 - Non Content
             * 200 - Sucesso
             * 
             * No verbo PUT eu devo passar todos os dados, mesmo os que eu não for atualizar
             * Já no verbo PATCH ou PAT algo assim, é possivel atualizar somente uma info
             * 
             * No código é feita uma conferencia para os dados de entrada falarem sobre o mesmo aluno
             */
            try
            {
                if(aluno.Id == id)
                {
                    await _alunoService.UpdateAluno(aluno);
                    return Ok($"Aluno com id={id} foi atualizado com sucesso.");
                }
                else
                {
                    return BadRequest("Dados inconsistentes");
                }
            }
            catch
            {
                return BadRequest("Request inválido");
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var aluno = await _alunoService.GetAluno(id);
                if(aluno != null)
                {
                    await _alunoService.DeleteAluno(aluno);
                    return Ok($"Aluno de id = {id} foi excluido com sucesso.");
                }
                else
                {
                    return BadRequest($"Aluno com id = {id} não encontrado");
                }
            }
            catch
            {
                return BadRequest("Request inválido");
            }
        }
    }
}

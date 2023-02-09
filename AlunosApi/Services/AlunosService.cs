using AlunosApi.Context;
using AlunosApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AlunosApi.Services
{
    /*
     * Ao herdar IAlunoService(contrato), o Visual Studio já sugere implementação com esses throws
     * 
     * Usar Async e await em todos os metodos para que sejam metodos asíncronos e para que este serviço não
     * seja impeditivo para outros serviços rodarem.
     * 
     * Aqui não vamos entrar tanto em tratativas, vamos fazer metodos mas nao pensar muito em todas as 
     * possibilidades.
     * 
     * No inicio da classe do serviço, é preciso injetar o contexto
     * Para isso utilizei o 'private readonly AppDbContext _context;'
     * Pela sugestao do Visual Studio ele faz o construtor da injeção. Isso é possivel pois
     * criamos o serviço de context no Startup.cs.
     */
    public class AlunosService : IAlunoService
    {
        private readonly AppDbContext _context;

        public AlunosService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Aluno>> GetAlunos()
        {
            throw new System.NotImplementedException();
        }
        public async Task<Aluno> GetAluno(int id)
        {
            throw new System.NotImplementedException();
        }
        public async Task<IEnumerable<Aluno>> GetAlunosByNome(string nome)
        {
            throw new System.NotImplementedException();
        }
        public async Task CreateAluno(Aluno aluno)
        {
            throw new System.NotImplementedException();
        }
        public async Task UpdateAluno(Aluno aluno)
        {
            throw new System.NotImplementedException();
        }
        public async Task DeleteAluno(Aluno aluno)
        {
            throw new System.NotImplementedException();
        }
    }
}

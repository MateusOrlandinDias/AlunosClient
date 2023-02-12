using AlunosApi.Context;
using AlunosApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
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
        //"Injeção"
        private readonly AppDbContext _context;

        public AlunosService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Aluno>> GetAlunos()
        {
            /*
             * É bom adicionar sempre um try catch com tratativas para cada tipo de erro.
             * Aqui eu adicionei só um try com um catch apenas dando um throw (passando a exceção p frente),
             * o que não é recomendado mas é apenas para efeito de aula.
             * 
             * No caso deste método, ele vai retornar todos os alunos, mas em grande escala isso seria pouco 
             * performático. Uma sugestão seria utilizar algum tipo de filtro ao retornar, mas aqui queremos
             * todos mesmo.
             * 
             * Algo que melhoraria o desempenho deste método, seria utilizar o comando 
             * return await _context.Alunos.AsNoTracking().ToListAsync();
             * pois não carregaria as entidades com o AsNoTracking(), mas isso limitaria o método a não poder 
             * fazer alterações nos alunos retornados, e nesta aula não queremos limitar este método.
             * 
             * Utilizando o ToListAsync para retornar uma lista que pode ser iterada
             */
            try
            {
                return await _context.Alunos.ToListAsync();
            }
            catch
            {
                throw;
            }
        }
        public async Task<IEnumerable<Aluno>> GetAlunosByNome(string nome)
        {
            /*
             * Neste método, primeiro é definido um IEnumerable do tipo Aluno com o nome "alunos", que é
             * o que desejamos retornar.
             * 
             * Um IEnumerable é uma coleção do tipo Aluno que criamos. O que for retornado, poderá ser iterado 
             * por ser um IEnumerable.
             * 
             * Após isso, é feita uma comparação para saber se o que foi requisitado é nulo ou não
             * Caso NULO -> Chama o método GetAlunos() para retornar todos os alunos
             * Caso NÃO NULO -> Retorna os alunos que contenham a string de entrada, para isso foi utilizado o 
             * método where(n=> n.Nome.Contains(nome)) dentro dele é passado o que se chama "lambda"
             * 
             * Utilizando o ToListAsync para retornar uma lista que pode ser iterada
             * 
             * É importante se atentar ao uso do contexto para usar o n.Nome (uma coluna ou propriedade do 
             * tipo Aluno, trazida para o AlunoServices atravez do context)
             */
            try
            {
                IEnumerable<Aluno> alunos;
                if(!string.IsNullOrWhiteSpace(nome))
                {
                    alunos = await _context.Alunos.Where(n => n.Nome.Contains(nome)).ToListAsync();
                }
                else
                {
                    alunos = await GetAlunos();
                }
                return alunos;
            }
            catch
            {
                throw;
            }
        }
        public async Task<Aluno> GetAluno(int id)
        {
            /*
             * Aqui nesse método, repare que foi utilizado o FindAsync pois com ele ou com o Find, a gente
             * ganha desempenho, pois ele procura primeiro na memória, se não achar procura dentre todos os
             * alunos. Diferentemente do FisrstOrDefault que não tenta buscar na memória primeiro.
             */
            try
            {
                var aluno = await _context.Alunos.FindAsync(id);
                return aluno;
            }
            catch
            {
                throw;
            }
        }
        public async Task CreateAluno(Aluno aluno)
        {
            //Aqui primeiro se adiciona o aluno e depois salva a alteração no contexto;
            try
            {
                _context.Alunos.Add(aluno);
                await _context.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }
        public async Task UpdateAluno(Aluno aluno)
        {
            /*
             * Aqui eu passei o estado deste aluno para modificado e apliquei as alterações que foram dadas 
             * de input do metodo.
             */
            try
            {
                _context.Entry(aluno).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }
        public async Task DeleteAluno(Aluno aluno)
        {
            try
            {
                _context.Alunos.Remove(aluno);
                await _context.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }
    }
}

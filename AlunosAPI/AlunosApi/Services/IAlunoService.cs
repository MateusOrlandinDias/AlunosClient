using AlunosApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AlunosApi.Services
{
    public interface IAlunoService
    {
        /*
         * Uma interface é um contrato que diz todos os métodos que uma classe deve seguir...
         * O mais correto é sempre utilizar interfaces para isso.
         * 
         * Todos os metodos dessa interface tem que ser atendidos em uma classe pelo menos (mas não pode
         * cumprir metade em uma e metade na outra). Pode acontecer de duas classes cumprirem totalmente a mesma
         * interface, isso é util pois podemos ter os mesmos metodos em classes diferentes com diferentes 
         * finalidades.
         * 
         * O que tiver <> é o tipo que será retornado -> Get
         * O que não tiver este retorno, será para Create, Update e Delete
         * 
         * Agora uma classe concreta precisa atender aos requisitos deste contrato ou pode ocasionar erros
         * de compilação.
         */
        Task<IEnumerable<Aluno>> GetAlunos();
        Task<Aluno> GetAluno(int id);
        Task<IEnumerable<Aluno>> GetAlunosByNome(string nome);
        Task CreateAluno(Aluno aluno);
        Task UpdateAluno(Aluno aluno);
        Task DeleteAluno(Aluno aluno);
    }
}

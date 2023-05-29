using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

/*
 * Aqui esta sendo criado um tipo Aluno na qual com as anotations está informando ao entity core framework
 * para referenciar seus dados em uma tabela do banco de dados.
 * Na classe está sendo definida as colunas da tabela com suas especificações sendo incluidas com o annotations
 * 
 * 
 */
namespace AlunosApi.Models
{
    [Table("Alunos")]
    public class Aluno
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(80)]
        public string Nome { get; set; }
        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }
        [Required]
        public int Idade { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

/*
 * Em C#, um view model é uma classe que representa dados e comportamentos específicos de uma visualização 
 * (ou "view") em uma aplicação web ou desktop. Geralmente, um view model é usado para fornecer dados e operações 
 * necessárias para exibir informações na tela para o usuário.

Um view model geralmente contém propriedades que correspondem a campos na visualização, bem como métodos que 
executam ações em resposta às interações do usuário. O objetivo do view model é isolar a lógica de negócios da 
aplicação da lógica de apresentação, permitindo que a mesma lógica de negócios seja reutilizada em diferentes 
visualizações.

Ao usar um view model, os desenvolvedores podem separar a apresentação dos dados da lógica de negócios subjacente, 
o que pode tornar o código mais organizado, mais fácil de entender e mais fácil de manter ao longo do tempo.

Imagine que você é um professor que quer ensinar uma criança a desenhar um cachorro. Você pode explicar para a 
criança os diferentes elementos que compõem um cachorro, como cabeça, corpo, patas, etc. No entanto, isso pode ser 
um pouco difícil para a criança entender e reproduzir na hora de desenhar.

Então, para facilitar a tarefa da criança, você pode criar um modelo de visualização (ou "view model") do cachorro. Esse modelo é um desenho simples que mostra as partes principais do cachorro, como um círculo para a cabeça, um retângulo para o corpo e quatro linhas para as patas.

Ao usar esse modelo, a criança pode seguir as linhas e desenhar um cachorro muito mais facilmente, sem precisar se 
preocupar em lembrar todas as partes do cachorro ou como elas se encaixam.

De maneira similar, um view model em C# é um modelo simplificado de dados que representa uma visualização na sua 
aplicação. Ele contém apenas as informações necessárias para exibir a visualização e realizar operações 
relacionadas a ela. Assim como o modelo de visualização do cachorro ajuda a criança a desenhar, um view model 
ajuda a separar a lógica de apresentação da lógica de negócios da aplicação, tornando o código mais organizado e 
mais fácil de entender e manter.
 */
namespace AlunosApi.ViewModels
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Formato de email inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória")]
        [StringLength(20, ErrorMessage = "A {0} deve ter no mínimo {2} e no máximo {1} caracteres.", MinimumLength = 10)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}

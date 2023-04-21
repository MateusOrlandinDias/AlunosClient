using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace AlunosApi.Services
{
    public class AuthenticateService : IAuthenticate
    {
        /*No código fornecido, o caractere de sublinhado _ é usado antes do nome da variável _signInManager. Isso é uma convenção comum em C# para indicar que a variável é uma variável de instância de uma classe. É comum usar o caractere de sublinhado em nomes de variáveis de instância em C# para ajudar a distingui-las de outras variáveis, como variáveis locais ou parâmetros de método.

Além disso, a classe AuthenticateService implementa a interface IAuthenticate e recebe uma instância de SignInManager<IdentityUser> em seu construtor. SignInManager é uma classe fornecida pelo pacote Microsoft.AspNetCore.Identity que é usada para gerenciar a autenticação de usuários em aplicativos web. Ele fornece métodos que podem ser usados para realizar login, logout e outras operações relacionadas à autenticação.

Aqui, a instância _signInManager é injetada no construtor do serviço AuthenticateService por meio da injeção de dependência. Isso permite que o AuthenticateService use o SignInManager para realizar a autenticação de usuários. O tipo genérico IdentityUser especifica o tipo de usuário que será autenticado. Neste caso, a classe IdentityUser é fornecida pelo pacote Microsoft.AspNetCore.Identity.

Em resumo, _signInManager é uma variável de instância da classe AuthenticateService que contém uma instância de SignInManager<IdentityUser>, que é usada para gerenciar a autenticação de usuários em um aplicativo web.
         */
        private readonly SignInManager<IdentityUser> _signInManager;
        /*O mesmo da linha anterior é feito para o register com o user manager, isso é INJETAR 
         * junto com colocar no construtor*/
        private readonly UserManager<IdentityUser> _userManager;

        public AuthenticateService(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<bool> Authenticate(string email, string password)
        {
            /*passo o user,
             * senha,
             * false - Não armazenar o auth em cookies caso o user feche a pagina
             * lockoutOnFailure: false - Não bloquear conta se o user errar a senha
             */
            var result = await _signInManager.PasswordSignInAsync(email, password, false, lockoutOnFailure: false);

            return result.Succeeded;
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<bool> RegisterUser(string email, string password)
        {
            var appUser = new IdentityUser
            {
                UserName = email,
                Email = email
            };

            var result = await _userManager.CreateAsync(appUser, password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(appUser, isPersistent: false);
            }

            return result.Succeeded;
        }
    }
}

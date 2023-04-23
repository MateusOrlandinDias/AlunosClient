using AlunosApi.Services;
using AlunosApi.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AlunosApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IAuthenticate _authentication;

        public AccountController(IConfiguration configuration, IAuthenticate authentication)
        {
            //O ?? aqui ta checkando se os valores nao estao nulos, se tiver da throw (gerado automaticamente)
            _configuration = configuration ?? 
                throw new ArgumentNullException(nameof(configuration));
            _authentication = authentication ?? 
                throw new ArgumentNullException(nameof(authentication));
        }

        /* O ModelState representa um par de nome e valor, serve para armazenar o valor que foi submetido ao 
         * servidor e para armazenar os erros de validação asossiados com estes valores.
         * É como se fosse um dicionario que eu vou colocando as chaves que eu quero e armazenando certos 
         * valores em string dentro dele para possivelmente usar isso depois em alguma outra ocasião
         */
        //O retorno de UserToken não esta sendo usado e poderia ser trocado por RegisterModel
        //Mas no video ele deixou assim para dizer que poderiamos retornar o token se quisessemos
        [HttpPost("CreateUser")]
        public async Task<ActionResult<UserToken>> CreateUser([FromBody] RegisterModel model)
        {
            if(model.Password != model.ConfirmPassword)
            {
                ModelState.AddModelError("ConfirmPassword", "As senhas não conferem.");
                return BadRequest(ModelState);
            }

            var result = await _authentication.RegisterUser(model.Email, model.Password);

            if (result)
            {
                return Ok($"Usuário {model.Email} criado com sucesso.");
            }
            else
            {
                ModelState.AddModelError("CreateUser", "Registro inválido.");
                return BadRequest(ModelState);
            }
        }

        [HttpPost("LoginUser")]
        public async Task<ActionResult<UserToken>> Login([FromBody] LoginModel userInfo)
        {
            var result = await _authentication.Authenticate(userInfo.Email, userInfo.Password);

            if (result)
            {
                return GenerateToken(userInfo);
            }
            else
            {
                ModelState.AddModelError("LoginUser", "Login inválido.");
                return BadRequest(ModelState);
            }
        }

        /*Ali em cima quando eu usei o GenerateToken(userInfo), ele pede pra gerar este metodo de generate
         * token, aqui eu vou definir algumas claims (posso criar se quiser e usar as que ele usou).
         */
        private ActionResult<UserToken> GenerateToken(LoginModel userInfo)
        {
            var claims = new[]
            {
                new Claim("email", userInfo.Email),
                new Claim("meuToken", "token do Orlandin"),
                new Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
            //Com a chave acima, podemos gerar a seguinte assinatura digital
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            //Defino tambem um tempo de expiração do meu token
            var expiration = DateTime.UtcNow.AddMinutes(20);

            //Criando meu token JWT
            JwtSecurityToken token = new JwtSecurityToken(
                issuer: _configuration["JwtSecurityToken:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: expiration,
                signingCredentials: creds
                );

            //retorna o token em string
            return new UserToken()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration,
            };
        }
    }
}

using System.ComponentModel.DataAnnotations;

namespace AlunosApi.ViewModels
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Formato de email inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória")]
        [StringLength(20, ErrorMessage = "A {0} deve ter no mínimo {2} e no máximo {1} " +
            "caracteres.", MinimumLength = 10)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name ="Confirma senha")]
        [Compare("Password", ErrorMessage = "Senhas não conferem.")]
        public string ConfirmPassword { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace AlunosApi.ViewModels
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Formato de email inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória")]
        [StringLength(20, MinimumLength = 10, ErrorMessage = "A senha deve ter entre {MinimumLength} e {MaximumLength} caracteres.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name ="Confirma senha")]
        [Compare("Password", ErrorMessage = "Senhas não conferem.")]
        public string ConfirmPassword { get; set; }
    }
}

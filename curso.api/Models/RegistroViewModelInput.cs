using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace curso.api.Models
{
    public class RegistroViewModelInput
    {
        [Required(ErrorMessage = "O login é obrigatório")]
        public string Login { get; set; }
        [Required(ErrorMessage = "O Email é obrigatório")]
        public string Email { get; set; }
        [Required(ErrorMessage = "A Senha é obrigatório")]
        public string Senha { get; set; }
    }
}

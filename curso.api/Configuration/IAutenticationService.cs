using curso.api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace curso.api.Configuration
{
    public interface IAutenticationService
    {
        string GerarToken(UsuarioViewModelOutput _usuarioViewModelOutput);
    }
}

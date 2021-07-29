using curso.api.Business.Entities;
using curso.api.Business.Entities.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace curso.api.Infra.Data.Repository
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly CursoDBContext _contexto;

        public UsuarioRepository(CursoDBContext contexto)
        {
            _contexto = contexto;
        }

        public void Adicionar(Usuario usuario)
        {
            _contexto.Add(usuario);         
        }

        public void Commit()
        {
            _contexto.SaveChanges();
        }

        public Usuario ObterUsuario(string login)
        {
            return _contexto.Usuario.FirstOrDefault(u => u.Login == login);
        }
    }
}

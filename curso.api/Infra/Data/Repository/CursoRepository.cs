using curso.api.Business.Entities;
using curso.api.Business.Entities.Repositorios;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace curso.api.Infra.Data.Repository
{
    public class CursoRepository : ICursoRepository
    {
        private readonly CursoDBContext _contexto;
        public CursoRepository(CursoDBContext contexto)
        {
            _contexto = contexto;
        }
        public void Adicionar(Curso curso)
        {
            _contexto.Add(curso);
        }

        public void Commit()
        {
            _contexto.SaveChanges();
        }

        public IList<Curso> ObterPorUsuario(int codigousuario)
        {
            return _contexto.Curso.Include(i => i.Usuario).Where(c => c.CodigoUsuario == codigousuario).ToList();
        }
    }
}

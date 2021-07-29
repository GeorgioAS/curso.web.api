using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace curso.api.Business.Entities.Repositorios
{
    public interface ICursoRepository
    {
        void Adicionar(Curso curso);
        void Commit();
        IList<Curso> ObterPorUsuario(int codigousuario);
    }
}

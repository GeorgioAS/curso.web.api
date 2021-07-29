using curso.api.Business.Entities;
using curso.api.Business.Entities.Repositorios;
using curso.api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace curso.api.Controllers
{
    [Route("api/v1/cursos")]
    [ApiController]
    [Authorize]
    public class CursoController : ControllerBase
    {
        private readonly ICursoRepository _cursoRepository;

        public CursoController(ICursoRepository cursoRepository)
        {
            _cursoRepository = cursoRepository;
        }

        [SwaggerResponse(statusCode: 201, description: "Sucesso ao cadastrar o curso")]
        [SwaggerResponse(statusCode: 401, description: "Não autorizado")]
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Post(CursoViewModelInput cursoviewmodelinput)
        {
            Curso curso = new Curso();
            curso.Nome = cursoviewmodelinput.Nome;
            curso.Descricao = cursoviewmodelinput.Descricao;

            curso.CodigoUsuario = int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
            _cursoRepository.Adicionar(curso);
            _cursoRepository.Commit();

            return Created("", cursoviewmodelinput);
        }

        [SwaggerResponse(statusCode: 200, description: "Sucesso ao obter o curso")]
        [SwaggerResponse(statusCode: 401, description: "Não autorizado")]
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Get()
        {
            var codigousuario = int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
            var cursos = _cursoRepository.ObterPorUsuario(codigousuario)
                .Select(s => new CursoViewModelOutput()
                {
                    Nome = s.Nome,
                    Descricao = s.Descricao,
                    Login = s.Usuario.Login
                });
            ;
            return Ok(cursos);
        }
    }
}

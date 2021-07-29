using curso.api.Business.Entities;
using curso.api.Business.Entities.Repositorios;
using curso.api.Configuration;
using curso.api.Filters;
using curso.api.Infra.Data;
using curso.api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace curso.api.Controllers
{
    [Route("api/v1/usuario")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {

        private readonly IUsuarioRepository _usuarioRepositorio;
        //private readonly IConfiguration _configuration;
        private readonly IAutenticationService _autenticationService;
        public UsuarioController(IUsuarioRepository usuarioRepositorio, 
            
            IAutenticationService autenticationService)
        {
            _usuarioRepositorio = usuarioRepositorio;
            
            _autenticationService = autenticationService;
        }

        /// <summary>
        ///  Metodo para fazer o login do Usuario
        /// </summary>   
        /// <param name="LoginViewModelInput">ViewModel Login e Senha.</param>
        /// <returns></returns>
        ///
        [SwaggerResponse(statusCode:200,description :"Sucesso ao autenticar",Type = typeof(LoginViewModelInput))]
        [SwaggerResponse(statusCode: 400, description: "Campos obirgatórios", Type = typeof(ValidaCampoViewModelOutput))]
        [SwaggerResponse(statusCode: 500, description: "Erro Interno", Type = typeof(ErroGenericoViewModel))]
        [HttpPost]
        [Route("logar")]
        [ValidacaoModelStateCurtomizado]
        public IActionResult Logar(LoginViewModelInput loginViewModelInput) 
        {
           
            Usuario usuario = _usuarioRepositorio.ObterUsuario(loginViewModelInput.Login);

            if (usuario == null)
            {
                return BadRequest("Houve um erro ao tentar acessar o usuário.");
            }

            var usuarioViewModelOutput = new UsuarioViewModelOutput()
            {
                Codigo = usuario.Codigo,
                Email = usuario.Email,
                Login = loginViewModelInput.Login
            };

            //var secret = Encoding.ASCII.GetBytes(_configuration.GetSection("JwtConfigurations:Secret").Value);
            //var symmetricSecurityKey = new SymmetricSecurityKey(secret);
            //var securityTokenDescritor = new SecurityTokenDescriptor
            //{
            //    Subject = new System.Security.Claims.ClaimsIdentity(new Claim[]
            //    {
            //        new Claim(ClaimTypes.NameIdentifier, usuario.Codigo.ToString()),
            //        new Claim(ClaimTypes.Name, usuario.Login.ToString()),
            //        new Claim(ClaimTypes.Email, usuario.Email.ToString())
            //    }), Expires = DateTime.UtcNow.AddDays(1),
            //    SigningCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature)
            //};
            //var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            //var tokenGenerated = jwtSecurityTokenHandler.CreateToken(securityTokenDescritor);
            //var token = jwtSecurityTokenHandler.WriteToken(tokenGenerated);

            var token = _autenticationService.GerarToken(usuarioViewModelOutput);

            return Ok(new { Token = token, Usuario = usuario});        
        }

        /// <summary>
        ///  Metodo para Registrar o usuario
        /// </summary>   
        /// <param name="RegistroViewModelInput">Registro de Login.</param>
        /// <returns></returns>
        ///
        [SwaggerResponse(statusCode: 200, description: "Sucesso ao registrar", Type = typeof(RegistroViewModelInput))]
        [SwaggerResponse(statusCode: 400, description: "Campos obirgatórios", Type = typeof(ValidaCampoViewModelOutput))]
        [SwaggerResponse(statusCode: 500, description: "Erro Interno", Type = typeof(ErroGenericoViewModel))]
        [HttpPost]
        [Route("registrar")]
        [ValidacaoModelStateCurtomizado]
        public IActionResult Registrar(RegistroViewModelInput login)
        {

            //var optionsBuilder = new DbContextOptionsBuilder<CursoDBContext>();
            //optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            //CursoDBContext contexto = new CursoDBContext(optionsBuilder.Options);

            //var migracoes = contexto.Database.GetPendingMigrations();
            //if (migracoes.Count() > 0)
            //{
            //    contexto.Database.Migrate();
            //}

            var usuario = new Usuario();
            usuario.Login = login.Login;
            usuario.Senha = login.Senha;
            usuario.Email = login.Email;

            _usuarioRepositorio.Adicionar(usuario);
            _usuarioRepositorio.Commit();
            return Ok(login);
        }

    }
}

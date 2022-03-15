using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Protecta.Application.Service.Dtos.Perfil;
using Protecta.Application.Service.Dtos.Seguridad;
using Protecta.Application.Service.Helpers;
using Protecta.Application.Service.Services.PerfilModule;
using Protecta.Application.Service.Services.UserModule;
using Protecta.Domain.Service.UserModule.Aggregates.UserAgg;
using Protecta.CrossCuting.Utilities.Configuration;


namespace Protecta.Application.Service.Controllers.Seguridad
{
    [Authorize]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private IUserService _userService;
        private IPerfilService _perfilService;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;
        private readonly Ldap _ldapSettings;

        public UsersController(
            IUserService userService,
            IPerfilService perfilService,
            IMapper mapper,
            IOptions<AppSettings> appSettings,
            IOptions<Ldap> ldapSettings)
        {
            _userService = userService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
            _ldapSettings = ldapSettings.Value;
            _perfilService = perfilService;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]UserDto userDto)
        {
            var user = _userService.Authenticate(userDto.Username, userDto.Password);

            if (user == null)
                return Unauthorized();

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // return basic user info (without password) and token to store client side
            return Ok(new
            {
                user.Result.Id,
                user.Result.Username,
                user.Result.FirstName,
                user.Result.LastName,
                Token = tokenString
            });
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Register([FromBody]UserDto userDto)
        {
            // map dto to entity
            var user = _mapper.Map<Usuario>(userDto);

            try
            {
                // save 
                //_userService.Create(user, userDto.Password);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            var userDtos = _mapper.Map<IList<UserDto>>(users);
            return Ok(userDtos);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var user = _userService.GetById(id);
            var userDto = _mapper.Map<UserDto>(user);
            return Ok(userDto);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody]UserDto userDto)
        {
            // map dto to entity and set id
            var user = _mapper.Map<Usuario>(userDto);
            user.Id = id;

            try
            {
                // save 
                //_userService.Update(user, userDto.Password);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("Delete")]
        public IActionResult Delete([FromBody]int id)
        {
            _userService.Delete(id);
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("recursos")]
        public IActionResult ObtenerRecursos([FromBody]UserDto userDto)
        {
            var menuTask = _userService.ObtenerRecursosTask(userDto.Username);
            return Ok
            (new
            {
                Menu = menuTask.Result.Mensaje
            });
        }

        [AllowAnonymous]
        [HttpPost("auth")]
        public IActionResult Autentica([FromBody]UserDto userDto)
        {

            var usuarioDto = LdapHelper.Authenticate(userDto.Username.ToLower(), userDto.Password, _ldapSettings.Url, _ldapSettings.SearchBase, _ldapSettings.BindDn, _ldapSettings.BindCredentials, _ldapSettings.SearchFilter);

            if (usuarioDto == null)
                return BadRequest();

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                        new Claim(ClaimTypes.Name, usuarioDto.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new
            {
                Id = usuarioDto.Id,
                Username = usuarioDto.Username,
                FirstName = usuarioDto.FirstName,
                LastName = usuarioDto.LastName,
                Token = tokenString
            });


        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public IActionResult AgregaUsuarioPerfil([FromBody] RecursoProcesoDto recursoProcesoDto)
        {
            var usuarioDto = LdapHelper.FindUser(recursoProcesoDto.VcUsuario.ToLower(), _ldapSettings.Url, _ldapSettings.SearchBase, _ldapSettings.BindDn, _ldapSettings.BindCredentials, _ldapSettings.SearchFilter);

            if (usuarioDto == null)
                return BadRequest();

            var notificacionResult = _perfilService.RegistroUsuarioPerfil(usuarioDto.Username, usuarioDto.FirstName, usuarioDto.LastName, usuarioDto.Mail, recursoProcesoDto.IdPerfil, recursoProcesoDto.VcUsuariocreacion.ToLower());

            return Ok(new { Mensaje = notificacionResult });
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public IActionResult EliminaUsuarioPerfil([FromBody] RecursoProcesoDto recursoProcesoDto)
        {
            var notificacionResult = _perfilService.EliminaUsuarioPerfilTask(recursoProcesoDto.VcUsuario.ToLower(), recursoProcesoDto.IdPerfil, recursoProcesoDto.VcUsuariocreacion.ToLower());

            return Ok(new { Mensaje = notificacionResult });
        }
    }
}
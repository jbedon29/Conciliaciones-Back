using System;
using System.Collections.Generic;
using System.Linq;
using Protecta.Application.Service.Helpers;
using Protecta.Application.Service;
using Protecta.Domain.Service.UserModule.Aggregates.UserAgg;
using Protecta.CrossCuting.Log.Contracts;
using AutoMapper;
using System.Threading.Tasks;
using Protecta.Application.Service.Dtos;
using Protecta.Application.Service.Dtos.General;
using Protecta.CrossCuting.Utilities.Security;

namespace Protecta.Application.Service.Services.UserModule
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly ILoginRepository _loginRepository;
        private ILoggerManager _logger;
        private IMapper _mapper;

        public UserService(IUserRepository userRepository,
                            ILoginRepository loginRepository,
                            ILoggerManager logger,
                            IMapper mapper)
        {
            this.userRepository = userRepository;
            _loginRepository = loginRepository;
            _logger = logger;
            _mapper = mapper;
        }


        public async Task<UserDto> Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;

            string sPasswordMD5 = Cryptography.GetMd5Hash(password);

            //traer datos del usuario en base de datos
            var user = await _loginRepository.Authenticate(username, sPasswordMD5);

            // Verifica si el usuario existe
            if (user == null)
                return null;
            //IEnumerable<PRO_RESOURCES> entitiesResources = await _loginRepository.ResourcesRead(user.NIDPROFILE, NTYPERESOURCE);
            var userDtos = _mapper.Map<UserDto>(user);

            // Autenticacion existosa
            return userDtos;
        }

        public async Task<IEnumerable<UserDto>> GetAll()
        {
            UserDto oUsuario = new UserDto();

            List<UserDto> lUser = new List<UserDto>();

            //return _context.Users;
            return lUser;
        }

        public async Task<UserDto> GetById(int id)
        {
            // return _context.Users.Find(id);
            UserDto oUsuario = new UserDto();
            return oUsuario;
        }

        // Crear nuevo usuario
        public UserDto Create(UserDto user, string password)
        {
            // validation
            if (string.IsNullOrWhiteSpace(password))
                throw new AppException("Password is required");

            return user;
        }

        public async Task<PRO_RESOURCES> GetMenu(int perfil)
        {
            PRO_RESOURCES oResources = new PRO_RESOURCES();

            return oResources;
        }

        public async Task Update(UserDto userParam, string password = null) { }

        public async Task Delete(int id)
        {
            //return await userRepository.Remove(id);
        }

        // private helper methods
        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }

        public Task<IEnumerable<PRO_RESOURCES>> resourcesRead(long NIDPROFILE, long? NTYPERESOURCE = null)
        {
            throw new NotImplementedException();
        }

        public async Task<RespuestaDto> ObtenerRecursosTask(string username)
        {
            var menuResult = await _loginRepository.LecturaRecursos(username);

            // Verifica que el resultado no sea nulo
            if (menuResult == null)
                return null;
            //Se mapea el resultado de Producto;
            var resultRespuestaDto = _mapper.Map<RespuestaDto>(menuResult);

            return resultRespuestaDto;
        }

        /*
public async Task<PRO_RESOURCES> Menu(long NIDPROFILE)
{
   const long NTYPERESOURCE = 1; //Type Menu
   IEnumerable<PRO_RESOURCES> entities = await _loginRepository.ResourcesRead(NIDPROFILE, NTYPERESOURCE);
   entities = MMapper.ConvertEntities(entities);

   PRO_RESOURCES mainMenu = new PRO_RESOURCES();
   mainMenu.CHILDREN = entities;

   return mainMenu;
}

public async Task<IEnumerable<PRO_RESOURCES>> resourcesRead(long NIDPROFILE, long? NTYPERESOURCE = null)
{
   //SecurityBussines securityBussines = new SecurityBussines();
   IEnumerable<PRO_RESOURCES> entities = await _loginRepository.ResourcesRead(NIDPROFILE, NTYPERESOURCE);
   if (NTYPERESOURCE == null)
   {
       entities = MMapper.UnconvertEntitiesFathers(entities);
   }

   return entities;
}

*/
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query.ExpressionVisitors.Internal;
using Protecta.Application.Service.Dtos;
using Protecta.Application.Service.Dtos.General;
using Protecta.Domain.Service.UserModule.Aggregates.UserAgg;

namespace Protecta.Application.Service.Services.UserModule
{
    public interface IUserService
    {
        Task<UserDto> Authenticate(string username, string password);
        Task<IEnumerable<UserDto>> GetAll();
        Task<UserDto> GetById(int id);
        UserDto Create(UserDto user, string password);
        Task Update(UserDto user, string password = null);
        Task Delete(int id);
        Task<PRO_RESOURCES> GetMenu(int perfil);
        Task<IEnumerable<PRO_RESOURCES>> resourcesRead(long NIDPROFILE, long? NTYPERESOURCE = null);
        Task<RespuestaDto> ObtenerRecursosTask(string username);
    }
}

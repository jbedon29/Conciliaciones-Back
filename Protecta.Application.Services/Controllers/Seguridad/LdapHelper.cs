using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Features.Authentication;
using Microsoft.Extensions.Options;
using Protecta.Application.Service.Services.UserModule;
using Protecta.CrossCuting.Utilities.Configuration;
using Novell.Directory.Ldap;
using Protecta.Application.Service.Dtos;

namespace Protecta.Application.Service.Controllers.Seguridad
{
    public class LdapHelper
    {

        public static UserDto Authenticate(string username, string password, string ldapHost, string searchBase, string bindDn, string bindCredentials, string searchFilter)
        {
            var lc = new LdapConnection();
            var usuarioDto = new UserDto();
            const int ldapPort = LdapConnection.DEFAULT_PORT;
            try
            {

                lc.Connect(ldapHost, ldapPort);
                lc.Bind(bindDn, bindCredentials);
                var result = lc.Search(
                    searchBase,
                    LdapConnection.SCOPE_SUB,
                    string.Format(searchFilter, username),
                    null,
                    false
                );

                var user = result.HasMore() ? result.Next() : null;

                if (user != null && password != null && !password.Equals(string.Empty))
                {
                    lc.Bind(user.DN, password);
                    usuarioDto.Id = 1;
                    usuarioDto.Username = user.getAttribute("sAMAccountName") != null ? user.getAttribute("sAMAccountName").StringValue : "";
                    usuarioDto.FirstName = user.getAttribute("givenName") != null ? user.getAttribute("givenName").StringValue : "";
                    usuarioDto.LastName = user.getAttribute("sn") != null ? user.getAttribute("sn").StringValue : "";
                    usuarioDto.Mail = user.getAttribute("mail") != null ? user.getAttribute("mail").StringValue : "admin@protecta.com.pe";
                }

                lc.Disconnect();

            }
            catch (LdapException e)
            {
                Console.WriteLine("Error: " + e.ToString());
                return null;
            }

            return usuarioDto;
        }

        public static UserDto FindUser(string username, string ldapHost, string searchBase, string bindDn, string bindCredentials, string searchFilter)
        {
            var lc = new LdapConnection();
            var usuarioDto = new UserDto();
            const int ldapPort = LdapConnection.DEFAULT_PORT;
            try
            {

                lc.Connect(ldapHost, ldapPort);
                lc.Bind(bindDn, bindCredentials);
                var result = lc.Search(
                    searchBase,
                    LdapConnection.SCOPE_SUB,
                    string.Format(searchFilter, username),
                    null,
                    false
                );

                var user = result.HasMore() ? result.Next() : null;

                if (user != null)
                {
                    usuarioDto.Id = 1;
                    usuarioDto.Username = user.getAttribute("sAMAccountName") != null ? user.getAttribute("sAMAccountName").StringValue : "";
                    usuarioDto.FirstName = user.getAttribute("givenName") != null ? user.getAttribute("givenName").StringValue : "";
                    usuarioDto.LastName = user.getAttribute("sn") != null ? user.getAttribute("sn").StringValue : "";
                    usuarioDto.Mail = user.getAttribute("mail") != null ? user.getAttribute("mail").StringValue : "admin@protecta.com.pe";
                }

                lc.Disconnect();

            }
            catch (LdapException e)
            {
                Console.WriteLine("Error: " + e.ToString());
                return null;
            }

            return usuarioDto;
        }

    }
}
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.ComponentModel;

namespace Protecta.CrossCuting.Utilities.Configuration
{
    public class ImpersonateUser : IDisposable
    {
        protected const int LOGON32_PROVIDER_DEFAULT = 0;
        protected const int LOGON32_LOGON_INTERACTIVE = 2;
        public const int LOGON32_LOGON_NETWORK_CLEARTEXT = 8;// Win2K or higher
        public const int LOGON32_LOGON_NEW_CREDENTIALS = 9;// Win2K or higher

        public WindowsIdentity Identity = null;
        private System.IntPtr m_accessToken;


        [System.Runtime.InteropServices.DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool LogonUser(string lpszUsername, string lpszDomain,
        string lpszPassword, int dwLogonType, int dwLogonProvider, ref System.IntPtr phToken);

        [System.Runtime.InteropServices.DllImport("kernel32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        private extern static bool CloseHandle(System.IntPtr handle);


        // AccessToken ==> this.Identity.AccessToken
        //public Microsoft.Win32.SafeHandles.SafeAccessTokenHandle AT
        //{
        //    get
        //    {
        //        var at = new Microsoft.Win32.SafeHandles.SafeAccessTokenHandle(this.m_accessToken);
        //        return at;
        //    }
        //}


        public ImpersonateUser()
        {
            this.Identity = WindowsIdentity.GetCurrent();
        }


        public ImpersonateUser(string username, string domain, string password)
        {
            Login(username, domain, password);
        }


        public void Login(string username, string domain, string password)
        {
            if (this.Identity != null)
            {
                this.Identity.Dispose();
                this.Identity = null;
            }


            try
            {
                this.m_accessToken = new System.IntPtr(0);
                Logout();

                this.m_accessToken = System.IntPtr.Zero;
                bool logonSuccessfull = LogonUser(
                   username,
                   domain,
                   password,
                   LOGON32_LOGON_NEW_CREDENTIALS,
                   LOGON32_PROVIDER_DEFAULT,
                   ref this.m_accessToken);

                if (!logonSuccessfull)
                {
                    int error = System.Runtime.InteropServices.Marshal.GetLastWin32Error();
                    throw new System.ComponentModel.Win32Exception(error);
                }
                Identity = new WindowsIdentity(this.m_accessToken);
            }
            catch
            {
                throw;
            }

        } // End Sub Login 


        public void Logout()
        {
            if (this.m_accessToken != System.IntPtr.Zero)
                CloseHandle(m_accessToken);

            this.m_accessToken = System.IntPtr.Zero;

            if (this.Identity != null)
            {
                this.Identity.Dispose();
                this.Identity = null;
            }

        } // End Sub Logout 


 

        void IDisposable.Dispose()
        {
            Logout();
        }
    }
}

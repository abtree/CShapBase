using System;
using System.Security;
using System.Security.Claims;
using System.Security.Permissions;
using System.Security.Principal;

namespace Account
{
    class Account
    {
        static void Main(string[] args)
        {
            //设置windows环境
            AppDomain.CurrentDomain.SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);

            var principal = WindowsPrincipal.Current as WindowsPrincipal;
            var identify = principal.Identity as WindowsIdentity;
            Console.WriteLine("IdentifyType: {0}", identify.ToString());
            Console.WriteLine("Name: {0}", identify.Name);
            foreach (var group in identify.Groups) {
                Console.WriteLine("Group: {0}", group.Value);
            }
            Console.WriteLine("'User'?: {0}", principal.IsInRole(WindowsBuiltInRole.User));
            Console.WriteLine("'Administrator'? {0}", principal.IsInRole(WindowsBuiltInRole.Administrator));
            Console.WriteLine("Authenticated: {0}", identify.IsAuthenticated);
            Console.WriteLine("AuthType: {0}", identify.AuthenticationType);
            Console.WriteLine("Anonymous: {0}", identify.IsAnonymous);
            Console.WriteLine("Token: {0}", identify.Token);

            try
            {
                ShowMessage();
            }
            catch(SecurityException ex)
            {
                Console.WriteLine("Security exception caught ({0})", ex.Message);
                Console.WriteLine("The current principal must be in local Users group");
            }

            //查看声明(claim)信息
            foreach(var claim in principal.Claims)
            {
                Console.WriteLine("Subject: {0}", claim.Subject);
                Console.WriteLine("Issuer: {0}", claim.Issuer);
                Console.WriteLine("Type: {0}", claim.Type);
                Console.WriteLine("Value type: {0}", claim.ValueType);
                Console.WriteLine("Value: {0}", claim.Value);
                foreach(var prop in claim.Properties)
                {
                    Console.WriteLine("\tProperty: {0} {1}", prop.Key, prop.Value);
                }
                Console.WriteLine();
            }
        }

        //可以用注解限制函数的访问权限
        [PrincipalPermission(SecurityAction.Demand, Role = "S-1-1-0")]
        static void ShowMessage()
        {
            Console.WriteLine("The current principal is logged in locally");
            Console.WriteLine("(member of the local Users group)");
        }
    }
}

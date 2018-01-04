using System;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;

namespace FileIdentifyRW
{
    class FileIdentifyRW
    {
        static void ReadIdentify(string filename)
        {
            using(FileStream stream = File.Open(filename, FileMode.Open))
            {
                FileSecurity securityDescriptor = stream.GetAccessControl();
                AuthorizationRuleCollection rules = securityDescriptor.GetAccessRules(true, true, typeof(NTAccount));

                foreach(AuthorizationRule rule in rules)
                {
                    var fileRule = rule as FileSystemAccessRule;
                    Console.WriteLine("Access type: {0}", fileRule.AccessControlType);
                    Console.WriteLine("Rights: {0}", fileRule.FileSystemRights);
                    Console.WriteLine("Identify: {0}", fileRule.IdentityReference.Value);
                    Console.WriteLine();
                }
            }
        }

        private static void WriteIdentify(string filename)
        {
            //这里设置用户组
            var salesIdentify = new NTAccount("Sales");
            var developersIdentity = new NTAccount("Developers");
            var everyOneIdentify = new NTAccount("Everyone");

            //这里设置访问权限
            var salesAce = new FileSystemAccessRule(salesIdentify, FileSystemRights.Write, AccessControlType.Deny);
            var everyOneAce = new FileSystemAccessRule(everyOneIdentify, FileSystemRights.Read, AccessControlType.Allow);
            var developersAce = new FileSystemAccessRule(developersIdentity, FileSystemRights.FullControl, AccessControlType.Allow);

            var securityDescriptor = new FileSecurity();
            securityDescriptor.SetAccessRule(everyOneAce);
            securityDescriptor.SetAccessRule(developersAce);
            securityDescriptor.SetAccessRule(salesAce);

            File.SetAccessControl(filename, securityDescriptor);
        }
    }
}

using System;
using System.IO;
using System.Security.Principal;
using System.Security.AccessControl;

namespace FileAccessControl
{
    class FileAccessControl
    {
        /// <summary>
        /// 读取文件权限
        /// </summary>
        /// <param name="dirpath"></param>
        public void ReadACL(string dirpath)
        {
            try
            {
                DirectoryInfo myDir = new DirectoryInfo(dirpath);
                if (myDir.Exists)
                {
                    DirectorySecurity myDc = myDir.GetAccessControl();
                    foreach (FileSystemAccessRule fileRule in myDc.GetAccessRules(true, true, typeof(NTAccount))){
                        Console.WriteLine("{0} {1} {2} access for {3}", dirpath, fileRule.AccessControlType == AccessControlType.Allow ? "provides" : "denies", fileRule.FileSystemRights, fileRule.IdentityReference);
                    }
                }
            }
            catch
            {
                Console.WriteLine("InCorrect directory provided!");
            }
        }

        /// <summary>
        /// 设置文件权限
        /// </summary>
        /// <param name="filename"></param>
        public void WriteACL(string filename)
        {
            try
            {
                FileStream myFile = new FileStream(filename, FileMode.Open, FileAccess.ReadWrite);
                FileSecurity fileSec = myFile.GetAccessControl();

                FileSystemAccessRule newRule = new FileSystemAccessRule(new NTAccount(@"Admin\Name"), FileSystemRights.FullControl, AccessControlType.Allow);
                fileSec.AddAccessRule(newRule);  //这里需要权限
                File.SetAccessControl(filename, fileSec);

                foreach (FileSystemAccessRule fileRule in fileSec.GetAccessRules(true, true, typeof(NTAccount)))
                {
                    Console.WriteLine("{0} {1} {2} access for {3}", filename, fileRule.AccessControlType == AccessControlType.Allow ? "provides" : "denies", fileRule.FileSystemRights, fileRule.IdentityReference);
                }
            }
            catch
            {
                Console.WriteLine("InCorrect directory provided!");
            }
        }

        public static void Main()
        {
            FileAccessControl fac = new FileAccessControl();
            fac.WriteACL("a.txt");
        }
    }
}

using System;
using System.IO;

namespace FileInfo_
{
    class FileInfo_
    {
        /// <summary>
        /// 查看文件信息
        /// </summary>
        /// <param name="filename"></param>
        protected void DisplayFileInfo(string filename)
        {
            FileInfo thefile = new FileInfo(filename);

            if (!thefile.Exists)  // file not found
                return;

            Console.WriteLine("Name: {0}", thefile.Name);
            Console.WriteLine("CreateTime: {0}", thefile.CreationTime.ToLongTimeString());
            Console.WriteLine("Last Access Time: {0}", thefile.LastAccessTime.ToLongTimeString());
            Console.WriteLine("Last Write Time: {0}", thefile.LastWriteTime.ToLongTimeString());
            Console.WriteLine("File Size: {0} bytes", thefile.Length.ToString());
        }

        /// <summary>
        /// 查看文件夹信息
        /// </summary>
        /// <param name="folderpath"></param>
        protected void DisplayFolderList(string folderpath)
        {
            DirectoryInfo theFolder = new DirectoryInfo(folderpath);

            if (!theFolder.Exists)  //目录不存在
                return;

            Console.WriteLine("FolderPath: {0}", theFolder.FullName);
            Console.WriteLine("FolderName: {0}", theFolder.Name);

            //list all subfolders in folder
            foreach(DirectoryInfo nextfolder in theFolder.GetDirectories())
            {
                DisplayFileInfo(nextfolder.FullName);
            }

            //list all files in folder
            foreach(FileInfo nextFile in theFolder.GetFiles())
            {
                string path = Path.Combine(folderpath, nextFile.Name);  //这里获得文件的完整路径
                Console.WriteLine("File name: {0}", path);
            }
        }

        /// <summary>
        /// 操纵文件
        /// </summary>
        /// <param name="filefullname"></param>
        protected void FileHandle(string filefullname)
        {
            string path = Path.GetDirectoryName(filefullname);
            string name = "newName" + Path.GetExtension(filefullname);
            string path1 = Path.Combine(path, name);
            File.Move(filefullname, path1);  //移动
            name = "NewCopy" + Path.GetExtension(filefullname);
            path1 = Path.Combine(path, name);
            File.Copy(filefullname, path1);  //复制
            File.Delete(filefullname);  //删除
        }
    }
}

using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Text;

namespace ReadWrite
{
    class ReadWrite
    {
        /// <summary>
        /// 直接读出整个文件内容
        /// </summary>
        /// <param name="filename"></param>
        public void FileRead(string filename)
        {
            string filetext = File.ReadAllText(filename);
        }

        /// <summary>
        /// 写入一个文件 如果文件不存在 创建 如果存在 覆盖
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="txt"></param>
        public void WriteFile(string filename, string txt)
        {
            File.WriteAllText(filename, txt);
        }

        /// <summary>
        /// FileStream 实例 将文件内容读出 以16进制显示
        /// </summary>
        /// <param name="filename"></param>
        public void BinaryFileReader(string filename)
        {
            FileStream inStream = new FileStream(filename, FileMode.Open, FileAccess.Read);
            StringBuilder sb = new StringBuilder();
            sb.Capacity = Convert.ToInt32(inStream.Length);
            while (true)
            {
                int nextByte = inStream.ReadByte();
                if (nextByte < 0)
                    break;

                char nextChar = (char)nextByte;
                if (nextChar < 16)
                    sb.Append(" 0x" + string.Format("{0,1:x}", nextByte));
                else if (char.IsLetterOrDigit(nextChar) || char.IsPunctuation(nextChar))
                    sb.Append(" " + nextChar + " ");
                else
                    sb.Append(" x" + string.Format("0,2:x", nextByte));
            }
            inStream.Close();
            Console.Write(sb.ToString());
        }

        public void TxtFileRW(string filename)
        {
            StreamReader sr = new StreamReader(filename, Encoding.UTF8);
            StreamWriter sw = new StreamWriter(filename, false, Encoding.UTF8);
            sr.Close();
            sw.Close();
        }

        public void MemoryFile(string filename)
        {
            //创建一个内存中的文件 值得注意的是 磁盘上的文件的大小也会为1024 * 1024
            using (var mmFile = MemoryMappedFile.CreateFromFile(filename, FileMode.Create, "fileHandle", 1024 * 1024))
            {
                string valueToWrite = "Written to the mapped-memory file on " + DateTime.Now.ToString();
                var myAccessor = mmFile.CreateViewAccessor();  //这里创建一个映射文件的访问器

                myAccessor.WriteArray<byte>(0, Encoding.ASCII.GetBytes(valueToWrite), 0, valueToWrite.Length);

                var readout = new byte[valueToWrite.Length];
                myAccessor.ReadArray<byte>(0, readout, 0, readout.Length);
                var finalValue = Encoding.ASCII.GetString(readout);

                Console.WriteLine("Message: {0}", finalValue);
                Console.ReadKey();
            }
        }

        /// <summary>
        /// 获取磁盘驱动器(比如 C盘 D盘)信息
        /// </summary>
        public void DriveRead()
        {
            //获取计算机中所有磁盘的驱动器
            DriveInfo[] dis = DriveInfo.GetDrives();
            foreach(DriveInfo di in dis)
            {
                Console.WriteLine("Available Free Space: {0}", di.AvailableFreeSpace);
                Console.WriteLine("Drive Format: {0}", di.DriveFormat);
                Console.WriteLine("Drive Type: {0}", di.DriveType);
                Console.WriteLine("Is Ready: {0}", di.IsReady);
                Console.WriteLine("Name: {0}", di.Name);
                Console.WriteLine("Root Directory: {0}", di.RootDirectory);
                Console.WriteLine("ToString Value: {0}", di);
                Console.WriteLine("Total Free Space: {0}", di.TotalFreeSpace);
                Console.WriteLine("Total Size: {0}", di.TotalSize);
                Console.WriteLine("Volume Label: {0}", di.VolumeLabel);
                Console.WriteLine();
            }
        }
        
        //public static void Main()
        //{
        //    ReadWrite rw = new ReadWrite();
        //    //rw.BinaryFileReader("a.txt");
        //    //rw.MemoryFile("b.txt");
        //    rw.DriveRead();
        //}
    }
}

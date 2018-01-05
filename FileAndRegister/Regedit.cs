using System;
using Microsoft.Win32;

namespace Regedit
{
    /// <summary>
    /// 对注册表的操作需要权限
    /// </summary>
    class Regedit
    {
        public void Handle()
        {
            //读取特定目录 (需要一层层的读)
            RegistryKey hklm = Registry.LocalMachine;
            RegistryKey hkSoftWare = hklm.OpenSubKey("Software");
            RegistryKey hkMicroft = hkSoftWare.OpenSubKey("Microsoft", true);  //后面的true表示可以进行写操作（需要权限）

            RegistryKey hkMine = hkMicroft.CreateSubKey("MyOwnSoft");  //如果MyOwnSoft不存在 就创建 如果存在 就返回
            hkMine.SetValue("MyStringValue", "HelloWorld");  //设置值
            hkMine.SetValue("MyIntValue", 20);

            string key = (string)hkMine.GetValue("MyStringValue");
            int ival = (int)hkMine.GetValue("MyIntValue");

            //读完或写完需关闭
            hkMine.Close();
        }
    }
}

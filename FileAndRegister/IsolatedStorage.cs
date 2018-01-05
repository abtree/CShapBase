using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Text;
using System.Xml;

namespace IsolatedStorage
{
    class IsolatedStorage
    {
        public void SaveSetting()
        {
            IsolatedStorageFile storFile = IsolatedStorageFile.GetUserStoreForDomain();
            IsolatedStorageFileStream storStream = new IsolatedStorageFileStream("SelfPlacingWindow.xml", FileMode.Create, FileAccess.Write);

            XmlTextWriter writer = new XmlTextWriter(storStream, Encoding.UTF8);
            writer.Formatting = Formatting.Indented;

            writer.WriteStartDocument();
            writer.WriteStartElement("Settings");

            writer.WriteStartElement("BackColor");
            writer.WriteValue("Red");
            writer.WriteEndElement();

            writer.WriteStartElement("Width");
            writer.WriteValue(15);
            writer.WriteEndElement();

            writer.WriteStartElement("Height");
            writer.WriteValue(15);
            writer.WriteEndElement();

            writer.WriteEndElement();
            writer.Flush();
            writer.Close();

            storStream.Close();
            storFile.Close();
        }

        public void ReadSetting()
        {
            IsolatedStorageFile storFile = IsolatedStorageFile.GetUserStoreForDomain();
            string[] userFiles = storFile.GetFileNames("SelfPlacingWindow.xml");

            foreach (string userFile in userFiles)
            {
                if(userFile == "SelfPlacingWindow.xml")
                {
                    StreamReader storStream = new StreamReader(new IsolatedStorageFileStream("SelfPlacingWindow.xml", FileMode.Open, storFile));
                    XmlTextReader reader = new XmlTextReader(storStream);

                    while (reader.Read())
                    {
                        switch (reader.Name)
                        {
                            case "BackColor":
                                Console.WriteLine("backcolor: {0}", reader.ReadString());
                                break;
                            case "Width":
                                Console.WriteLine("Width: {0}", reader.ReadString());
                                break;
                            case "Height":
                                Console.WriteLine("Height: {0}", reader.ReadString());
                                break;
                            default:
                                break;
                        }
                    }

                    reader.Close();
                    storStream.Close();
                }
            }
            storFile.Close();
        }
    }
}

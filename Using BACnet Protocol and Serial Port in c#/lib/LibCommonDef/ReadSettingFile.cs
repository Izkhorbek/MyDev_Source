using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace LibCommonDef
{
    public class ReadSettingFile<TSetting> where TSetting : new()
    {
        public static TSetting Read(string fileName)
        {
            TSetting result = new TSetting();
            try
            { 
                XmlSerializer serializer = new XmlSerializer(typeof(TSetting));
                serializer.UnknownNode += serializer_UnknownNode;
                serializer.UnknownAttribute += new XmlAttributeEventHandler(serializer_UnknownAttribute);

                FileStream fs = new FileStream(fileName, FileMode.Open);

                result = (TSetting)serializer.Deserialize(fs);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Check the file {ex.Message}");
            }

            return result;
        }
  
        private static void serializer_UnknownNode(object sender, XmlNodeEventArgs e)
        {
            Console.WriteLine("Unknown Node:" + e.Name + "\t" + e.Text);
        }

        private static void serializer_UnknownAttribute(object sender, XmlAttributeEventArgs e)
        {
            System.Xml.XmlAttribute attr = e.Attr;
            Console.WriteLine("Unknown attribute " + attr.Name + "='" + attr.Value + "'");
        }
    }
}

using log4net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace HIB.Outlook.Sync.Common
{
    public enum XMLFolderType
    {
        AddIn,
        Service
    }
    public static class XMLSerializeHelper
    {
        private const string filePath = @"C:\Heffernan.DB";

        private static readonly ILog Logger = LogManager.GetLogger(typeof(XMLSerializeHelper));
        public static void Serialize<T>(this List<T> value, XMLFolderType folderType, string filename = null)
        {
            try
            {
                var fileName = string.Empty;
                if (filename == null)
                    fileName = typeof(T).Name;
                else
                    fileName = filename;

                string folderPath = Path.Combine(filePath, folderType.ToString());
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                string XmlfilePath = Path.Combine(folderPath, fileName + ".xml");

                var result = DeSerialize<T>(folderType);


                result.AddRange(value);

                try
                {
                    XmlSerializer xmlSerialiser = new XmlSerializer(typeof(List<T>));

                    TextWriter Filestream = new StreamWriter(XmlfilePath);

                    xmlSerialiser.Serialize(Filestream, result);

                    Filestream.Close();


                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

        }

        //public static void CreateInstance<T>(XMLFolderType folderType)
        //{
        //    try
        //    {
        //        xmlSerialiser = new XmlSerializer(typeof(List<T>));
        //        DeSerialize<T>(folderType);
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error(ex);
        //    }
        //}

        public static List<T> DeSerialize<T>(XMLFolderType folderType, string filename = null)
        {
            List<T> type = new List<T>();
            try
            {
                var fileName = string.Empty;
                if (filename == null)
                    fileName = typeof(T).Name;
                else
                    fileName = filename;
                string folderPath = Path.Combine(filePath, folderType.ToString());
                string XmlfilePath = Path.Combine(folderPath, fileName + ".xml");

                if (Directory.Exists(folderPath) && File.Exists(XmlfilePath))
                {
                    try
                    {
                        XmlSerializer serialiser = new XmlSerializer(typeof(List<T>));
                        FileStream fs = new FileStream(XmlfilePath, FileMode.Open);
                        XmlReader reader = XmlReader.Create(fs);
                        type = serialiser.Deserialize(reader) as List<T>;
                        fs.Close();
                        if (!string.IsNullOrEmpty(XmlfilePath) && File.Exists(XmlfilePath))
                        {
                            File.Delete(XmlfilePath);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(ex);
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return type;
        }
    }
}

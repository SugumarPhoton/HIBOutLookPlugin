using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace HIB.Outlook.Helper.Common
{
    public enum XMLFolderType
    {
        AddIn,
        Service,
        Notification,
        UserDetail
    }
    public static class XMLSerializeHelper
    {
        private static readonly object lockObject = new object();
        private static readonly string filePath = ConfigurationManager.AppSettings["MainFolderPath"]?.ToString();
        public static void SerializeOnly<T>(this List<T> value, XMLFolderType folderType, string filename = null)
        {
            try
            {
                if (value.Count > 0)
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
                    try
                    {
                        lock (lockObject)
                        {
                            XmlSerializer xmlSerialiser = new XmlSerializer(typeof(List<T>));
                            using (TextWriter Filestream = new StreamWriter(XmlfilePath))
                            {
                                xmlSerialiser.Serialize(Filestream, value);
                                Filestream.Close();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
                    }
                }


            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
            }
            finally
            {
                Logger.save();
            }

        }

        public static void Serialize<T>(this List<T> value, XMLFolderType folderType, string filename = null)
        {
            try
            {
                if (value.Count > 0)
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
                        lock (lockObject)
                        {
                            XmlSerializer xmlSerialiser = new XmlSerializer(typeof(List<T>));
                            using (TextWriter Filestream = new StreamWriter(XmlfilePath))
                            {
                                xmlSerialiser.Serialize(Filestream, result);
                                Filestream.Close();
                                Filestream.Dispose();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
            }
            finally
            {
                Logger.save();
            }

        }

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
                        lock (lockObject)
                        {
                            XmlSerializer serialiser = new XmlSerializer(typeof(List<T>));
                            using (FileStream fs = new FileStream(XmlfilePath, FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read))
                            {
                                XmlReader reader = XmlReader.Create(fs);
                                type = serialiser.Deserialize(reader) as List<T>;
                                fs.Close();
                                reader.Close();
                            }
                            if (!string.IsNullOrEmpty(XmlfilePath) && File.Exists(XmlfilePath))
                            {
                                File.Delete(XmlfilePath);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
            }
            finally
            {
                // Logger.save();
            }
            return type;
        }
        public static List<T> DeSerializeOnly<T>(XMLFolderType folderType, string filename = null)
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
                        lock (lockObject)
                        {
                            XmlSerializer serialiser = new XmlSerializer(typeof(List<T>));
                            using (FileStream fs = new FileStream(XmlfilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
                            {
                                XmlReader reader = XmlReader.Create(fs);
                                type = serialiser.Deserialize(reader) as List<T>;
                                fs.Close();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
                    }

                }

            }
            catch (Exception ex)
            {
                Logger.ErrorLog(ex, Logger.SourceType.WindowsService, "");
            }
            finally
            {
                // Logger.save();
            }
            return type;
        }
    }
}

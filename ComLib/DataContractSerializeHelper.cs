using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Jpsys.SagyoManage.ComLib
{
    /// <summary>
    /// DataContractSerializer を使用したシリアライズのヘルパメソッドを提供します。
    /// </summary>
    public static class DataContractSerializeHelper
    {
        public static string SerializeToString(object obj)
        {
            System.Text.StringBuilder sbOutput = new System.Text.StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings()
            {
                Indent = true,
                NewLineOnAttributes = true,
                //改行コードを保持する
                NewLineHandling = NewLineHandling.Entitize,
            };

            using (XmlWriter sw = XmlWriter.Create(sbOutput, settings))
            {
                DataContractSerializer serializer = new DataContractSerializer(obj.GetType());
                serializer.WriteObject(sw, obj);
            }

            return sbOutput.ToString();
        }

        public static T DeserializeFromString<T>(string text)
        {
            using (StringReader sr = new StringReader(text))
            {
                XmlReaderSettings settings = new XmlReaderSettings();

                var xmlReader = XmlReader.Create(sr);
                DataContractSerializer serializer = new DataContractSerializer(typeof(T));
                return (T)serializer.ReadObject(xmlReader);
            }
        }
    }
}

using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

namespace Blartenix
{
    /// <summary>
    /// Some of general Blartenix utilities methods
    /// </summary>
    public static class Utilities
    {
        /// <summary>
        /// Encodes a string to a Base64 string representation
        /// </summary>
        /// <param name="decodedString">A simple string</param>
        /// <returns></returns>
        public static string EncodeString(string decodedString)
        {
            return Convert.ToBase64String(Encoding.ASCII.GetBytes(decodedString));
        }

        /// <summary>
        /// Decodes a Base64 string.
        /// </summary>
        /// <param name="encodedString">A Base64 string</param>
        /// <returns></returns>
        public static string DecodeString(string encodedString)
        {
            return Encoding.ASCII.GetString(Convert.FromBase64String(encodedString));
        }

        public static T DeserializeXML<T>(string xml, bool xmlIsAFile)
        {
            StreamReader strReader = null;
            XmlTextReader xmlReader = null;
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(BlartenixLanguage));
                object obj;
                if (xmlIsAFile)
                {
                    strReader = new StreamReader(xml);
                    xmlReader = new XmlTextReader(xml);
                    obj = serializer.Deserialize(xmlReader);
                }
                else
                {
                    TextReader reader = new StringReader(xml);
                    obj = serializer.Deserialize(reader);
                }

                return (T)obj;
            }
            catch (Exception e)
            {
                Debug.LogError(e.ToString());
            }
            finally
            {
                if (xmlReader != null)
                {
                    xmlReader.Close();
                }
                if (strReader != null)
                {
                    strReader.Close();
                }
            }

            return default;
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Blartenix
{
    [XmlRoot("Language")]
    [Serializable]
    public class BlartenixLanguage
    {
        public const string NAMESPACE = "BlartenixLanguage-v2.0";

        [XmlElement(ElementName = "TemplateID", Namespace = NAMESPACE)]
        public string templateID = string.Empty;

        [XmlElement(ElementName = "Name", Namespace = NAMESPACE)]
        public string name = string.Empty;

        [XmlArray(ElementName = "LanguageTextList", Namespace = NAMESPACE)]
        [XmlArrayItem(ElementName = "LanguageText", Namespace = NAMESPACE)]
        public List<LanguageTextXmlTag> languageTexts = new List<LanguageTextXmlTag>();



        [XmlNamespaceDeclarations]
        protected XmlSerializerNamespaces Namespaces { get; }

        public BlartenixLanguage()
        {
            Namespaces = new XmlSerializerNamespaces(new XmlQualifiedName[] {
                new XmlQualifiedName("blx", NAMESPACE)
            });
        }

        public string ToXml()
        {
            try
            {
                var stringwriter = new Utf8StringWriter();
                var serializer = new XmlSerializer(GetType());
                serializer.Serialize(stringwriter, this, Namespaces);
                string xml = stringwriter.ToString();
                return xml;
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError(e);
            }

            return null;
        }

        public string ToXml(List<LanguageTextXmlTag> languageTexts)
        {
            try
            {
                BlartenixLanguage tempLang = new BlartenixLanguage
                {
                    templateID = this.templateID,
                    name = this.name,
                    languageTexts = languageTexts
                };

                var stringwriter = new Utf8StringWriter();
                var serializer = new XmlSerializer(tempLang.GetType());
                serializer.Serialize(stringwriter, tempLang, tempLang.Namespaces);
                string xml = stringwriter.ToString();
                return xml;
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError(e);
            }

            return null;
        }

        private class Utf8StringWriter : StringWriter
        {
            public override Encoding Encoding => Encoding.UTF8;
        }
    }
}
using System;
using System.Xml.Serialization;

namespace Blartenix
{
    [Serializable]
    public class LanguageTextXmlTag
    {
        [XmlAttribute(AttributeName = "idName")]
        public string idName = string.Empty;
        [XmlText]
        public string value = string.Empty;

        public LanguageTextXmlTag() {}

        public LanguageTextXmlTag(string idName, string value)
        {
            this.idName = idName;
            this.value = value;
        }
    }
}
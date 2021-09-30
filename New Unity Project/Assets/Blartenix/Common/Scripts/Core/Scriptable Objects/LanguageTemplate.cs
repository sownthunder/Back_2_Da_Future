using System.Collections.Generic;
using UnityEngine;

namespace Blartenix
{
    [CreateAssetMenu(fileName = "New Language Template", menuName = "Blartenix/Language Template")]
    public class LanguageTemplate : ScriptableObject
    {
        [SerializeField]
        private string id = string.Empty;
        [SerializeField]
        private LanguageTemplate[] groupedTemplates = null;
        [SerializeField]
        private BlartenixLanguage language = null;
        [SerializeField]
        private string[] languageTextIdNames = null;

        public string[] LanguageTextIdNames => languageTextIdNames;

        public string Export()
        {
            language.templateID = id;
            language.name = "#LANGUAGE_NAME#";

            if (groupedTemplates != null && groupedTemplates.Length > 0)
                return language.ToXml(GetLanguagesText());

            return language.ToXml();
        }

        private List<LanguageTextXmlTag> GetLanguagesText()
        {
            List<LanguageTextXmlTag> langTexts = new List<LanguageTextXmlTag>();

            for (int i = 0; i < groupedTemplates.Length; i++)
            {
                langTexts.AddRange(groupedTemplates[i].GetLanguagesText());
            }

            langTexts.AddRange(language.languageTexts);
            
            return langTexts;
        }
    }
}
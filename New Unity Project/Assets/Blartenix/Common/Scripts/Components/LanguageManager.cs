using System;
using System.Collections.Generic;
using UnityEngine;

namespace Blartenix
{
    public class LanguageManager : SingletonBehaviour<LanguageManager>
    {
        public event Action OnChangeLanguage;

        [SerializeField]
        private GameSettings gameSettings = null;
        [SerializeField]
        private List<TextAsset> languageFiles = null;
        [SerializeField]
        private int selectedLanguage = 0;

        private BlartenixLanguage CurrentLanguage { get; set; }
        private IList<string> LanguagesNames { get; set; }
        public int SelectedLanguage => selectedLanguage;



        protected override void OnAwake()
        {
            if(gameSettings != null)
            {
                selectedLanguage = gameSettings.Language;
            }

            CurrentLanguage = LoadLanguage(selectedLanguage);
        }



        private BlartenixLanguage LoadLanguage(int languageFileIndex)
        {
            return Utilities.DeserializeXML<BlartenixLanguage>(languageFiles[languageFileIndex].text, false);
        }

        internal void SetLanguage(int languageIndex)
        {
            if (selectedLanguage == languageIndex) return;

            selectedLanguage = languageIndex;
            
            if(gameSettings != null)
            {
                gameSettings.Language = selectedLanguage;
            }

            CurrentLanguage = LoadLanguage(selectedLanguage);
            
            OnChangeLanguage?.Invoke();
        }

        internal IList<string> GetLanguagesNames()
        {
            if (LanguagesNames == null)
            {
                LanguagesNames = new List<string>();
                for (int i = 0; i < languageFiles.Count; i++)
                {
                    LanguagesNames.Add(LoadLanguage(i).name);
                }
            }


            return LanguagesNames;
        }

        internal string GetText(string idName)
        {
            LanguageTextXmlTag text = CurrentLanguage.languageTexts.Find(t => t.idName == idName);
            if (text != null)
                return System.Text.RegularExpressions.Regex.Unescape(text.value);
            else
                Debug.LogError($"Language text with id '{idName}' not found in language file");

            return null;
        }
    }
}
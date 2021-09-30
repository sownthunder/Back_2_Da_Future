using UnityEngine;
using UnityEngine.UI;

namespace Blartenix
{
    public class LanguageText : MonoBehaviour
    {
        [SerializeField]
        private LanguageTemplate languageTemplate;
        [SerializeField]
        private string idName = null;
        [SerializeField]
        [HideInInspector]
        private int idNameIndex;
        [SerializeField]
        private Text text = null;

        public string Text => text.text;

        private void Start()
        {
            if (LanguageManager.Instance != null)
            {
                LanguageManager.Instance.OnChangeLanguage += SetText;
                SetText();
            }
        }

        private void OnDestroy()
        {
            if(LanguageManager.Instance != null)
                LanguageManager.Instance.OnChangeLanguage -= SetText;
        }

        private void SetText()
        {
            text.text = LanguageManager.Instance.GetText(idName);
        }


    }
}
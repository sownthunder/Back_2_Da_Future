using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Blartenix.Demos.Common
{
    public class bxMultiLanguageManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject settingsMenu = null;
        [SerializeField]
        private Dropdown languagesDropdown = null;

        private void Awake()
        {
            languagesDropdown.onValueChanged.AddListener(OnDropdownValueChange);
            settingsMenu.SetActive(false);

        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))
                languagesDropdown.value = languagesDropdown.value == 0 ? 1 : 0;
        }

        private void Start()
        {
            languagesDropdown.ClearOptions();
            List<string> languages = LanguageManager.Instance.GetLanguagesNames().ToList();
            languagesDropdown.AddOptions(languages);
            languagesDropdown.value = LanguageManager.Instance.SelectedLanguage;
        }

        public void OnSettingsButtonClick()
        {
            settingsMenu.SetActive(true);
        }

        public void OnCloseSettingsMenuButtonClick()
        {
            settingsMenu.SetActive(false);
        }

        public void OnDropdownValueChange(int value)
        {
            LanguageManager.Instance.SetLanguage(value);
        }

    }
}
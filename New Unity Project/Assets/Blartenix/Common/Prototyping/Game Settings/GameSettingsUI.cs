using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Blartenix.Prototyping.Common
{
    public class GameSettingsUI : MonoBehaviour
    {
        [SerializeField]
        private GameSettings gameSettings = null;
        [Header("UI Elements")]
        [SerializeField]
        private CanvasEnabler canvasEnabler = null;
        [SerializeField]
        private Dropdown languagesDropdown = null;
        [SerializeField]
        private Dropdown graphicsDropdown = null;
        [SerializeField]
        private Dropdown resolutionsDropdown = null;
        [SerializeField]
        private Toggle fullscreenToggle = null;
        [SerializeField]
        private Slider musicVolumeSlider = null;
        [SerializeField]
        private Slider sfxVolumeSlider = null;


        private IList<Resolution> resolutions;


        internal bool IsOpen => canvasEnabler.enabled;


        private void Awake()
        {
            Init();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.O))
                canvasEnabler.SwitchEnabled();
        }

        private void Init()
        {
            //Setup language dropdown
            SetupLanguagesDropdown();

            //Setup graphics dropdown
            SetupGraphicsDropdown();

            //Setup resolutions dropdown
            SetupResolutionsDropdown();

            //Setup fullscreen toggle
            fullscreenToggle.isOn = gameSettings.Fullscreen;

            //Setup music volume slider
            musicVolumeSlider.minValue = GameSettings.MIN_VOLUME_VALUE;
            musicVolumeSlider.maxValue = GameSettings.MAX_VOLUME_VALUE;
            musicVolumeSlider.value = gameSettings.MusicVolume;

            //Setup sfx volume slider
            sfxVolumeSlider.minValue = GameSettings.MIN_VOLUME_VALUE;
            sfxVolumeSlider.maxValue = GameSettings.MAX_VOLUME_VALUE;
            sfxVolumeSlider.value = gameSettings.SfxVolume;
        }


        private void SetupLanguagesDropdown()
        {
            languagesDropdown.ClearOptions();
            languagesDropdown.interactable = LanguageManager.Instance != null;

            if(LanguageManager.Instance != null)
            {
                List<string> langNames = LanguageManager.Instance.GetLanguagesNames().ToList();

                languagesDropdown.AddOptions(langNames);
                LanguageManager.Instance.SetLanguage(gameSettings.Language);
                languagesDropdown.value = gameSettings.Language;
            }
        }


        private void SetupGraphicsDropdown()
        {
            graphicsDropdown.ClearOptions();

            graphicsDropdown.AddOptions(QualitySettings.names.Reverse().ToList());
            graphicsDropdown.value = gameSettings.Graphics;
        }

        private void SetupResolutionsDropdown()
        {
            resolutionsDropdown.ClearOptions();
            
            resolutions = Screen.resolutions.Reverse().Select(r => new Resolution { width = r.width, height = r.height }).Distinct().ToList();

            List<string> options = resolutions.Select(r => $"{r.width} x {r.height}").ToList();
            resolutionsDropdown.AddOptions(options);

            gameSettings.Resolution = Mathf.Clamp(gameSettings.Resolution, 0, resolutions.Count - 1);
            resolutionsDropdown.value = gameSettings.Resolution;
            
            Screen.SetResolution(resolutions[gameSettings.Resolution].width, resolutions[gameSettings.Resolution].height, gameSettings.Fullscreen);
        }


        public void Open()
        {
            canvasEnabler.enabled = true;
        }


        public void Close()
        {
            canvasEnabler.enabled = false;
        }


        public void OnLanguageDropdownChange(int languageIndex)
        {
            LanguageManager.Instance.SetLanguage(languageIndex);
            gameSettings.Language = languageIndex;
        }

        public void OnGraphicsDropdownChange(int graphicIndex)
        {
            gameSettings.Graphics = graphicIndex;
        }

        public void OnResolutionDropdownChange(int resolutionIndex)
        {
            gameSettings.Resolution =  resolutionIndex;
            Screen.SetResolution(resolutions[gameSettings.Resolution].width, resolutions[gameSettings.Resolution].height, gameSettings.Fullscreen);
        }

        public void OnFullscreenToggleChange(bool isOn)
        {
            gameSettings.Fullscreen = isOn;
            SetupResolutionsDropdown();
        }

        public void OnMusicSliderChange(float volume)
        {
            gameSettings.MusicVolume = volume;
        }

        public void OnSFXSliderChange(float volume)
        {
            gameSettings.SfxVolume = volume;
        }

        public void OnResetDefaultValues()
        {
            gameSettings.ResetValues();
            Init();
        }
    }
}
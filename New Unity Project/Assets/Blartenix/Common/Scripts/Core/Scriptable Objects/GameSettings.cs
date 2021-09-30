using UnityEngine;
using UnityEngine.Audio;

namespace Blartenix
{
    [CreateAssetMenu(fileName = "New Game Settings", menuName = "Blartenix/Game Settings")]
    public class GameSettings : ScriptableObject
    {
        public const float MIN_VOLUME_VALUE = 0.0001f;
        public const float MAX_VOLUME_VALUE = 1;

        private const string LANGUAGE_KEY = "language";
        private const string DEFAULT_GRAPHICS_KEY = "default_graphics";
        private const string GRAPHICS_KEY = "graphics";
        private const string RESOLUTION_KEY = "resolution";
        private const string DEFAULT_FULLSCREEN_KEY = "default_fullscreen";
        private const string FULLSCREEN_KEY = "fullscreen";

        [Header("Audio Settings")]
        [SerializeField]
        private AudioMixer gameAudioMixer = null;
        [Tooltip("Name of the volume param exposed in the audio mixer for the music")]
        [SerializeField]
        private string musicVolumeParamName = "music_volume";
        [Tooltip("Name of the volume param exposed in the audio mixer for the sfx")]
        [SerializeField]
        private string sfxVolumeParamName = "sfx_volume";
        [Range(MIN_VOLUME_VALUE, MAX_VOLUME_VALUE)]
        [SerializeField]
        private float defaultMusicVolume = 0.7f;
        [Range(MIN_VOLUME_VALUE, MAX_VOLUME_VALUE)]
        [SerializeField]
        private float defaultSFXVolume = 0.7f;



        internal int Language
        {
            get
            {
                if (!PlayerPrefs.HasKey(LANGUAGE_KEY))
                {
                    PlayerPrefs.SetInt(LANGUAGE_KEY, 0);
                    PlayerPrefs.Save();
                }

                return PlayerPrefs.GetInt(LANGUAGE_KEY);
            }
            set
            {
                if (Language == value) return;

                PlayerPrefs.SetInt(LANGUAGE_KEY, value);
                PlayerPrefs.Save();
            }
        }


        internal int Graphics
        {
            get
            {
                if (!PlayerPrefs.HasKey(DEFAULT_GRAPHICS_KEY))
                    DefaultGraphics = QualitySettings.names.Length - 1 - QualitySettings.GetQualityLevel();

                if(!PlayerPrefs.HasKey(GRAPHICS_KEY))
                {
                    PlayerPrefs.SetInt(GRAPHICS_KEY, DefaultGraphics);
                    PlayerPrefs.Save();
                }

                return PlayerPrefs.GetInt(GRAPHICS_KEY);
            }
            set
            {
                if (Graphics == value) return;

                PlayerPrefs.SetInt(GRAPHICS_KEY, value);
                PlayerPrefs.Save();
                QualitySettings.SetQualityLevel(QualitySettings.names.Length - 1 - value);
            }
        }

        private int DefaultGraphics
        {
            get
            {
                //must be setup before
                return PlayerPrefs.GetInt(DEFAULT_GRAPHICS_KEY);
            }
            set
            {
                if (PlayerPrefs.HasKey(DEFAULT_GRAPHICS_KEY)) return;

                PlayerPrefs.SetInt(DEFAULT_GRAPHICS_KEY, value);
                PlayerPrefs.Save();
            }
        }

        internal int Resolution
        {
            get
            {
                if (!PlayerPrefs.HasKey(RESOLUTION_KEY))
                {
                    PlayerPrefs.SetInt(RESOLUTION_KEY, 0);
                    PlayerPrefs.Save();
                }

                return PlayerPrefs.GetInt(RESOLUTION_KEY);
            }
            set
            {
                if (Resolution == value) return;

                PlayerPrefs.SetInt(RESOLUTION_KEY, value);
                PlayerPrefs.Save();
            }
        }


        internal bool Fullscreen
        {
            get
            {
                if (!PlayerPrefs.HasKey(DEFAULT_FULLSCREEN_KEY))
                    DefaultFullscreen = Screen.fullScreen;
                
                if (!PlayerPrefs.HasKey(FULLSCREEN_KEY))
                {
                    PlayerPrefs.SetInt(FULLSCREEN_KEY, DefaultFullscreen ? 1 : 0);
                    PlayerPrefs.Save();
                }

                return PlayerPrefs.GetInt(FULLSCREEN_KEY) == 1 ? true : false;
            }
            set
            {
                if (Screen.fullScreen == value) return;

                PlayerPrefs.SetInt(FULLSCREEN_KEY, value ? 1 : 0);
                PlayerPrefs.Save();
                Screen.fullScreen = value;
            }
        }

        private bool DefaultFullscreen
        {
            get
            {
                //Must be seted up before
                return PlayerPrefs.GetInt(DEFAULT_FULLSCREEN_KEY) == 1 ? true : false;
            }
            set
            {
                //We can only set it once
                if (PlayerPrefs.HasKey(DEFAULT_FULLSCREEN_KEY)) return;

                PlayerPrefs.SetInt(DEFAULT_FULLSCREEN_KEY, value ? 1 : 0);
                PlayerPrefs.Save();
            }
        }

        internal float MusicVolume
        {
            get
            {
                if (!PlayerPrefs.HasKey(musicVolumeParamName))
                    SetVolume(musicVolumeParamName, defaultMusicVolume);

                return PlayerPrefs.GetFloat(musicVolumeParamName);
            }
            set
            {   
                SetVolume(musicVolumeParamName, Mathf.Clamp(value, MIN_VOLUME_VALUE, MAX_VOLUME_VALUE));
            }
        }

        internal float SfxVolume
        {
            get
            {
                if (!PlayerPrefs.HasKey(sfxVolumeParamName))
                    SetVolume(sfxVolumeParamName, defaultSFXVolume);

                return PlayerPrefs.GetFloat(sfxVolumeParamName);
            }
            set
            {
                SetVolume(sfxVolumeParamName, Mathf.Clamp(value, MIN_VOLUME_VALUE, MAX_VOLUME_VALUE));
            }
        }



        internal void ResetValues()
        {
            PlayerPrefs.DeleteAll();
        }


        private void SetVolume(string volumePrefKey, float volume)
        {
            PlayerPrefs.SetFloat(volumePrefKey, volume);
            gameAudioMixer.SetFloat(volumePrefKey, Mathf.Log10(volume) * 20);
            PlayerPrefs.Save();
        }
    }
}
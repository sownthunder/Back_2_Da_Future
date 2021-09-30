using UnityEngine;
using Blartenix.Prototyping.Common;


namespace Blartenix.Demos.Common
{
    public class bxCommonGameManager : MonoBehaviour
    {
        [SerializeField]
        private GameSettingsUI gameSettingsUi = null;


        private void Awake()
        {
            BlartenixLogger.GlobalInstance = new bxMyLoggerClass();
        }

        private void Start()
        {
            BlartenixLogger.GlobalInstance.DisplayMessage("Hello from Blartenix Logger Global Instance");
        }

        public void OpenSettingsMenu()
        {
            if (gameSettingsUi.IsOpen) return;

            gameSettingsUi.Open();
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
        }
    }
}
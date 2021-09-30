using UnityEngine;

namespace Blartenix
{
    public class CanvasEnabler : MonoBehaviour
    {
        [SerializeField]
        private bool defaultState = false;
        [SerializeField]
        private Canvas canvas = null;
        [SerializeField]
        private Behaviour[] enablingElements = null;

        internal new bool enabled
        {
            get => canvas.enabled;
            set
            {
                if (enabled == value) return;

                canvas.enabled = value;
                for (int i = 0; i < enablingElements.Length; i++)
                {
                    enablingElements[i].enabled = value;
                }
            }
        }

        private void Awake()
        {
            if (canvas == null)
                canvas = GetComponent<Canvas>();

            enabled = defaultState;
        }

        [ContextMenu("Switch Enabled")]
        public void SwitchEnabled()
        {
            enabled = !enabled;
        }
    }
}
using System.Linq;
using UnityEngine;

namespace Blartenix
{
    /// <summary>
    /// Use the TriggerPictogram2D componenet for showing a pictogram when the OnTriggerEnter2D
    /// is called on the object or when you want to.
    /// </summary>
    public class TriggerPictogram2D : MonoBehaviour
    {
        [SerializeField]
        private CanvasEnabler[] pictograms = null;
        [Min(0)]
        [SerializeField]
        private int defaultPictogram = 0;
        [SerializeField]
        private string[] triggeringTags = null;
        [SerializeField]
        private LookForTagAt lookForTagAt = LookForTagAt.TriggeringCollider;

        private GameObject observed;
        private int currentPictogram;

        public bool IsShowing { get; private set; }

        private void Awake()
        {
            currentPictogram = defaultPictogram;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (observed) return;

            GameObject entered = null;

            switch (lookForTagAt)
            {
                case LookForTagAt.TriggeringCollider:
                    entered = collision.gameObject;
                    break;
                case LookForTagAt.ColliderParent:
                    entered = collision.transform.parent.gameObject;
                    break;
                case LookForTagAt.ColliderRoot:
                    entered = collision.transform.root.gameObject;
                    break;
            }

            if (!triggeringTags.Contains(entered.tag)) return;

            observed = entered;

            ShowPictogram();
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (!observed) return;

            GameObject entered = null;

            switch (lookForTagAt)
            {
                case LookForTagAt.TriggeringCollider:
                    entered = collision.gameObject;
                    break;
                case LookForTagAt.ColliderParent:
                    entered = collision.transform.parent.gameObject;
                    break;
                case LookForTagAt.ColliderRoot:
                    entered = collision.transform.root.gameObject;
                    break;
            }

            if (!triggeringTags.Contains(entered.tag)) return;

            if (entered != observed) return;

            observed = null;

            HidePictogram();
        }


        internal void HidePictogram()
        {
            if (!pictograms[currentPictogram].enabled) return;

            pictograms[currentPictogram].enabled = false;
            currentPictogram = defaultPictogram;
            IsShowing = false;
        }


        internal void ShowPictogram()
        {
            pictograms[currentPictogram].enabled = true;
            IsShowing = true;
        }


        internal void ShowPictogram(int pictogramIndex)
        {
            if (pictograms[currentPictogram].enabled)
                HidePictogram();

            currentPictogram = pictogramIndex;
            ShowPictogram();
        }


        internal void ShowPictogram(int pictogramIndex, float hideTime)
        {
            if (pictograms[currentPictogram].enabled)
                HidePictogram();

            currentPictogram = pictogramIndex;
            ShowPictogram();

            Invoke(nameof(HidePictogram), hideTime);
        }

        internal void SetDefaultPictogram(int pictogramIndex)
        {
            defaultPictogram = pictogramIndex;
        }
    }
}
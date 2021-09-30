using System.Linq;
using UnityEngine;

namespace Blartenix
{
    public class TriggerPictogram : MonoBehaviour
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
        [SerializeField]
        private bool lookAtCamera = true;
        [SerializeField]
        private bool lookAtMainCamera = true;
        [SerializeField]
        private Camera targetCam = null;

        private Collider observed;
        private int currentPictogram;

        public bool IsShowing { get; private set; }


        private void Awake()
        {
            if(lookAtCamera && lookAtMainCamera)
                targetCam = Camera.main;

            if (lookAtCamera)
            {
                for (int i = 0; i < pictograms.Length; i++)
                {
                    Vector3 scale = pictograms[i].transform.localScale;
                    scale.x *= -1;
                    pictograms[i].transform.localScale = scale;
                }
            }

            currentPictogram = defaultPictogram;
        }


        private void Update()
        {
            if (!IsShowing) return;

            if (observed == null || !observed.enabled || !observed.gameObject.activeInHierarchy)
            {
                if (observed != null)
                    observed = null;

                HidePictogram();
                return;
            }

            if (!lookAtCamera) return;

            if (!pictograms[currentPictogram].enabled) return;

            pictograms[currentPictogram].transform.LookAt(targetCam.transform);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (observed) return;

            GameObject entered = null;

            switch (lookForTagAt)
            {
                case LookForTagAt.TriggeringCollider:
                    entered = other.gameObject;
                    break;
                case LookForTagAt.ColliderParent:
                    entered = other.transform.parent.gameObject;
                    break;
                case LookForTagAt.ColliderRoot:
                    entered = other.transform.root.gameObject;
                    break;
            }

            if (!triggeringTags.Contains(entered.tag)) return;

            observed = other;

            ShowPictogram();
        }

        private void OnTriggerExit(Collider other)
        {
            if (!observed) return;

            GameObject entered = null;

            switch (lookForTagAt)
            {
                case LookForTagAt.TriggeringCollider:
                    entered = other.gameObject;
                    break;
                case LookForTagAt.ColliderParent:
                    entered = other.transform.parent.gameObject;
                    break;
                case LookForTagAt.ColliderRoot:
                    entered = other.transform.root.gameObject;
                    break;
            }

            if (!triggeringTags.Contains(entered.tag)) return;

            if (other != observed) return;

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
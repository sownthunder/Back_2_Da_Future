using UnityEngine;

namespace Blartenix
{
    public class Billboard : MonoBehaviour
    {
        [SerializeField]
        private bool lookAtMainCamera = true;
        [SerializeField]
        private Transform lookAtTarget = null;
        [SerializeField]
        private Vector3 lookPositionOffset = Vector3.zero;
        [SerializeField]
        private bool invertXScale = false;
        [Range(0.1f, 1)]
        [SerializeField]
        private float smooth = 0.5f;
        [SerializeField]
        private bool rotateX = false;


        private Vector3 targetPoint = Vector3.zero;

        private void Awake()
        {
            if (invertXScale)
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

            if (lookAtMainCamera)
                lookAtTarget = Camera.main.transform;
        }

        private void Start()
        {
            SetTarget(lookAtTarget);
        }

        private void Update()
        {
            if(lookAtTarget == null)
            {
                Debug.LogError("There is no look at target");
                return;
            }

            targetPoint.Set(lookAtTarget.position.x + lookPositionOffset.x, rotateX ? lookAtTarget.position.y + lookPositionOffset.y : transform.position.y, lookAtTarget.position.z + lookPositionOffset.z);
            Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, smooth);
        }

        /// <summary>
        /// Sets the target to look at
        /// </summary>
        /// <param name="target"></param>
        internal void SetTarget(Transform target)
        {
            lookAtTarget = target;
        }
    }
}
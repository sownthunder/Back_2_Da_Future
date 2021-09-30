using UnityEngine;

namespace Blartenix.Demos
{
    public class bxCameraFollow : MonoBehaviour
    {
        [SerializeField]
        private string playerTag = "Player";
        [SerializeField]
        private float followSpeed = 5;

        private Transform playerTarget = null;
        private Vector3 targetPos = Vector3.zero;

        private void Start()
        {
            playerTarget = GameObject.FindGameObjectWithTag(playerTag)?.transform;
        }

        private void LateUpdate()
        {
            if (playerTarget)
            {
                targetPos.Set(playerTarget.position.x, transform.position.y, transform.position.z);
                transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * followSpeed);
            }

        }

        public void SetTarget(Transform target)
        {
            playerTarget = target;
        }
    }
}
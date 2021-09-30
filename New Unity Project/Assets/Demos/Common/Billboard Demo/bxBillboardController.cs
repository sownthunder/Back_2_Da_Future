using UnityEngine;

namespace Blartenix.Demos.Common
{
    public class bxBillboardController : MonoBehaviour
    {
        [SerializeField]
        private Transform camTransform = null;
        [SerializeField]
        private Transform targetTransform = null;

        private float input = 0;
        private void Update()
        {
            input = Input.GetAxis("Horizontal");

            camTransform.RotateAround(targetTransform.position, Vector3.up * -input, 90 * Time.deltaTime);
        }
    }
}
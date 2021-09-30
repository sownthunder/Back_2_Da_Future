using UnityEngine;

namespace Blartenix.Demos.Common
{
    public class bxShape : MonoBehaviour
    {
        [SerializeField]
        private string shapeName = "Shape Name";

        public string ShapeName => shapeName;
    }
}
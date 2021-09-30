using System.Collections;
using UnityEngine;

namespace Blartenix.Demos.Common
{
    public class bxTransformObjectPoolDemo : MonoBehaviour
    {
        [SerializeField]
        private TransformPool pool = null;

        private void Awake()
        {
            if (pool == null)
                pool = GetComponent<TransformPool>();

            pool.InitPool();
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Q))
            {
                Transform t = pool.Get(new Vector3(0, 0.3f, 0), Quaternion.identity, null);
                StartCoroutine(IAutoRemove(t));
            }
        }

        private IEnumerator IAutoRemove(Transform t)
        {
            yield return new WaitForSeconds(2);

            pool.Put(t);
        }
    }
}
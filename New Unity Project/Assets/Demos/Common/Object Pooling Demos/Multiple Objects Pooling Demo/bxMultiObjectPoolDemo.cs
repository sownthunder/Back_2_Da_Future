using System.Collections;
using UnityEngine;

namespace Blartenix.Demos.Common
{
    public class bxMultiObjectPoolDemo : MonoBehaviour
    {
        [SerializeField]
        private bxShapesPool pool = null;

        private void Awake()
        {
            if (pool == null)
                pool = GetComponent<bxShapesPool>();

            pool.InitPool();
        }


        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                bxShape shape = pool.Get(s => s.ShapeName == "Cube", new Vector3(0, 0.3f, 0), Quaternion.identity, null);
                print(shape.name);
                if (shape != null)
                    StartCoroutine(IAutoRemove(shape));
                else
                    Debug.LogError("Could not find a shape with name 'Cube'");
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                bxShape shape = pool.Get(GetCapsule, new Vector3(0, 0.3f, 0), Quaternion.identity, null);

                if (shape != null)
                    StartCoroutine(IAutoRemove(shape));
                else
                    Debug.LogError("Could not find a capsule");
            }
        }

        //Anothe way for creating a predicate
        private bool GetCapsule(bxShape shape)
        {
            return shape.ShapeName == "Capsule";
        }


        private IEnumerator IAutoRemove(bxShape shape)
        {
            yield return new WaitForSeconds(2);

            pool.Put(shape);
        }
    }
}
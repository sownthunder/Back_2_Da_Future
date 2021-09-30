using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Blartenix
{
    public abstract class ObjectPool<T> : MonoBehaviour where T : Component
    {
        [SerializeField]
        private T prefab = default;
        [SerializeField]
        [Min(0)]
        private int initialCount = 1;
        [SerializeField]
        private T[] preloaded = null;

        private List<T> inactives;
        private List<T> actives;
        private bool poolInitialized = false;

        private int Total => inactives.Count + actives.Count;



        internal void InitPool()
        {
            actives = new List<T>();
            inactives = new List<T>();

            if(preloaded != null)
            {
                for (int i = 0; i < preloaded.Length; i++)
                {
                    if (preloaded[i].gameObject.activeInHierarchy)
                        actives.Add(preloaded[i]);
                    else
                    {
                        preloaded[i].transform.SetParent(transform);
                        inactives.Add(preloaded[i]);
                    }
                }
            }

            while (Total < initialCount)
            {
                T go = CreateNewInstance(false);
                go.transform.SetParent(transform);
                inactives.Add(go);
            }

            poolInitialized = true;
        }

        private T CreateNewInstance(bool active)
        {
            T go = Instantiate(prefab);

            go.name = go.name.Replace("(Clone)", string.Empty).Replace("Prefab", string.Empty);
            
            go.gameObject.SetActive(active);

            return go;
        }

        internal T Get(bool active = true)
        {
            if(!poolInitialized)
            {
                Debug.LogWarning($"Object pool '{name}' should be initialized on Awake method");
                InitPool();
            }

            T go;
            if (inactives.Count > 0)
            {
                go = inactives.First();
                go.gameObject.SetActive(active);

                inactives.Remove(go);
            }
            else
            {
                go = CreateNewInstance(active);
            }

            actives.Add(go);

            return go;
        }

        internal T Get(Transform parent, bool active = true)
        {
            T go = Get(false);
            
            go.transform.SetParent(parent);

            go.gameObject.SetActive(active);

            return go;
        }

        internal T Get(Vector3 position, Quaternion rotation, bool active = true)
        {
            T go = Get(false);
            go.transform.position = position;
            go.transform.rotation = rotation;

            go.gameObject.SetActive(active);

            return go;
        }

        internal T Get(Vector3 position, Quaternion rotation, Transform parent, bool active = true)
        {
            T go = Get(position, rotation, false);
            go.transform.SetParent(parent);

            go.gameObject.SetActive(active);

            return go;
        }


        internal void Put(T go)
        {
            go.transform.SetParent(transform);

            go.gameObject.SetActive(false);

            if (actives.Contains(go))
                actives.Remove(go);

            inactives.Add(go);
        }

    }
}
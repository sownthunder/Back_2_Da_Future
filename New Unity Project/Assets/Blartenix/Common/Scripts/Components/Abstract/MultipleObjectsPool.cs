using System;
using System.Collections.Generic;
using UnityEngine;

namespace Blartenix
{
    public abstract class MultipleObjectsPool<T> : MonoBehaviour where T : Component
    {
        [SerializeField]
        private List<T> prefabs = default;
        [SerializeField]
        private T[] preloaded = null;


        private List<T> inactives;
        private List<T> actives;
        private bool poolInitialized = false;


        internal void InitPool()
        {
            actives = new List<T>();
            inactives = new List<T>();

            if (preloaded != null)
            {
                for (int i = 0; i < preloaded.Length; i++)
                {
                    if (preloaded[i].gameObject.activeInHierarchy)
                        actives.Add(preloaded[i]);
                    else
                        inactives.Add(preloaded[i]);
                }
            }

            poolInitialized = true;
        }


        private T CreateNewInstance(T prefab, bool active = true)
        {
            T go = Instantiate(prefab);

            go.name = go.name.Replace("(Clone)", string.Empty).Replace("Prefab", string.Empty);

            go.gameObject.SetActive(active);

            return go;
        }

        internal T Get(Predicate<T> predicate, bool active = true)
        {
            if (!poolInitialized)
            {
                Debug.LogWarning($"Object pool '{name}' should be initialized on Awake method");
                InitPool();
            }

            T go = default;
            if (inactives.Count > 0)
            {
                go = inactives.Find(predicate);

                if (go != null)
                {
                    go.gameObject.SetActive(active);
                    inactives.Remove(go);

                    actives.Add(go);
                    go.transform.SetParent(null);
                }
                else
                {
                    T prefab = prefabs.Find(predicate);

                    if (prefab != null)
                    {
                        go = CreateNewInstance(prefab);
                        actives.Add(go);
                    }
                    else
                        Debug.LogWarning("Couldn't find specified object in list nor a prefab for creating a new instance");
                }
            }
            else
            {
                T prefab = prefabs.Find(predicate);

                if (prefab != null)
                {
                    go = CreateNewInstance(prefab);
                    actives.Add(go);
                }
                else
                    Debug.LogWarning("Couldn't find specified object in list nor a prefab for creating a new instance");
            }

            return go;
        }

        internal T Get(Predicate<T> predicate, Transform parent, bool active = true)
        {
            T go = Get(predicate, false);

            if (go != null)
            {
                go.transform.SetParent(parent);
                go.gameObject.SetActive(active);
            }

            return go;
        }

        internal T Get(Predicate<T> predicate, Vector3 position, Quaternion rotation, bool active = true)
        {
            T go = Get(predicate, false);

            if (go != null)
            {
                go.transform.position = position;
                go.transform.rotation = rotation;

                go.gameObject.SetActive(active);
            }

            return go;
        }

        internal T Get(Predicate<T> predicate, Vector3 position, Quaternion rotation, Transform parent, bool active = true)
        {
            T go = Get(predicate, position, rotation, false);

            if (go != null)
            {
                go.transform.SetParent(parent);
                go.gameObject.SetActive(active);
            }

            return go;
        }

        internal void Put(T go)
        {
            go.transform.SetParent(transform, transform);

            go.gameObject.SetActive(false);

            if (actives.Contains(go))
                actives.Remove(go);

            inactives.Add(go);
        }
    }
}
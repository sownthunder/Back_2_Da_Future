using UnityEngine;

namespace Blartenix
{
    public abstract class SingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        [SerializeField]
        private bool dontDestroyOnLoad = false;


        private static T instance;
        private static object m_Lock = new object();


        internal static T Instance
        {
            get
            {
                lock (m_Lock)
                {
                    if (instance == null)
                    {
                        // Search for existing instance if exist.
                        instance = (T)FindObjectOfType(typeof(T));
                    }

                    return instance;
                }
            }
        }

        /// <summary>
        /// Replaces builtin Awake method if needed.
        /// </summary>
        protected virtual void OnAwake() { }
        /// <summary>
        /// Replaces builtin OnDestroy method if needed.
        /// </summary>
        protected virtual void OnDestroyed() { }

        private void Awake()
        {
            if (instance == null)
            {
                instance = GetComponent<T>();

                if (dontDestroyOnLoad)
                    DontDestroyOnLoad(gameObject);
            }
            else if(instance != this)
                Destroy(gameObject);

            OnAwake();
        }

        private void OnDestroy()
        {
            OnDestroyed();

            if (instance == this)
                instance = null;

        }
    }
}
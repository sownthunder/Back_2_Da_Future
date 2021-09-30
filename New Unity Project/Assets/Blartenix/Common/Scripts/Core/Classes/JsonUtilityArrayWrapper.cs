using System;
using UnityEngine;

namespace Blartenix
{
    /// <summary>
    /// A helper class for JsonUtility serialization of array as the root object.
    /// </summary>
    /// <typeparam name="T">Array Type</typeparam>
    [Serializable]
    public class JsonUtilityArrayWrapper<T>
    {
        [SerializeField]
        private T[] array;

        public T[] Array => array;

        public JsonUtilityArrayWrapper(T[] array)
        {
            this.array = array;
        }
    }
}
using System;
using UnityEngine;

namespace Blartenix
{
    /// <summary>
    /// A helper class for JsonUtility serialization of primitive as root object
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class JsonUtilityPrimitiveWrapper<T>
    {
        [SerializeField]
        private T primitive;

        public T Primitive => primitive;

        public JsonUtilityPrimitiveWrapper(T primitive)
        {
            this.primitive = primitive;
        }
    }
}
using System;

namespace abcCrea.PersistentData
{
    /// <summary>
    /// Serializable key-value pair used by SerializableDictionary.
    /// </summary>
    [Serializable]
    public class SerializableKeyValuePair<TKey, TValue>
    {
        public TKey Key;
        public TValue Value;
    }
}

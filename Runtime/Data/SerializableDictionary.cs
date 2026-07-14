using System;
using System.Collections.Generic;

namespace abcCrea.PersistentData
{
    /// <summary>
    /// Serializable dictionary wrapper used to store dictionary entries with Unity JsonUtility.
    /// Converts a dictionary into a serializable list and restores it after loading.
    /// </summary>
    [Serializable]
    public class SerializableDictionary<TKey, TValue>
    {
        public List<SerializableKeyValuePair<TKey, TValue>> Entries = new();

        public SerializableDictionary() { }

        public SerializableDictionary(Dictionary<TKey, TValue> dictionary)
        {
            foreach (var pair in dictionary)
            {
                Entries.Add(new SerializableKeyValuePair<TKey, TValue>
                {
                    Key = pair.Key,
                    Value = pair.Value
                });
            }
        }

        /// <summary>
        /// Converts the serialized entry list back into a standard dictionary.
        /// </summary>
        public Dictionary<TKey, TValue> ToDictionary()
        {
            Dictionary<TKey, TValue> dictionay = new();

            foreach (var entry in Entries)
            {
                dictionay[entry.Key] = entry.Value;
            }

            return dictionay;
        }
    }
}

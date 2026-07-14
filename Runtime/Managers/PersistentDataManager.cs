using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace abcCrea.PersistentData
{
    /// <summary>
    /// Handles persistent local data storage for Unity projects.
    /// Uses PlayerPrefs for primitive values and JSON files for classes and dictionaries.
    /// Also provides methods to load, save, modify, and delete locally stored data.
    /// </summary>
    public class PersistentDataManager : MonoBehaviour
    {
        public event Action OnDataSaved;
        public event Action OnDataLoaded;
        public static event Action OnAllDataDeleted;

        [SerializeField] private int _currentSavedVersion = 1;

        #region Debug

#if UNITY_EDITOR

        [Header("Debug - Primitive Test")]
        [SerializeField] private string _debugKey = "Test_Int";
        [SerializeField] private int _debugIntValue = 0;

        /// <summary>
        /// Saves the current debug integer value using the configured debug key.
        /// </summary>
        public void DebugSaveInt()
        {
            SaveInt(_debugKey, _debugIntValue);
            PlayerPrefs.Save();

            Debug.Log($"[DEBUG SAVE] Key: {_debugKey} | Value: {_debugIntValue}");
        }

        /// <summary>
        /// Loads and logs the integer stored with the configured debug key.
        /// </summary>
        public void DebugLoadInt()
        {
            int value = LoadInt(_debugKey, -999);
            Debug.Log($"[DEBUG LOAD] Key: {_debugKey} | Value: {value}");
        }

        /// <summary>
        /// Deletes the primitive value associated with the configured debug key.
        /// </summary>
        public void DebugDeleteKey()
        {
            DeletePrimitive(_debugKey);
            Debug.Log($"[DEBUG DELETE] Key: {_debugKey}");
        }

        /// <summary>
        /// Deletes every PlayerPrefs value and JSON save file managed by this system.
        /// </summary>
        public void DebugDeleteAll() => DeleteAllData();

#endif
        #endregion

        /// <summary>
        /// Saves the configured save-data version in PlayerPrefs.
        /// </summary>
        public void SaveVersion()
        {
            PlayerPrefs.SetInt("SaveVersion", _currentSavedVersion);
            PlayerPrefs.Save();

            Debug.Log($"[PersistentDataManager] Saved Version: {_currentSavedVersion}");
            OnDataSaved?.Invoke();
        }

        /// <summary>
        /// Loads the saved data version. Returns zero when no version has been saved.
        /// </summary>
        public int LoadVersion()
        {
            int version = PlayerPrefs.GetInt("SaveVersion", 0);
            Debug.Log($"[PersistentDataManager] Loaded Version: {version}");

            OnDataLoaded?.Invoke();
            return version;
        }

        /// <summary>
        /// Deletes all PlayerPrefs data and all top-level JSON files in the persistent data directory.
        /// </summary>
        public void DeleteAllData()
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
            DeleteAllJsonFiles();
            Debug.Log("[DEBUG] All PlayerPrefs and JSON save files deleted");
            OnAllDataDeleted?.Invoke();
        }

        /// <summary>
        /// Deletes all JSON files stored directly inside Application.persistentDataPath.
        /// </summary>
        private static void DeleteAllJsonFiles()
        {
            string persistentDataPath = Application.persistentDataPath;
            if (!Directory.Exists(persistentDataPath)) return;
            string[] jsonFiles = Directory.GetFiles(persistentDataPath, "*.json", SearchOption.TopDirectoryOnly);
            foreach (string jsonFile in jsonFiles)
            {
                File.Delete(jsonFile);
                Debug.Log($"[DELETE] Json -> {jsonFile}");
            }
        }

        /// <summary>
        /// Serializes and saves a dictionary as a JSON file.
        /// </summary>
        public static void SaveDictionaryData<TKey, TValue>(string fileName, Dictionary<TKey, TValue> dictionary)
        {
            var wrapper = new SerializableDictionary<TKey, TValue>(dictionary);
            string json = JsonUtility.ToJson(wrapper, true);

            string path = Path.Combine(Application.persistentDataPath, fileName);
            File.WriteAllText(path, json);

            Debug.Log($"[SAVE] Dictionary -> {path}");
        }

        /// <summary>
        /// Loads a dictionary from a JSON file. Returns an empty dictionary when the file does not exist.
        /// </summary>
        public static Dictionary<TKey, TValue> LoadDictionaryData<TKey, TValue>(string fileName)
        {
            string path = Path.Combine(Application.persistentDataPath, fileName);

            if (!File.Exists(path))
            {
                Debug.LogWarning($"[LOAD] Dictionary file not found: {path}");
                return new Dictionary<TKey, TValue>();
            }

            string json = File.ReadAllText(path);
            var wrapper = JsonUtility.FromJson<SerializableDictionary<TKey, TValue>>(json);

            Debug.Log($"[LOAD] Dictionary <- {path}");
            return wrapper.ToDictionary();
        }

        /// <summary>
        /// Deletes a dictionary JSON file when it exists.
        /// </summary>
        public static void DeleteDictionaryData(string fileName)
        {
            string path = Path.Combine(Application.persistentDataPath, fileName);

            if (File.Exists(path))
            {
                File.Delete(path);
                Debug.Log($"[DELETE] Dictionary -> {path}");
            }
        }

        /// <summary>
        /// Serializes and saves a class instance as a JSON file.
        /// </summary>
        public static void SaveClassData<T>(string fileName, T data) where T : class
        {
            string json = JsonUtility.ToJson(data, true);

            string path = Path.Combine(Application.persistentDataPath, fileName);
            File.WriteAllText(path, json);

            Debug.Log($"[SAVE] Class -> {path}");
        }

        /// <summary>
        /// Loads a class instance from a JSON file. Returns a new instance when the file does not exist.
        /// </summary>
        public static T LoadClassData<T>(string fileName) where T : class, new()
        {
            string path = Path.Combine(Application.persistentDataPath, fileName);

            if (!File.Exists(path))
            {
                Debug.LogWarning($"[LOAD] Class file not found: {path}");
                return new T();
            }

            string json = File.ReadAllText(path);

            Debug.Log($"[LOAD] Class <- {path}");
            return JsonUtility.FromJson<T>(json);
        }

        /// <summary>
        /// Deletes a class JSON file when it exists.
        /// </summary>
        public static void DeleteClassData(string fileName)
        {
            string path = Path.Combine(Application.persistentDataPath, fileName);

            if (File.Exists(path))
            {
                File.Delete(path);
                Debug.Log($"[DELETE] Class -> {path}");
            }
        }

        public static void SaveInt(string key, int value) => PlayerPrefs.SetInt(key, value);
        public static int LoadInt(string key, int defaultValue = 0) => PlayerPrefs.GetInt(key, defaultValue);

        public static void SaveFloat(string key, float value) => PlayerPrefs.SetFloat(key, value);
        public static float LoadFloat(string key, float defaultValue = 0f) => PlayerPrefs.GetFloat(key, defaultValue);

        public static void SaveString(string key, string value) => PlayerPrefs.SetString(key, value);
        public static string LoadString(string key, string defaultValue = "") => PlayerPrefs.GetString(key, defaultValue);

        public static void SaveBool(string key, bool value) => PlayerPrefs.SetInt(key, value ? 1 : 0);
        public static bool LoadBool(string key, bool defaultValue = false) => PlayerPrefs.GetInt(key, defaultValue ? 1 : 0) == 1;

        /// <summary>
        /// Saves a Vector3 as three PlayerPrefs float values.
        /// </summary>
        public static void SaveVector3(string key, Vector3 value)
        {
            PlayerPrefs.SetFloat(key + "_X", value.x);
            PlayerPrefs.SetFloat(key + "_Y", value.y);
            PlayerPrefs.SetFloat(key + "_Z", value.z);
        }

        /// <summary>
        /// Loads a Vector3 from three PlayerPrefs float values.
        /// </summary>
        public static Vector3 LoadVector3(string key, Vector3 defaultValue = default)
        {
            float x = PlayerPrefs.GetFloat(key + "_X", defaultValue.x);
            float y = PlayerPrefs.GetFloat(key + "_Y", defaultValue.y);
            float z = PlayerPrefs.GetFloat(key + "_Z", defaultValue.z);

            return new Vector3(x, y, z);
        }

        /// <summary>
        /// Deletes a primitive PlayerPrefs key and its optional Vector3 component keys.
        /// </summary>
        public static void DeletePrimitive(string key)
        {
            PlayerPrefs.DeleteKey(key);
            PlayerPrefs.DeleteKey(key + "_X");
            PlayerPrefs.DeleteKey(key + "_Y");
            PlayerPrefs.DeleteKey(key + "_Z");
        }
    }
}

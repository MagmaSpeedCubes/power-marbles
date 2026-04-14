using System.Collections.Generic;

using UnityEngine;

using MagmaLabs;
using MagmaLabs.Editor;


namespace MagmaLabs.Economy{
    public class SaveManager : MonoBehaviour
    {
        [SerializeField] protected string fileName = "saveData.json";
        [SerializeField] protected bool autoSave = true;
        [ShowIf("autoSave", true)]
        [SerializeField] protected float autoSaveInterval = 60f;

        [SerializeField] protected bool logDebugMessages = false;
        public static SaveManager instance { get; private set; }

        public SaveData saveData { get; private set; }
        
        private string filePath => System.IO.Path.Combine(Application.persistentDataPath, fileName);

        void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(this.gameObject);
                Debug.LogWarning("Multiple instances of SaveManager detected. Destroying duplicate.");
            }
            else
            {
                instance = this;
                DontDestroyOnLoad(this.gameObject);
            }

            string serialized = System.IO.File.Exists(filePath) ? System.IO.File.ReadAllText(filePath) : "{}";
            saveData = new SaveData(serialized);
            
            DebugEnhanced.LogDebugMode(System.IO.File.Exists(filePath)
            ?$"Loaded data from {filePath}: {serialized}" 
            : $"No save file found at {filePath}", logDebugMessages);

            if (autoSave)
            {
                InvokeRepeating(nameof(Save), autoSaveInterval, autoSaveInterval);
            }
        }

        public void Reset()
        {
            saveData = new SaveData("{}");
        }

        public void Save()
        {
            string serialized = saveData.Serialize();
            System.IO.File.WriteAllText(filePath, serialized);
            DebugEnhanced.LogDebugMode($"Saved data to {filePath}: {serialized}", logDebugMessages);
        }

        void OnApplicationQuit()
        {
            Save();
        }

        public void SaveString(string key, string value)
        {
            saveData.saveStrings[key] = value;
            DebugEnhanced.LogDebugMode($"Saved string: {key} = {value}", logDebugMessages);
        }

        public void SaveFloat(string key, float value)
        {
            saveData.saveFloats[key] = value;
            DebugEnhanced.LogDebugMode($"Saved float: {key} = {value}", logDebugMessages);
        }

        public void SaveInt(string key, int value)
        {
            saveData.saveInts[key] = value;
            DebugEnhanced.LogDebugMode($"Saved int: {key} = {value}", logDebugMessages);
        }  

        public void SaveBool(string key, bool value)
        {
            saveData.saveBools[key] = value;
            DebugEnhanced.LogDebugMode($"Saved bool: {key} = {value}", logDebugMessages);
        }
        public string LoadString(string key, string defaultValue = "")
        {
            DebugEnhanced.LogDebugMode(saveData.saveStrings.ContainsKey(key) ? 
            $"Loaded string: {key} = {saveData.saveStrings[key]}" 
            : $"String not found: {key}, returning default value: {defaultValue}"
            , logDebugMessages);

            return saveData.saveStrings.ContainsKey(key) ? saveData.saveStrings[key] : defaultValue;
            
        }

        public float LoadFloat(string key, float defaultValue = 0f)
        {
            DebugEnhanced.LogDebugMode(saveData.saveFloats.ContainsKey(key) ? 
            $"Loaded float: {key} = {saveData.saveFloats[key]}" 
            : $"Float not found: {key}, returning default value: {defaultValue}"
            , logDebugMessages);
            return saveData.saveFloats.ContainsKey(key) ? saveData.saveFloats[key] : defaultValue;
        }

        public int LoadInt(string key, int defaultValue = 0)
        {
            DebugEnhanced.LogDebugMode(saveData.saveInts.ContainsKey(key) ? 
            $"Loaded int: {key} = {saveData.saveInts[key]}" 
            : $"Int not found: {key}, returning default value: {defaultValue}"
            , logDebugMessages);
            return saveData.saveInts.ContainsKey(key) ? saveData.saveInts[key] : defaultValue;
        }
    
        public bool LoadBool(string key, bool defaultValue = false)
            {
                DebugEnhanced.LogDebugMode(saveData.saveBools.ContainsKey(key) ? 
                $"Loaded bool: {key} = {saveData.saveBools[key]}" 
                : $"Bool not found: {key}, returning default value: {defaultValue}"
                , logDebugMessages);
                return saveData.saveBools.ContainsKey(key) ? saveData.saveBools[key] : defaultValue;
            }
    
    }





    [System.Serializable]
    public class SaveData 
    {
        [SerializeField] private SerializableKeyValue<string>[] serializableStrings = new SerializableKeyValue<string>[0];
        [SerializeField] private SerializableKeyValue<float>[] serializableFloats = new SerializableKeyValue<float>[0];
        [SerializeField] private SerializableKeyValue<int>[] serializableInts = new SerializableKeyValue<int>[0];
        [SerializeField] private SerializableKeyValue<bool>[] serializableBools = new SerializableKeyValue<bool>[0];

        public Dictionary<string, string> saveStrings = new Dictionary<string, string>();
        public Dictionary<string, float> saveFloats = new Dictionary<string, float>();
        public Dictionary<string, int> saveInts = new Dictionary<string, int>();
        public Dictionary<string, bool> saveBools = new Dictionary<string, bool>();

        public SaveData(){}

        public SaveData(string serialized)
        {
            JsonUtility.FromJsonOverwrite(serialized, this);
            DeserializeDictionaries();
        }

        private void DeserializeDictionaries()
        {
            saveStrings.Clear();
            foreach (var item in serializableStrings)
                saveStrings[item.key] = item.value;

            saveFloats.Clear();
            foreach (var item in serializableFloats)
                saveFloats[item.key] = item.value;

            saveInts.Clear();
            foreach (var item in serializableInts)
                saveInts[item.key] = item.value;

            saveBools.Clear();
            foreach (var item in serializableBools)
                saveBools[item.key] = item.value;
        }

        private void SerializeDictionaries()
        {
            serializableStrings = new SerializableKeyValue<string>[saveStrings.Count];
            int index = 0;
            foreach (var kvp in saveStrings)
                serializableStrings[index++] = new SerializableKeyValue<string>(kvp.Key, kvp.Value);

            serializableFloats = new SerializableKeyValue<float>[saveFloats.Count];
            index = 0;
            foreach (var kvp in saveFloats)
                serializableFloats[index++] = new SerializableKeyValue<float>(kvp.Key, kvp.Value);

            serializableInts = new SerializableKeyValue<int>[saveInts.Count];
            index = 0;
            foreach (var kvp in saveInts)
                serializableInts[index++] = new SerializableKeyValue<int>(kvp.Key, kvp.Value);

            serializableBools = new SerializableKeyValue<bool>[saveBools.Count];
            index = 0;
            foreach (var kvp in saveBools)
                serializableBools[index++] = new SerializableKeyValue<bool>(kvp.Key, kvp.Value);
        }

        public string Serialize()
        {
            SerializeDictionaries();
            return JsonUtility.ToJson(this);
        }
    }

}

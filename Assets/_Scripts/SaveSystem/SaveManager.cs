using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace _Scripts
{
    public class SaveManager : Singleton<SaveManager>
    {
        private static string Path => $"{Application.persistentDataPath}/Save.json";
        
        [JsonProperty] private static Dictionary<string, Dictionary<string, SaveData>> _saveData;

        protected override void Awake()
        {
            base.Awake();
            Load();
        }

        [ContextMenu("Save")]
        public void Save()
        {
            var saveHandlers = FindObjectsByType<SaveHandler>(FindObjectsInactive.Include, FindObjectsSortMode.None);

            _saveData ??= new Dictionary<string, Dictionary<string, SaveData>>();
            
            foreach (var saveHandler in saveHandlers)
            {
                SaveState(saveHandler.Id, saveHandler.Save());
            }
            
            var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            var json = JsonConvert.SerializeObject(_saveData, Formatting.Indented, settings);
            
            File.WriteAllText(Path, json);
        }

        [ContextMenu("Load")]
        public void Load()
        {
            if (!File.Exists(Path))
            {
                _saveData ??= new Dictionary<string, Dictionary<string, SaveData>>();
                return;
            }
            
            var json = File.ReadAllText(Path);
            var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            
            _saveData = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, SaveData>>>(json, settings);
            
            var saveHandlers = FindObjectsByType<SaveHandler>(FindObjectsInactive.Include, FindObjectsSortMode.None);

            foreach (var saveHandler in saveHandlers)
            {
                if (!_saveData.TryGetValue(saveHandler.Id, out var saveData)) continue;
                
                saveHandler.Load(saveData);
            }
        }

        [ContextMenu("Delete")]
        public void DeleteSave()
        {
            if (!File.Exists(Path)) return;
            
            File.Delete(Path);
        }
        
        public void SaveState(string id, Dictionary<string, SaveData> stateInfo)
        {
            if (string.IsNullOrEmpty(id)) return;
            
            _saveData[id] = stateInfo;
        }
    }
}
using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Managers
{
    public class SaveHandler : MonoBehaviour
    {
        [field: SerializeField] public string Id { get; private set; }
        
        private ISaveableBase[] _saveables;
        
        private void Awake()
        {
            _saveables = GetComponents<ISaveableBase>();
        }

        [ContextMenu("Generate Id")]
        private void GenerateId() => Id = Guid.NewGuid().ToString();
        
        public Dictionary<string, SaveData> Save()
        {
            var saveData = new Dictionary<string, SaveData>();
            
            foreach (var saveable in _saveables)
            {
                saveData[saveable.SaveKey] = saveable.Save();
            }

            return saveData;
        }

        public void Load(Dictionary<string, SaveData> data)
        {
            foreach (var saveable in _saveables)
            {
                if (!data.TryGetValue(saveable.SaveKey, out var saveData)) continue;
                
                saveable.Load(saveData);
            }
        }
    }
}
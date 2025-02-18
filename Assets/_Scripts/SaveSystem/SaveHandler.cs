using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts
{
    public class SaveHandler : MonoBehaviour
    {
        [field: SerializeField] public string Id { get; private set; }
        
        private ISaveableBase[] _saveables;
        private ISaveableBase[] Saveables => _saveables ??= GetComponents<ISaveableBase>();

        [ContextMenu("Generate Id")]
        public void GenerateId() => Id = Guid.NewGuid().ToString();

        private void Awake()
        {
            Validate();
        }

        private void Validate()
        {
            if (string.IsNullOrWhiteSpace(Id))
            {
                Debug.LogError("Id cannot be empty");
            }
        }
        
        public Dictionary<string, SaveData> Save()
        {
            var saveData = new Dictionary<string, SaveData>();
            
            foreach (var saveable in Saveables)
            {
                saveData[saveable.SaveKey] = saveable.Save();
            }

            return saveData;
        }

        public void Load(Dictionary<string, SaveData> data)
        {
            foreach (var saveable in Saveables)
            {
                if (!data.TryGetValue(saveable.SaveKey, out var saveData)) continue;
                
                saveable.Load(saveData);
            }
        }

        private void OnDestroy()
        {
            SaveManager.Instance.SaveState(Id, Save());
        }
    }
}
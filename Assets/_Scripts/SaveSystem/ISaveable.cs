using UnityEngine;

namespace _Scripts
{
    public interface ISaveable<TSaveData> : ISaveableBase where TSaveData : SaveData
    {
        string ISaveableBase.SaveKey => GetType().Name;
        
        void ISaveableBase.Load(SaveData data)
        {
            if (data is not TSaveData concreteData3)
            {
                Debug.Log($"WRONG - expected {typeof(TSaveData)} | type: {data.GetType()}");
                return;
            }
            
            Load(concreteData3);
        }
        
        public void Load(TSaveData data);
    }

    public interface ISaveableBase
    {
        string SaveKey { get; }
        public SaveData Save();

        void Load(SaveData data);
    }
}
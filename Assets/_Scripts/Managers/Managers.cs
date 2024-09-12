using UnityEngine;

namespace _Scripts.Managers
{
    public class Managers : Singleton<Managers>
    {
        [SerializeField] private ObjectsToLoadSO objectsToLoad;

        public bool ManagersInitialized { get; private set; }

        protected override void Awake()
        {
            base.Awake();
        
            InitializeManagers();
        }
    
        private void InitializeManagers()
        {
            if(ManagersInitialized || objectsToLoad == null) return;

            if (objectsToLoad == null)
            {
                Debug.LogError($"Unassigned managers to load in {name} object!");
            }

            foreach (var manager in objectsToLoad.ManagersList)
            {
                Instantiate(manager);
            }

            ManagersInitialized = true;
        }
    }
}
using UnityEngine;

namespace _Scripts.Managers
{
    public class Managers : Singleton<Managers>
    {
        public static GameManager GameManager => GameManager.Instance;
        public static AudioManager AudioManager => AudioManager.Instance;
        public static UIManager UIManager => UIManager.Instance;
        public static ObjectPoolingManager ObjectPoolingManager => ObjectPoolingManager.Instance;
        public static InputManager InputManager => InputManager.Instance;
        public static CamerasManager CamerasManager => CamerasManager.Instance;
        public static ScenesManager ScenesManager => ScenesManager.Instance;
        public static PlayerManager PlayerManager => PlayerManager.Instance;

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
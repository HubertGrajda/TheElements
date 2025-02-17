using UnityEngine;

namespace _Scripts
{
    public static class Bootstrapper
    {
        private const string MANAGERS_PATH = "Managers";
    
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void LoadManagers()
        {
            var managersPrefab = Resources.Load<GameObject>(MANAGERS_PATH);
            var managersInstance = Object.Instantiate(managersPrefab);
            Object.DontDestroyOnLoad(managersInstance);
        }
    }
}
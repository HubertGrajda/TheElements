using UnityEngine;

namespace _Scripts
{
   public abstract class Singleton<T> : MonoBehaviour where T: MonoBehaviour
   {
      public static T Instance { get; private set; }

      protected virtual void Awake()
      {
         if (Instance != null && Instance != this)
         {
            Destroy(gameObject);
         }
         else
         {
            Instance = this as T;
            DontDestroyOnLoad(gameObject);
         }
      }
   }
}
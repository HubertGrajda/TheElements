using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace _Scripts.Pooling
{
    public class ObjectPoolingManager : Singleton<ObjectPoolingManager>
    {
        [Serializable]
        public class Pool<T> where T : IPoolable
        {
            public string Tag { get; private set; }
            public T Prefab { get; private set; }
            public GameObject Container { get; private set; }
            public List<IPoolable> Spawned { get; private set; }
        
            public Pool(string name, T type)
            {
                Tag = name;
                Prefab = type;
                Container = new GameObject($"{Tag} Pool");
                Spawned = new List<IPoolable>();
            
                Container.transform.SetParent(Instance.transform);
            }
        }
    
        private readonly Dictionary<string, Pool<IPoolable>> _poolDictionary = new();

        private T AddToPool<T>(T type) where T : MonoBehaviour, IPoolable
        {
            var pool = GetOrCreatePool(type);
            
            var instantiated = Instantiate(pool.Prefab as T, pool.Container.transform);
            
            instantiated.gameObject.SetActive(false);
            
            pool.Spawned.Add(instantiated);

            return instantiated == null ? null : instantiated;

        }
    
        public T SpawnFromPool<T>(T type, Vector3 position, Quaternion rotation) where T : MonoBehaviour, IPoolable
        {
            var pooled = GetFromPool(type);
        
            pooled.gameObject.SetActive(true);
            pooled.transform.position = position;
            pooled.transform.rotation = rotation;
            
            return pooled;
        }

        public T GetFromPool<T>(T type) where T : MonoBehaviour, IPoolable
        {
            var pool = GetOrCreatePool(type);
            T pooled;
            
            foreach (var poolable in pool.Spawned.ToList())
            {
                var objectToPool = poolable as T;
            
                if (objectToPool == null)
                {
                    pool.Spawned.Remove(objectToPool);
                    continue;
                }

                if (!objectToPool.CanBePooled) continue;
                if (objectToPool.gameObject.activeInHierarchy) continue;

                pooled = objectToPool;
                return pooled;
            }
            
            pooled = AddToPool(type);
            return pooled;
        }

        private Pool<IPoolable> GetOrCreatePool<T>(T type) where T : MonoBehaviour, IPoolable
        {
            if (!_poolDictionary.TryGetValue(type.name, out var pool))
            {
                pool = CreateNewPool(type);
            }
            
            return pool;
        }
    
        private Pool<IPoolable> CreateNewPool<T>(T type) where T : MonoBehaviour, IPoolable
        {
            var newPool = new Pool<IPoolable>(type.name, type);
            _poolDictionary.Add(type.name, newPool);
            return newPool;
        }
    }
}
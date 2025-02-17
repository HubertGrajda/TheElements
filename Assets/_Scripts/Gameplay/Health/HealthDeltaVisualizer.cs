using _Scripts.Pooling;
using UnityEngine;

namespace _Scripts
{
    public class HealthDeltaVisualizer : MonoBehaviour
    {
        [SerializeField] private HealthSystem healthSystem;
        [SerializeField] private QuickTextVisualizer quickTextPrefab;
        
        [SerializeField] private Vector2 variationX = new(-0.5f, 0.5f);
        [SerializeField] private Vector2 variationY = new(-2f, 0f);
        [SerializeField] private Vector2 variationZ = new(-1f, -1f);
        
        private ObjectPoolingManager _objectPoolingManager;

        private void Awake()
        {
            _objectPoolingManager = ObjectPoolingManager.Instance;
        }

        private void OnEnable() => AddListeners();

        private void OnDisable() => RemoveListeners();

        private void AddListeners()
        {
            healthSystem.OnDamaged += OnDamaged;
        }

        private void RemoveListeners()
        {
            healthSystem.OnDamaged -= OnDamaged;
        }
        
        private void OnDamaged(int damage)
        {
            var healthPointsVisualizer = _objectPoolingManager.GetFromPool(quickTextPrefab);
            
            healthPointsVisualizer.transform.SetParent(transform, false);
            healthPointsVisualizer.transform.localPosition = GetLocalPositionVariation();;
            healthPointsVisualizer.transform.localRotation = Quaternion.identity;
            healthPointsVisualizer.Show($"-{damage}");
        }

        private Vector3 GetLocalPositionVariation()
        {
            var x  = Random.Range(variationX.x, variationX.y);
            var y = Random.Range(variationY.x, variationY.y);
            var z = Random.Range(variationZ.x, variationZ.y);
            
            return new Vector3(x, y, z);
        }
    }
}
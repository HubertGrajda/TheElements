using System;
using UnityEngine;

namespace _Scripts
{
    public class GroundDetector : MonoBehaviour
    {
        [SerializeField] private Transform detectionCenter;
        [SerializeField] private float detectionRadius;
    
        [SerializeField] private LayerMask detectionLayer;
    
        public bool IsGrounded { get; private set; }
        public event Action<bool> OnGroundedChanged;

        private readonly Collider[] _detected = new Collider[1];

        private float _thresholdTimer;
    
        private const float THRESHOLD_TIME = 0.3f;
        private void Update()
        {
            var groundDetected = 
                Physics.OverlapSphereNonAlloc(detectionCenter.position, detectionRadius, _detected, detectionLayer) > 0;

            if (IsGrounded == groundDetected) return;
        
            _thresholdTimer += Time.deltaTime;

            if (IsGrounded && _thresholdTimer < THRESHOLD_TIME) return;
        
            IsGrounded = groundDetected;
            OnGroundedChanged?.Invoke(groundDetected);
            _thresholdTimer = 0f;
        }

        private void OnDrawGizmos()
        {
            if (detectionCenter == null) return;
        
            Gizmos.DrawSphere(detectionCenter.position, detectionRadius);
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;

public class Detector : MonoBehaviour
{
    [TagField]
    [SerializeField] private string detectionTag;
    [SerializeField] private LayerMask detectionLayer;
    
    [SerializeField] private float cooldown;
    [SerializeField] private int detectionLimit;
    [SerializeField] private float detectionRange;
    
    public bool TryGetDetected(out List<Collider> detected)
    {
        detected = default;
        var detectedColliders = new Collider[detectionLimit];
        var size = Physics.OverlapSphereNonAlloc(transform.position, detectionRange, detectedColliders, detectionLayer);

        if (size == 0) return false;
            
        detectedColliders = detectedColliders.Where(x => x.CompareTag(detectionTag)).ToArray();

        if (detectedColliders.Length == 0) return false;

        detected = detectedColliders.ToList();
        return true;
    }
    
    public bool TryGetDetected<TComponent>( out List<TComponent> detected)
    {
        detected = default;
        
        if (!TryGetDetected(out var detectedColliders)) return false;

        var detectedComponents = new List<TComponent>();
            
        foreach (var collider in detectedColliders)
        {
            if (!collider.TryGetComponent(out TComponent component)) continue;
                
            detectedComponents.Add(component);
        }

        if (detectedComponents.Count == 0) return false;
            
        detected = detectedComponents;
        return true;
    }

    public void ChangeDetectionRange(float range)
    {
        detectionRange = range;
    }
}
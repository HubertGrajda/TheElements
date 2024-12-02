using System.Collections.Generic;
using System.Linq;
using _Scripts;
using UnityEngine;

public static class Detection
{
    public static WaterSource GetNearestWaterSource(Vector3 objectPos)
    {
        var minDistance = Mathf.Infinity;
        var waterSources = GameObject.FindGameObjectsWithTag(Constants.Tags.WATER_TAG);
        var nearestWater = waterSources[0];

        foreach (var water in waterSources)
        {
            var distance = Vector3.Distance(objectPos, water.transform.position);
            
            if (minDistance <= distance) continue;
            
            minDistance = distance;
            nearestWater = water;
        }

        return !nearestWater.TryGetComponent(out WaterSource nearestWaterSource) ? null : nearestWaterSource;
    }

    public static Color32 AverageColorFromTexture(Texture2D texture)
    {
        var textureColors = texture.GetPixels32();
        var pixels = textureColors.Length;
        var red = 0;
        var green = 0;
        var blue = 0;
        
        foreach (var pixelColor in textureColors)
        {
            red += pixelColor.r;
            green += pixelColor.g;
            blue += pixelColor.b;
        }
        return new Color32((byte)(red/pixels), (byte)(green/pixels), (byte)(blue/pixels), 255);
    }
    
    public static Color32 MainColorFromTexture(Texture2D texture, int precision)
    {
        var textureColors = texture.GetPixels32();
        var pixels = textureColors.Length;
        var red = 0;
        var green = 0;
        var blue = 0;
        
        for (var i = 0; i < precision; i++)
        {
            var random = Random.Range(0, pixels);
            red += textureColors[random].r;
            green += textureColors[random].g;
            blue += textureColors[random].b;
        }
        
        return new Color32((byte)(red / precision), (byte)(green / precision), (byte)(blue / precision), 255);
    }

    public static Color ClosestGroundColor(Vector3 objectPos)
    {
        var grounds = GameObject.FindGameObjectsWithTag(Constants.Tags.GROUND_TAG);
        var closestGround = grounds[0];
        var closestDistance = Mathf.Infinity;
        
        foreach (var ground in grounds)
        {
            var currDistance = Vector3.Distance(objectPos, ground.transform.position);
            
            if (currDistance < closestDistance)
            {
                closestDistance = currDistance;
                closestGround = ground;
            }
        }
        
        return closestGround.GetComponent<MeshRenderer>().material.color;
    }
}

public abstract class Detector : ScriptableObject
{
    [SerializeField] private int detectionLimit;
    protected int DetectionLimit => detectionLimit;
}

public class SphereDetector : Detector
{
    public bool TryGetDetected<TComponent>(Vector3 position, float range, string tag, out List<TComponent> detected)
    {
        detected = default;
        var detectedColliders = new Collider[DetectionLimit];
        var size = Physics.OverlapSphereNonAlloc(position, range, detectedColliders);

        if (size == 0) return false;
            
        detectedColliders = detectedColliders.Where(x => x.CompareTag(tag)).ToArray();

        if (detectedColliders.Length == 0) return false;

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
}

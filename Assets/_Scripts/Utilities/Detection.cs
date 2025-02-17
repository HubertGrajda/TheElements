using UnityEngine;

namespace _Scripts
{
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

            return nearestWater.TryGetComponent(out WaterSource nearestWaterSource) ? nearestWaterSource : null;
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
    }
}
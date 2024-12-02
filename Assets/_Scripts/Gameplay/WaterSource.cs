using UnityEngine;

namespace _Scripts
{
    [RequireComponent(typeof(MeshRenderer))]
    public class WaterSource : MonoBehaviour, IColorProvider
    {
        private static readonly int WaterColor = Shader.PropertyToID("_WaterColor");

        private MeshRenderer _meshRenderer;
        private Color _providedColor;
        
        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
            _providedColor = _meshRenderer.material.GetColor(WaterColor);
        }

        public Color GetColor() => _providedColor;
    }
}
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts.Spells
{
    public class WaterBall: WaterSpell
    {
        [SerializeField] private AnimationCurve speedCurve;
        [SerializeField] private AnimationCurve varietyCurve;
        [SerializeField] private float varietyRange = 5;
        private static readonly int WaterColor = Shader.PropertyToID("_WaterColor");

        private void SetMaterial()
        {
            var color = Detection.NearestWater(transform.position).GetComponent<MeshRenderer>().material.GetColor(WaterColor);  
            Vfx.SetVector4("waterColor", color);
        }
    
        private IEnumerator MoveToTarget(Vector3 target)
        {
            var startPosition = transform.position;
            var time = 0f;
            var variety = new Vector3(Random.Range(-varietyRange, varietyRange), Random.Range(-varietyRange, varietyRange), 0);
        
            while (time < 1)
            {
                var varietyTime = varietyCurve.Evaluate(time);
                transform.position = Vector3.Lerp(startPosition, target, speedCurve.Evaluate(time)) +
                                     new Vector3(varietyTime * variety.x, 0, varietyTime * variety.y);
            
                var magnitude = (transform.position - target).magnitude;
            
                if (magnitude < 0f) break;
            
                time += Time.deltaTime * speed;
                yield return null;
            }
            Vfx.SendEvent("OnHit");
            StartCoroutine(DisableSpellAfterDelay());
        }
    
        public override void CastSpell()
        {
            base.CastSpell();
            var screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
            var ray = Camera.main.ScreenPointToRay(screenCenter);
        
            if (Physics.Raycast(ray, out var hit))
            {
                SetMaterial();
                StartCoroutine(MoveToTarget(hit.point));
            }
            else
            {
                StartCoroutine(MoveToTarget(ray.GetPoint(50)));
            }
        }
    }
}
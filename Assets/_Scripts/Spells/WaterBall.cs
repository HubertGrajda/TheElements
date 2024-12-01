using System.Collections;
using _Scripts.Managers;
using UnityEngine;

namespace _Scripts.Spells
{
    public class WaterBall: Spell
    {
        [SerializeField] private AnimationCurve speedCurve;
        [SerializeField] private AnimationCurve varietyCurve;
        [SerializeField] private float varietyRange = 5;
        
        private const string WATER_VFX_COLOR = "waterColor";
        private const string ON_HIT_EVENT = "OnHit";

        public override void Launch()
        {
            base.Launch();
            
            var screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
            var ray = CameraManager.Instance.CameraMain.ScreenPointToRay(screenCenter);
            
            var target = Physics.Raycast(ray, out var hit) 
                ? hit.point 
                : ray.GetPoint(50f);
            
            SetMaterial();
            StartCoroutine(MoveToTarget(target));
        }
        
        private IEnumerator MoveToTarget(Vector3 target)
        {
            var startPosition = transform.position;
            var time = 0f;

            var varietyX = Random.Range(-varietyRange, varietyRange);
            var varietyY = Random.Range(-varietyRange, varietyRange);
        
            while (time < 1)
            {
                var varietyTime = varietyCurve.Evaluate(time);
                var evaluatedSpeed = speedCurve.Evaluate(time);
                
                var varietyOffset = new Vector3(varietyTime * varietyX, 0, varietyTime * varietyY);
                var newPosition = Vector3.Lerp(startPosition, target, evaluatedSpeed) + varietyOffset;
                var magnitude = (newPosition - target).magnitude;
                
                if (magnitude < 0f) break;
                
                transform.position = newPosition;
            
                time += Time.deltaTime * speed;
                yield return null;
            }
            
            Vfx.SendEvent(ON_HIT_EVENT); 
            Disable();
        }
        
        private void SetMaterial()
        {
            var nearestWater = Detection.GetNearestWaterSource(transform.position);
            var color = nearestWater.GetColor();  
            
            Vfx.SetVector4(WATER_VFX_COLOR, color);
        }
    }
}
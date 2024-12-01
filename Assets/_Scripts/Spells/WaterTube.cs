using System.Collections;
using _Scripts.Managers;
using UnityEngine;

namespace _Scripts.Spells
{
    public class WaterTube : Spell
    {
        [SerializeField] private AnimationCurve speedCurve;
        [SerializeField] private float endSpeed;

        private Vector3 _startPoint;
        private Vector3 _middlePoint1;
        private Vector3 _middlePoint2;
        private Vector3 _endPoint;

        private const string START_POINT_PARAM = "StartPoint";
        private const string MIDDLE_POINT_1_PARAM = "MiddlePoint1";
        private const string MIDDLE_POINT_2_PARAM = "MiddlePoint2";
        private const string END_POINT_PARAM = "EndPoint";
    
        private void Update()
        {
            SpellCollider.transform.position = Vfx.GetVector3(END_POINT_PARAM);
            _middlePoint1 = Vector3.Lerp(_startPoint, _endPoint, 0.2f) + new Vector3(0,3f,0); ;
            _middlePoint2 = Vector3.Lerp(_startPoint, _endPoint, 0.7f) + new Vector3(0,1.5f, 0);
        }

        public override void PrepareToLaunch()
        {
            base.PrepareToLaunch();
            
            var screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
            var ray = CameraManager.Instance.CameraMain.ScreenPointToRay(screenCenter);
            var nearestWater = Detection.GetNearestWaterSource(_endPoint);

            _endPoint = Physics.Raycast(ray, out var hit) 
                ? hit.point
                : ray.GetPoint(50);

            _startPoint = nearestWater.transform.position;
            Vfx.SetVector4("Color", nearestWater.GetColor());
        }

        public override void Launch()
        {
            base.Launch();
            
            StartCoroutine(MoveToTarget(_endPoint));
        }
        
        private IEnumerator MoveToTarget(Vector3 target)
        {
            var variety = new Vector3(Random.Range(-3, 3f), Random.Range(2, 3f), Random.Range(-3, 3f));
            var startPosition = _startPoint;
            
            Vfx.SetVector3(START_POINT_PARAM, _startPoint);
            Vfx.SetVector3(MIDDLE_POINT_1_PARAM, _startPoint);
            Vfx.SetVector3(MIDDLE_POINT_2_PARAM, _startPoint);
            Vfx.SetVector3(END_POINT_PARAM, _startPoint);

            var endPointPos = startPosition;
            var middlePoint1Pos = startPosition;
            var middlePoint2Pos = startPosition;
            
            float time = 0;

            while (time < 1)
            {
                endPointPos = Vector3.Slerp(startPosition, _middlePoint1, speedCurve.Evaluate(time));
                middlePoint2Pos = Vector3.Lerp(middlePoint2Pos, endPointPos, speedCurve.Evaluate(time/2));
                middlePoint1Pos = Vector3.Lerp(middlePoint1Pos, middlePoint2Pos, speedCurve.Evaluate(time/3));
                
                Vfx.SetVector3(END_POINT_PARAM, endPointPos);
                Vfx.SetVector3(MIDDLE_POINT_2_PARAM, middlePoint2Pos);
                Vfx.SetVector3(MIDDLE_POINT_1_PARAM, middlePoint1Pos);
            
                time += Time.deltaTime * speed;
                yield return null;
            }
        
            while (time < 1)
            {
                var speedCurveEvaluated = speedCurve.Evaluate(time);

                endPointPos = Vector3.Slerp(startPosition, _middlePoint2 + variety, speedCurveEvaluated);
                middlePoint2Pos = Vector3.Lerp(middlePoint2Pos, endPointPos, speedCurveEvaluated);
                middlePoint1Pos = Vector3.Lerp(middlePoint1Pos, middlePoint2Pos, speedCurveEvaluated);
                startPosition = Vector3.Lerp(startPosition, middlePoint1Pos, speedCurveEvaluated);
                
                Vfx.SetVector3(END_POINT_PARAM, endPointPos);
                Vfx.SetVector3(MIDDLE_POINT_2_PARAM, middlePoint2Pos);
                Vfx.SetVector3(MIDDLE_POINT_1_PARAM, middlePoint1Pos);
                Vfx.SetVector3(START_POINT_PARAM, startPosition);
            
                time += Time.deltaTime * speed;
                yield return null;
            }

            time = 0;
            while (time < 1 )
            {
                var speedCurveEvaluated = speedCurve.Evaluate(time);

                endPointPos = Vector3.Lerp(endPointPos, _endPoint, speedCurveEvaluated);
                middlePoint2Pos = Vector3.Lerp(middlePoint2Pos, endPointPos, speedCurveEvaluated);
                middlePoint1Pos = Vector3.Lerp(middlePoint1Pos, middlePoint2Pos, speedCurveEvaluated);
                startPosition = Vector3.Lerp(startPosition, middlePoint1Pos, speedCurveEvaluated);

                Vfx.SetVector3(END_POINT_PARAM, endPointPos);
                Vfx.SetVector3(MIDDLE_POINT_2_PARAM, middlePoint2Pos);
                Vfx.SetVector3(MIDDLE_POINT_1_PARAM, middlePoint1Pos);
                Vfx.SetVector3(START_POINT_PARAM, startPosition);

                time += Time.deltaTime * endSpeed ;
                yield return null;
            }
            Disable();
        }
        
        private void OnCollisionEnter(Collision _)
        {
            Disable();
        }
    }
}
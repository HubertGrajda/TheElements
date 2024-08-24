using System.Collections;
using UnityEngine;

namespace _Scripts.Spells
{
    public class WaterTube : WaterSpell
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
            spellCollider.transform.position = vfx.GetVector3(END_POINT_PARAM);
            _middlePoint1 = Vector3.Lerp(_startPoint, _endPoint, 0.2f) + new Vector3(0,3f,0); ;
            _middlePoint2 = Vector3.Lerp(_startPoint, _endPoint, 0.7f) + new Vector3(0,1.5f, 0);
        }
    
        private IEnumerator MoveToTarget(Vector3 target)
        {
            var variety = new Vector3(Random.Range(-3, 3f), Random.Range(2, 3f), Random.Range(-3, 3f));
            var startPosition = _startPoint;
            vfx.SetVector3(START_POINT_PARAM, _startPoint);
            vfx.SetVector3(MIDDLE_POINT_1_PARAM, _startPoint);
            vfx.SetVector3(MIDDLE_POINT_2_PARAM, _startPoint);
            vfx.SetVector3(END_POINT_PARAM, _startPoint);

            float time = 0;

            while (time < 1)
            {
                vfx.SetVector3(END_POINT_PARAM,
                    Vector3.Slerp(startPosition, _middlePoint1, speedCurve.Evaluate(time)));
            
                vfx.SetVector3(MIDDLE_POINT_2_PARAM,
                    Vector3.Lerp(vfx.GetVector3(MIDDLE_POINT_2_PARAM), vfx.GetVector3(END_POINT_PARAM), speedCurve.Evaluate(time/2)));
            
                vfx.SetVector3(MIDDLE_POINT_1_PARAM,
                    Vector3.Lerp(vfx.GetVector3(MIDDLE_POINT_1_PARAM), vfx.GetVector3(MIDDLE_POINT_2_PARAM), speedCurve.Evaluate(time/3)));
            
                time += Time.deltaTime * speed;
                yield return null;
            }
        
            while (time < 1)
            {
                var speedCurveEvaluated = speedCurve.Evaluate(time);
                vfx.SetVector3(END_POINT_PARAM,
                    Vector3.Slerp(startPosition, _middlePoint2 + variety, speedCurveEvaluated));
            
                vfx.SetVector3(MIDDLE_POINT_2_PARAM,
                    Vector3.Lerp(vfx.GetVector3(MIDDLE_POINT_2_PARAM), vfx.GetVector3(END_POINT_PARAM), speedCurveEvaluated));
            
                vfx.SetVector3(MIDDLE_POINT_1_PARAM,
                    Vector3.Lerp(vfx.GetVector3(MIDDLE_POINT_1_PARAM), vfx.GetVector3(MIDDLE_POINT_2_PARAM), speedCurveEvaluated));
            
                vfx.SetVector3(START_POINT_PARAM,
                    Vector3.Lerp(vfx.GetVector3(START_POINT_PARAM), vfx.GetVector3(MIDDLE_POINT_1_PARAM), speedCurveEvaluated));
            
                time += Time.deltaTime * speed;
                yield return null;
            }

            time = 0;
            while (time < 1 )
            {
                var speedCurveEvaluated = speedCurve.Evaluate(time);
                vfx.SetVector3(END_POINT_PARAM,
                    Vector3.Lerp(vfx.GetVector3(END_POINT_PARAM), _endPoint, speedCurveEvaluated));
            
                vfx.SetVector3(MIDDLE_POINT_2_PARAM,
                    Vector3.Lerp(vfx.GetVector3(MIDDLE_POINT_2_PARAM), vfx.GetVector3(END_POINT_PARAM), speedCurveEvaluated));
            
                vfx.SetVector3(MIDDLE_POINT_1_PARAM,
                    Vector3.Lerp(vfx.GetVector3(MIDDLE_POINT_1_PARAM), vfx.GetVector3(MIDDLE_POINT_2_PARAM), speedCurveEvaluated));
            
                vfx.SetVector3(START_POINT_PARAM,
                    Vector3.Lerp(vfx.GetVector3(START_POINT_PARAM), vfx.GetVector3(MIDDLE_POINT_1_PARAM), speedCurveEvaluated));

                time += Time.deltaTime * endSpeed ;
                yield return null;
            }
            StartCoroutine(DisableSpellAfterDelay());
        }
        private void OnCollisionEnter(Collision collision)
        {
            StartCoroutine(DisableSpellAfterDelay());
        }

        public override void CastSpell()
        {
            var screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
            var ray = Camera.main.ScreenPointToRay(screenCenter);

            _endPoint = Physics.Raycast(ray, out var hit) 
                ? hit.point
                : ray.GetPoint(50);

            _startPoint = Detection.NearestWater(_endPoint).transform.position;
            vfx.SetVector4("Color", Detection.NearestWater(_endPoint).GetComponent<MeshRenderer>().material.GetColor("_WaterColor"));
            StartCoroutine(MoveToTarget(_endPoint));
        }
    }
}
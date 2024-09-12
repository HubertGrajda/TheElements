using System.Collections;
using UnityEngine;

namespace _Scripts.Spells
{
    [RequireComponent(typeof(Rigidbody))]
    public class RollingBoulder : EarthSpell
    {
        [SerializeField] private float detectingDistance = 3f;
        [SerializeField] private float distanceBetweenGround = 0.3f;

        [Tooltip("0 - no adjustment \n1 - instant adjustment")]
        [Range(0f, 1f)]
        [SerializeField] private float heightAdjustmentSpeed = 0.5f;
        [SerializeField] private float fallingSpeed = 0.05f;
        [SerializeField] private float slowingDownTime = 3f;
        [SerializeField] private float delayToSlowDown = 10f;
        
        private float _currentSpeed;
        private bool _stopped;

        private Rigidbody _rb;
        private Texture2D _currTex;
        private Color _currColor = Color.black;


        protected override void Awake()
        {
            base.Awake();
            _rb = GetComponent<Rigidbody>();
        }
        
        private void Update()
        {
            if (_stopped) return;
            
            ChangeColorToGroundTexture();
            SnapToGround();
        }
        
        private void AddVelocity(Vector3 direction) // function call when casting
        {
            _stopped = false;
            _rb.velocity = direction * speed;
            transform.rotation.SetLookRotation(direction);
        }

        private void SnapToGround()
        {
            var rayStartPoint = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
            if (Physics.Raycast(rayStartPoint, transform.TransformDirection(-Vector3.up), out var hit, detectingDistance))
            {
                var targetPosition = new Vector3(transform.position.x, hit.point.y + distanceBetweenGround, transform.position.z);
                
                transform.position = targetPosition;
                Vfx.SetVector3("ColliderPosition", hit.point + new Vector3(0, -0.5f, 0));
            }
            else
            {
                transform.position = new Vector3(transform.position.x, transform.position.y - fallingSpeed * Time.deltaTime, transform.position.z);
                Vfx.SendEvent("NoGround");
            }
        }
        
        public override void CastSpell()
        {
            base.CastSpell();
            AddVelocity(transform.forward);
            StartCoroutine(SlowDownAfterDelay(delayToSlowDown));
            Vfx.SendEvent("OnPlay");
        }
        
        private IEnumerator SlowDownAfterDelay(float delay)
        {
            var timer = 0f;
            var velocity = _rb.velocity;
            yield return new WaitForSeconds(delay);
            
            while (timer < slowingDownTime)
            {
                _rb.velocity = Vector3.Lerp(velocity, Vector3.zero, timer / slowingDownTime);
                
                timer += Time.deltaTime;
                yield return null;
            }

            _rb.velocity = Vector3.zero;
            _stopped = true;
            StartCoroutine(DisableSpellAfterDelay());
        }
        
        private void ChangeColorToGroundTexture()
        {
            var rayStartPoint = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            
            if (!Physics.Raycast(rayStartPoint, transform.TransformDirection(-Vector3.up), out var hit)) return;
            
            if (hit.collider.gameObject.layer != LayerMask.NameToLayer("Ground")) return;

            var color = _currColor;
            var meshRenderer = hit.collider.GetComponent<MeshRenderer>();
            var tex = meshRenderer.material.mainTexture as Texture2D;

            if (hit.collider.TryGetComponent(out Terrain terrain))
            {
                var terrainPosition = hit.point - terrain.transform.position;
                var splatMapPosition = new Vector3(
                    terrainPosition.x / terrain.terrainData.size.x, 
                    0,
                    terrainPosition.z / terrain.terrainData.size.z);
                
                var x = Mathf.FloorToInt(splatMapPosition.x * terrain.terrainData.alphamapWidth);
                var z = Mathf.FloorToInt(splatMapPosition.z * terrain.terrainData.alphamapHeight);
                var alphaMap = terrain.terrainData.GetAlphamaps(x, z, 1, 1);

                var primaryTex = 0;
                for (var i = 1; i < alphaMap.Length; i++)
                {
                    if (!(alphaMap[0, 0, i] > alphaMap[0, 0, primaryTex])) continue;

                    primaryTex = i;
                }

                tex = terrain.terrainData.terrainLayers[primaryTex].diffuseTexture;
                
                if (_currTex != tex)
                {
                    color = Detection.MainColorFromTexture(
                        terrain.terrainData.terrainLayers[primaryTex].diffuseTexture, 100);
                    _currTex = tex;
                }
            }
            else if (tex == null)
            {
                color = meshRenderer.material.color;
            }
            else if (tex != _currTex)
            {
                color = Detection.MainColorFromTexture(tex, 50);
                _currTex = tex;
            }

            if (_currColor == color) return;
            
            _currColor = color;
            Vfx.SetVector4("Color1", color);
        }
    }
}
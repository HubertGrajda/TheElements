using System.Collections;
using UnityEngine;

namespace _Scripts.Spells
{
    public class RollingBoulder : EarthSpell
    {
        [SerializeField] private float detectingDistance = 3f;
        [SerializeField] private float distanceBetweenGround = 0.3f;

        [Tooltip("0 - no adjustment \n1 - instant adjustment")]
        [Range(0f, 1f)]
        [SerializeField] private float heightAdjustmentSpeed = 0.5f;
        [SerializeField] private float fallingSpeed = 0.05f; // how fast will the object fall if there is no ground below

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
        
        private void OnEnable()
        {
            _stopped = false;
            StartCoroutine(SlowDown());
            vfx.SendEvent("OnPlay");
        }

        private void Update()
        {
            ChangeColorToGroundTexture();
            SnapToGround();
        }
        
        private void AddVelocity(Vector3 direction) // function call when casting
        {
            _rb.velocity = direction * speed;
            transform.rotation.SetLookRotation(direction);
        }

        private void SnapToGround()
        {
            if (_stopped) return;
            
            var rayStartPoint = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
            if (Physics.Raycast(rayStartPoint, transform.TransformDirection(-Vector3.up), out var hit, detectingDistance))
            {
                transform.position = Vector3.Lerp(
                    transform.position, new Vector3(transform.position.x, hit.point.y + distanceBetweenGround, transform.position.z), heightAdjustmentSpeed);
                vfx.SetVector3("ColliderPosition", hit.point + new Vector3(0, -0.5f, 0));
            }
            else
            {
                transform.position = new Vector3(transform.position.x, transform.position.y - fallingSpeed, transform.position.z);
                vfx.SendEvent("NoGround");
            }
        }
        
        public override void CastSpell()
        {
            base.CastSpell();
            AddVelocity(transform.forward);
        }
        
        private IEnumerator SlowDown()
        {
            _currentSpeed = 1;
            while (_currentSpeed > 0)
            {
                _rb.velocity = Vector3.Lerp(Vector3.zero, _rb.velocity, _currentSpeed);
                _currentSpeed -= 0.01f;
                yield return null;
            }
            _stopped = true;
            StartCoroutine(DisableSpellAfterDelay());
        }
        
        private void ChangeColorToGroundTexture()
        {
            var color = _currColor;
            var rayStartPoint = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            
            if (!Physics.Raycast(rayStartPoint, transform.TransformDirection(-Vector3.up), out var hit)) return;
            
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                var meshRenderer = hit.collider.GetComponent<MeshRenderer>();
                var tex = meshRenderer.material.mainTexture as Texture2D;
                if (hit.collider is TerrainCollider)
                {
                    var terrain = hit.collider.GetComponent<Terrain>();
                    var terrainPosition = hit.point - terrain.transform.position;
                    var splatMapPosition = new Vector3(terrainPosition.x / terrain.terrainData.size.x, 0, terrainPosition.z / terrain.terrainData.size.z);
                    var x = Mathf.FloorToInt(splatMapPosition.x * terrain.terrainData.alphamapWidth);
                    var z = Mathf.FloorToInt(splatMapPosition.z * terrain.terrainData.alphamapHeight);
                    var alphaMap = terrain.terrainData.GetAlphamaps(x, z, 1, 1);
                    var primaryTex = 0;
                    
                    for (var i = 1; i < alphaMap.Length; i++)
                    {
                        if (alphaMap[0, 0, i] > alphaMap[0, 0, primaryTex])
                        {
                            primaryTex = i;
                        }
                    }
                    tex = terrain.terrainData.terrainLayers[primaryTex].diffuseTexture;
                    if(_currTex != tex)
                    {
                        color = Detection.MainColorFromTexture(terrain.terrainData.terrainLayers[primaryTex].diffuseTexture, 100);
                        _currTex = tex;
                    }
                
                }
                else if (tex == null)
                {
                    color = meshRenderer.material.color;
                }
                else if (tex != null && tex != _currTex)
                {
                    color = Detection.MainColorFromTexture(tex,50);
                    _currTex = tex;
                }
            }

            if (_currColor == color) return;
            
            _currColor = color;
            vfx.SetVector4("Color1", color);
        }
    }
}
using System.Collections;
using UnityEngine;

namespace _Scripts.Spells
{
    [RequireComponent(typeof(Rigidbody))]
    public class RollingBoulder : Spell
    {
        [SerializeField] private float detectingDistance = 3f;
        [SerializeField] private float distanceBetweenGround = 0.3f;

        [Tooltip("0 - no adjustment \n1 - instant adjustment")]
        [Range(0f, 1f)]
        [SerializeField] private float heightAdjustmentSpeed = 0.5f;
        [SerializeField] private float fallingSpeed = 0.05f;
        [SerializeField] private float slowingDownTime = 3f;
        [SerializeField] private float delayToSlowDown = 10f;
        
        private Rigidbody _rb;
        private Texture2D _currTex;

        private bool _stopped;
        private float _currentSpeed;
        private Color _currColor = Color.black;

        private const string COLOR_PARAM = "Color1";
        private const string COLLIDER_POSITION_PARAM = "ColliderPosition";
        private const string NO_GROUND_EVENT = "NoGround";
        private const string ON_PLAY_EVENT = "OnPlay";
        
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
            var posX = transform.position.x;
            var posY = transform.position.y;
            var posZ = transform.position.z;
            var rayStartPoint = new Vector3(posX, posY + 1, posZ);
            
            if (Physics.Raycast(rayStartPoint, transform.TransformDirection(-Vector3.up), out var hit, detectingDistance))
            {
                var targetPosition = new Vector3(posX, hit.point.y + distanceBetweenGround, posZ);
                var offset = new Vector3(0, -0.5f, 0);
                
                transform.position = targetPosition;
                Vfx.SetVector3(COLLIDER_POSITION_PARAM, hit.point + offset);
            }
            else
            {
                var newPosY = posY - fallingSpeed * Time.deltaTime;
                transform.position = new Vector3(posX, newPosY, posZ);
                Vfx.SendEvent(NO_GROUND_EVENT);
            }
        }
        
        public override void Launch()
        {
            base.Launch();
            AddVelocity(transform.forward);
            StartCoroutine(SlowDownAfterDelay(delayToSlowDown));
            Vfx.SendEvent(ON_PLAY_EVENT);
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
            Disable();
        }
        
        private void ChangeColorToGroundTexture()
        {
            var rayStartPoint = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            
            if (!Physics.Raycast(rayStartPoint, transform.TransformDirection(-Vector3.up), out var hit)) return;
            
            if (hit.collider.gameObject.layer != LayerMask.NameToLayer(Constants.Tags.GROUND_TAG)) return;

            if (hit.collider.TryGetComponent(out Terrain terrain))
            {
                SetVisualsFromTerrain(hit.point, terrain);
                return;
            }
            
            if (!hit.collider.TryGetComponent(out MeshRenderer meshRenderer)) return;

            if (meshRenderer.material.mainTexture is Texture2D tex)
            {
                SetVisualsFromTexture2D(tex);
                return;
            }

            _currTex = null;
            _currColor = meshRenderer.material.color;
            
            Vfx.SetVector4(COLOR_PARAM, _currColor);
        }

        private void SetVisualsFromTerrain(Vector3 point, Terrain terrain)
        {
            var terrainData = terrain.terrainData;
            var terrainPosition = point - terrain.transform.position;
                
            var splatMapPositionX = terrainPosition.x / terrainData.size.x;
            var splatMapPositionZ = terrainPosition.z / terrainData.size.z;
                
            var x = Mathf.FloorToInt(splatMapPositionX * terrainData.alphamapWidth);
            var z = Mathf.FloorToInt(splatMapPositionZ * terrainData.alphamapHeight);
                
            var alphaMap = terrain.terrainData.GetAlphamaps(x, z, 1, 1);

            var primaryTex = 0;
                
            for (var i = 1; i < alphaMap.Length; i++)
            {
                if (alphaMap[0, 0, i] <= alphaMap[0, 0, primaryTex]) continue;

                primaryTex = i;
            }

            var texture = terrainData.terrainLayers[primaryTex].diffuseTexture;

            SetVisualsFromTexture2D(texture);
        }

        private void SetVisualsFromTexture2D(Texture2D texture)
        {
            if (texture == _currTex) return;
            
            _currColor = Detection.MainColorFromTexture(texture, 50);
            _currTex = texture;
            
            Vfx.SetVector4(COLOR_PARAM, _currColor);
        }
    }
}
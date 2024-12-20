using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

namespace _Scripts.Spells
{
    [RequireComponent(typeof(Collider))]
    public abstract class Spell : MonoBehaviour, ISpell, IPoolable
    {
        [field: SerializeField] public SpellConfig SpellData { get; private set; }
        [field: SerializeField] public VisualEffect Vfx { get; private set; }

        [SerializeField] private float disableDelay = 5f;
        [SerializeField] protected float speed;

        protected Collider SpellCollider { get; private set; }
        protected bool Launched { get; private set; }
        public bool Cancelled { get; private set; }
        protected Transform SpawnPoint { get; private set; }
        protected SpellLauncher SpellLauncher { get; private set; }
        
        public virtual bool CanBeLaunched => true;
        
        protected virtual void Awake()
        {
            SpellCollider = GetComponent<Collider>();
        }
        
        public virtual void Use(SpellLauncher spellLauncher)
        {
            Launched = false;
            Cancelled = false;
            
            SpellLauncher = spellLauncher;
            SpawnPoint = spellLauncher.SpawnPoint;
            
            transform.position = SpawnPoint.position;
            
            if (SpellData.IsChildOfSpawnPoint)
            {
                transform.SetParent(SpawnPoint);
            }
        }

        public virtual void Cast()
        {
        }

        public virtual void PrepareToLaunch()
        {
        }
        
        public virtual void Launch()
        {
            PrepareToLaunch();
            gameObject.SetActive(true);
            Launched = true;
        }
        
        public virtual void Cancel()
        {
            Cancelled = true;
        }
        
        protected void Disable()
        {
            if (!gameObject.activeInHierarchy) return;
            
            StartCoroutine(DisableWithLastParticle());
        }
        
        private IEnumerator DisableWithLastParticle()
        {
            var timer = 0f;
            while (timer < disableDelay)
            {
                timer += Time.deltaTime;
                yield return null;
            }
            
            gameObject.SetActive(false);
        }

        protected void OnDisable() => StopAllCoroutines();
    }
}
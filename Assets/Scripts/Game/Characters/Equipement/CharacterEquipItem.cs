using Sirenix.OdinInspector;
using TestTask.Game.Vitals;
using UnityEngine;
using Zenject;

namespace TestTask.Game.Characters
{
    public class CharacterEquipItem : MonoInstaller, ICharacterEquipItem
    {
        [Inject, ShowInInspector, ReadOnly, FoldoutGroup("Debug")]
        protected IUltimateCharacterController character { get; private set; }

        [SerializeField, FoldoutGroup("Attack")] float attackRange = 1f;
        [SerializeField, FoldoutGroup("Attack")] float attackRate = 0.75f;
        [SerializeField, FoldoutGroup("Attack")] float attackDuration = 0.75f;
        [SerializeField, FoldoutGroup("Attack")] float damage = 5;

        [SerializeField] bool directionByRoot = true;
        [SerializeField] Transform rayOriginPoint;
        [SerializeField] LayerMask targetsMask;

        [SerializeField, FoldoutGroup("Events")] CharacterEquipAnimationEvents animEvents; 

        private float lastAttackTime;
        private float fromLastAttackSeconds => Time.time - lastAttackTime;

        public float AttackRange => attackRange;
        public float AttackRate => attackRate;
        public float Damage => damage;

        public bool IsItemReadyToAttack => fromLastAttackSeconds >= attackRange;
        public ICharacterEquipEvents Events => animEvents;

        public bool CanPlayerMoveWhenAttack;

        public override void InstallBindings()
        {
            Container.
                Bind<ICharacterEquipItem>().
                FromInstance(this).
                AsCached().
                NonLazy();
        }

        public override void Start()
        {
            Events.Attack += AttackMoment;
        }

        private void AttackMoment ()
        {
            var hit = Physics2D.Raycast(rayOriginPoint.position,
                directionByRoot ? character.View.Root.forward : rayOriginPoint.forward,
                AttackRange,
                targetsMask);
            if (hit && hit.collider)
            {
                if(hit.collider.transform.TryGetComponent<RaycastTargetTransfer>(out var transfer))
                {
                    if(transfer.NextObject.TryGetComponent<IHealthController>(out var health))
                    {
                        HandleAttackByObject(health);
                    }
                }
            }
        }

        private void HandleAttackByObject (IHealthController health)
        {
            health.ChangeHealth(new ChangeHealthData(gameObject, -damage));
        }

        public virtual void Attack()
        {
            lastAttackTime = Time.time;

            if (CanPlayerMoveWhenAttack == false)
                character.CanMove = false;
        }

        private void OnDrawGizmosSelected()
        {
            if (rayOriginPoint == null)
                return;

            var origin = rayOriginPoint ? rayOriginPoint.position : transform.position;
            var dir = (directionByRoot && character != null) ? character.View.Root.forward : rayOriginPoint.forward;
            Gizmos.color = Color.red;
            Gizmos.DrawLine(origin, origin + dir * AttackRange);
        }
    }
}
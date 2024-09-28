using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace TestTask.Game.Characters
{
    public class CharacterEquipItem : MonoInstaller, ICharacterEquipItem
    {
        [ShowInInspector, ReadOnly, FoldoutGroup("Debug")]
        protected IUltimateCharacterController character { get; private set; }

        [SerializeField, FoldoutGroup("Attack")] float attackRange = 1f;
        [SerializeField, FoldoutGroup("Attack")] float attackRate = 0.75f;
        [SerializeField, FoldoutGroup("Attack")] float attackDuration = 0.75f;
        [SerializeField, FoldoutGroup("Attack")] float damage = 5;

        private float lastAttackTime;
        private float fromLastAttackSeconds =>
            Time.time - lastAttackTime;

        public float AttackRange => attackRange;
        public float AttackRate => attackRate;
        public float Damage => damage;

        public bool IsItemReadyToAttack => fromLastAttackSeconds >= attackRange;

        public bool CanPlayerMoveWhenAttack;
        public string AttackClipName = "Attack";

        public virtual void Attack()
        {
            lastAttackTime = Time.time;

            if (CanPlayerMoveWhenAttack == false)
                character.CanMove = false;
        }

        public override void InstallBindings()
        {
            Container.
                Bind<ICharacterEquipItem>().
                FromInstance(this).
                AsCached().
                NonLazy();
        }
    }
}
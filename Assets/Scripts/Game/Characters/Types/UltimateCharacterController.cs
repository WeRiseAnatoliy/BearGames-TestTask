using Sirenix.OdinInspector;
using UnityEngine;

namespace TestTask.Game.Characters
{
    public class UltimateCharacterController : CharacterController, IUltimateCharacterController
    {
        [SerializeField] bool canMove = true;

        public float SpeedMove = 5f;

        [ShowInInspector, ReadOnly, FoldoutGroup("Debug")]
        public bool CanMove { get => canMove; set => canMove = value; }
        public AnimationCurve SpeedByMagnitude = AnimationCurve.Linear(0, 0, 1, 1);

        [ShowInInspector, ReadOnly, FoldoutGroup("Debug")]
        public Vector3 Velocity { get; private set; }

        public override void InstallBindings()
        {
            base.InstallBindings();

            Container.
               Bind<IUltimateCharacterController>().
               FromInstance(this).
               AsCached().
               NonLazy();
        }

        public virtual void Move(Vector3 direction)
        {
            if (canMove == false)
                return;

            var speed = SpeedByMagnitude.Evaluate(direction.magnitude);
            Velocity = direction * speed;
            View.SetMoving(Mathf.Abs(direction.x) * speed);
        }

        public void Attack()
        {
            View.SetMoving(0);
            View.PlayClip("Attack");
        }
    }
}
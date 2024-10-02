using Sirenix.OdinInspector;
using UnityEngine;

namespace TestTask.Game.Characters
{
    public class UltimateCharacterController : CharacterController, IUltimateCharacterController
    {
        [SerializeField, FoldoutGroup("Physics")] bool useTwoDPhysics;
        [SerializeField, FoldoutGroup("Physics")] bool useMovementInterface;

        [SerializeField, FoldoutGroup("Moving")] bool canMove = true;
        
        public float SpeedMove = 5f;

        [ShowInInspector, ReadOnly, FoldoutGroup("Debug")]
        public bool CanMove { get => canMove; set => canMove = value; }
        [FoldoutGroup("Moving")]
        public AnimationCurve SpeedByMagnitude = AnimationCurve.Linear(0, 0, 1, 1);

        [ShowInInspector, ReadOnly, FoldoutGroup("Debug")]
        public Vector3 Velocity { get; private set; }

        private IMovementPhysics physics;

        public override void Start()
        {
            base.Start();

            if(useMovementInterface)
                physics = useTwoDPhysics ?
                    new TwoDRigibdodyMovement(GetComponent<Rigidbody2D>()) :
                    new ThreeDRigibodyMovement(GetComponent<Rigidbody>());
        }

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

            if(useMovementInterface)
                physics.Move(Vector2.right * direction * speed);

            View.SetMoving(Mathf.Abs(direction.x) * speed);
        }

        public void Attack()
        {
            View.SetMoving(0);
            View.PlayClip("Attack");
        }
    }
}
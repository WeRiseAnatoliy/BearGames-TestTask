using UnityEngine;

namespace TestTask.Game.Characters
{
    public class UltimateCharacterController : CharacterController, IUltimateCharacterController
    {
        [SerializeField] bool canMove = true;

        public float SpeedMove = 5f;
        public bool CanMove { get => CanMove; set => canMove = value; }
        public AnimationCurve SpeedByMagnitude = AnimationCurve.Linear(0, 0, 1, 1);

        private Collider2D charCollider;
        private Rigidbody2D rbody;

        public Vector3 Velocity =>
            rbody ? rbody.velocity : Vector3.zero;

        public override void Start()
        {
            base.Start();

            charCollider = GetComponent<Collider2D>();
            rbody = GetComponent<Rigidbody2D>();
        }

        public void Move(Vector3 direction)
        {
            if (canMove == false)
                return;

            var speed = SpeedByMagnitude.Evaluate(direction.magnitude);
            rbody.velocity = direction * speed * SpeedMove; 
            View.SetMoving(direction.x * speed);
        }

        public void Attack()
        {
            
        }
    }
}
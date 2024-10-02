using UnityEngine;

namespace TestTask.Game.Characters
{
    public class ThreeDRigibodyMovement : IMovementPhysics
    {
        public readonly Rigidbody Rbody;

        public ThreeDRigibodyMovement(Rigidbody rbody)
        {
            Rbody = rbody;
        }

        public void Move(Vector3 direction)
        {
            Rbody.velocity = direction;
        }
    }
}
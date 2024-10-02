using UnityEngine;

namespace TestTask.Game.Characters
{
    public class TwoDRigibdodyMovement : IMovementPhysics
    {
        public readonly Rigidbody2D Rbody;

        public TwoDRigibdodyMovement(Rigidbody2D rbody)
        {
            Rbody = rbody;
        }

        public void Move(Vector3 direction)
        {
            Rbody.velocity = direction;
        }
    }
}
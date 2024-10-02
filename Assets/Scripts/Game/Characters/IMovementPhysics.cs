using UnityEngine;

namespace TestTask.Game.Characters
{
    public interface IMovementPhysics
    {
        void Move(Vector3 direction);
    }
}
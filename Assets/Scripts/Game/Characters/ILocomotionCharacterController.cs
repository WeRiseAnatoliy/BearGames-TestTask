using UnityEngine;

namespace TestTask.Game.Characters
{
    public interface ILocomotionCharacterController : ICharacterController
    {
        Vector3 Velocity { get; }
        void Move(Vector3 direction);
    }
}
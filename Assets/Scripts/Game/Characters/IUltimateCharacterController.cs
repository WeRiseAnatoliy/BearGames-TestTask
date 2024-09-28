using UnityEngine;

namespace TestTask.Game.Characters
{
    public interface IUltimateCharacterController : ICharacterController
    {
        bool CanMove { get; set; }
        Vector3 Velocity { get; }
        void Move(Vector3 direction);
        void Attack();
    }
}
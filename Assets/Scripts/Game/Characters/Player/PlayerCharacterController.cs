using UnityEngine;

namespace TestTask.Game.Characters
{
    public class PlayerCharacterController : UltimateCharacterController
    {
        protected override void Live_Update()
        {
            Move(Vector3.right * Input.GetAxis("Horizontal"));
        }
    }
}
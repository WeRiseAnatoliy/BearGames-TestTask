using UnityEngine;

namespace TestTask.Game.Characters
{
    public class PlayerCharacterController : TwoDUltimateCharacterController
    {
        protected override void Live_Update()
        {
            if(View.IsCurrentPlay("Attack") == false &&
                View.IsCurrentPlay("Hit") == false)
            {
                Move(Vector3.right * Input.GetAxis("Horizontal"));

                if (Input.GetMouseButtonDown(0))
                    Attack();
            }
        }
    }
}
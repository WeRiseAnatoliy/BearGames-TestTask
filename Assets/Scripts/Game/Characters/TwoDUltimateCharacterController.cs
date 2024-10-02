using Sirenix.OdinInspector;
using UnityEngine;

namespace TestTask.Game.Characters
{
    public class TwoDUltimateCharacterController : UltimateCharacterController
    {
        [ShowInInspector, ReadOnly, FoldoutGroup("Debug")]
        private Vector3 startViewRot;

        public override void Start()
        {
            base.Start();

            startViewRot = View.Root.localEulerAngles;
        }

        public override void Move(Vector3 direction)
        {
            if (CanMove == false)
                return;

            if(Mathf.Abs(direction.x) > 0.1f)
                SetLookAxis(direction.x > 0);

            base.Move(direction);
        }

        protected void SetLookAxis (bool right)
        {
            var y = right ? startViewRot.y : -startViewRot.y;
            Debug.Log($"Right or left: {right}, y angle {y}");
            View.Root.localEulerAngles = new Vector3(startViewRot.x, y, startViewRot.z);
        }
    }
}
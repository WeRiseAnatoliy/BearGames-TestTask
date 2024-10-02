using Sirenix.OdinInspector;
using UnityEngine;

namespace TestTask.Game.Characters
{
    public class TwoDUltimateCharacterController : UltimateCharacterController
    {
        [ShowInInspector, ReadOnly, FoldoutGroup("Debug")]
        private Vector3 startViewRot;
        private bool lastLookDirRight;

        public override void Start()
        {
            base.Start();

            startViewRot = View.Root.localEulerAngles;
        }

        public override void Move(Vector3 direction)
        {
            if (Mathf.Abs(direction.x) > 0.1f)
                lastLookDirRight = direction.x > 0;

            SetLookAxis(lastLookDirRight);

            base.Move(direction);
        }

        protected void SetLookAxis (bool right)
        {
            var y = right ? startViewRot.y : -startViewRot.y;
            View.Root.localEulerAngles = new Vector3(startViewRot.x, y, startViewRot.z);
        }
    }
}
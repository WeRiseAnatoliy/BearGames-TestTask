using UnityEngine;

namespace TestTask.Game.Enviroment
{
    public class ParallaxLayer : MonoBehaviour
    {
        public float Factor = 1f;

        internal void Move(float delta)
        {
            transform.localPosition += transform.right * delta * Factor;
        }
    }
}
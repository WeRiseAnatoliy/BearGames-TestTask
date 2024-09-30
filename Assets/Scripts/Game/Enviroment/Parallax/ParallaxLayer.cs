using UnityEngine;

namespace TestTask.Game.Enviroment
{
    public class ParallaxLayer : IParallaxLayer
    {
        public GameObject Target;
        public float Factor = 1f;

        public ParallaxLayer(GameObject target, float factor)
        {
            Target = target;
            Factor = factor;
        }

        void IParallaxLayer.Move(Vector3 targetMoveDelta)
        {
            Target.transform.localPosition += Target.transform.right * targetMoveDelta.x * Factor;
        }
    }
}
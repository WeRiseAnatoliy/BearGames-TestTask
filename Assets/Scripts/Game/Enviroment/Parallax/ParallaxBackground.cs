using System.Collections.Generic;
using UnityEngine;

namespace TestTask.Game.Enviroment
{
    public class ParallaxBackground : MonoBehaviour
    {
        [SerializeField] AnimationCurve layerSpeedFactor = AnimationCurve.EaseInOut(0, 0, 1, 1);

        private List<IParallaxLayer> layers = new List<IParallaxLayer>();
        private Transform followTarget;
        private Vector3 lastTargetPos;

        private void Start()
        {
            followTarget = Camera.main.transform;
            lastTargetPos = followTarget.position;
            RefreshLayers();
        }

        private void Update()
        {
            if (followTarget)
            {
                Move(lastTargetPos - followTarget.position);
                lastTargetPos = followTarget.position;
            }
        }

        private void Move(Vector3 delta)
        {
            for(var x = 0; x < layers.Count; x++)
                layers[x].Move(delta);
        }

        public void RefreshLayers ()
        {
            layers.Clear();

            for(var x = 0; x < transform.childCount; x++)
            {
                var percent = x / (float)transform.childCount;
                layers.Add(new ParallaxLayer(transform.GetChild(x).gameObject, layerSpeedFactor.Evaluate(percent)));
            }
        }
    }
}
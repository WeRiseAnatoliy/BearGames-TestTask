using System.Collections.Generic;
using UnityEngine;

namespace TestTask.Game.Enviroment
{
    public class ParallaxBackground : MonoBehaviour
    {
        [SerializeField] ParallaxFollowTarget followTarget;

        private List<ParallaxLayer> layers = new List<ParallaxLayer>();

        private void Start()
        {
            followTarget.OnChangePosition += Move;

            RefreshLayers();
        }

        private void OnDisable()
        {
            if(followTarget)
                followTarget.OnChangePosition -= Move;
        }

        private void Move(float deltaX)
        {
            for(var x = 0; x < layers.Count; x++)
                layers[x].Move(deltaX);
        }

        public void RefreshLayers ()
        {
            layers.Clear();

            for(var x = 0; x < transform.childCount; x++)
                if(transform.GetChild(x).TryGetComponent<ParallaxLayer>(out var layer))
                    layers.Add(layer);
        }
    }
}
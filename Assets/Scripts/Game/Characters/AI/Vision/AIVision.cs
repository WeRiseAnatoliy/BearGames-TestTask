using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace TestTask.Game.Characters
{
    public class AIVision : MonoInstaller
    {
        public LayerMask VisionLayer;
        public float Length = 5f;
        [Range(1, 8)] public int Quality = 4;
        [Range(0, 360)] public float FieldOfView = 120f;
        public string[] TagFilter;

        public List<GameObject> Visibled { get; private set; } = new();

        public int VisibledCount => Visibled.Count;

        public override void InstallBindings()
        {
            Container.
               Bind<AIVision>().
               FromInstance(this).
               AsCached().
               NonLazy();
        }

        private void Update()
        {
            CheckVision();
        }

        private void CheckVision()
        {
            var nowVisibled = new List<GameObject>();

            var halfFOV = FieldOfView / 2f;
            var angleStep = FieldOfView / Quality;

            for (var x = 0; x < Quality; x++)
            {
                var angle = -halfFOV + x * angleStep;
                var direction = Quaternion.Euler(0, 0, angle) * transform.right;
                var hit = Physics2D.Raycast(transform.position, direction, Length, VisionLayer);

                if (hit.collider != null)
                {
                    var detected = hit.collider.gameObject;
                    if (detected.TryGetComponent<RaycastTargetTransfer>(out var targetTransfer))
                        detected = targetTransfer.NextObject.gameObject;

                    if (TagFilter.Length == 0 || TagFilter.Contains(detected.tag))
                    {
                        nowVisibled.Add(detected);

                        if (!Visibled.Contains(detected))
                            Visibled.Add(detected);
                    }
                }
            }

            Visibled.RemoveAll(x => nowVisibled.Contains(x) == false);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;

            var halfFOV = FieldOfView / 2f;
            var angleStep = FieldOfView / Quality;

            for (var x = 0; x < Quality; x++)
            {
                var angle = -halfFOV + x * angleStep;
                var direction = Quaternion.Euler(0, 0, angle) * transform.right;
                Gizmos.DrawRay(transform.position, direction * Length);
            }
        }
    }
}
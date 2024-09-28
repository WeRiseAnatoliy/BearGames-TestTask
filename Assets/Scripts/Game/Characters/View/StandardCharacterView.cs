using UnityEngine;
using Zenject;

namespace TestTask.Game.Characters
{
    public class StandardCharacterView : MonoInstaller, ICharacterView
    {
        [SerializeField] Animator animator;
        [SerializeField] string movingParam = "moving";

        public override void InstallBindings()
        {
            Container.
                Bind<ICharacterView>().
                FromInstance(this).
                AsCached().
                NonLazy();
        }

        public void PlayClip(string nodeName, int layer = 0)
        {
            animator.Play(nodeName, layer);
        }

        public void SetMoving(float direction)
        {
            animator.SetFloat(movingParam, direction);
        }
    }
}
using UnityEngine;
using Zenject;

namespace TestTask.Game.Characters
{
    public class StandardCharacterView : MonoInstaller, ICharacterView
    {
        [SerializeField] Animator animator;
        [SerializeField] string movingParam = "moving";

        public Transform Root => animator.transform;

        public override void InstallBindings()
        {
            Container.
                Bind<ICharacterView>().
                FromInstance(this).
                AsCached().
                NonLazy();
        }

        public bool IsCurrentPlay(string nodeName, int layer = 0)
        {
            return animator.GetCurrentAnimatorStateInfo(layer).IsName(nodeName);
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
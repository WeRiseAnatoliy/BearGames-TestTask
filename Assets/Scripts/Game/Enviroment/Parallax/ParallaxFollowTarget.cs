using Zenject;

namespace TestTask.Game.Enviroment
{
    public class ParallaxFollowTarget : MonoInstaller
    {
        public System.Action<float> OnChangePosition;

        private float oldPosition;

        public override void InstallBindings()
        {
            Container.
                Bind<ParallaxFollowTarget>().
                FromInstance(this).
                AsCached().
                NonLazy();
        }

        private void Update()
        {
            if (transform.position.x != oldPosition)
                OnChangePosition?.Invoke(oldPosition - transform.position.x);
            oldPosition = transform.position.x;
        }
    }
}
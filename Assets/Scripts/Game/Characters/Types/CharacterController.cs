using MonsterLove.StateMachine;
using Sirenix.OdinInspector;
using TestTask.Game.Vitals;
using Zenject;

namespace TestTask.Game.Characters
{
    public abstract class CharacterController : MonoInstaller, ICharacterController
    {
        [Inject, ShowInInspector, ReadOnly, FoldoutGroup("Debug")]
        public IHealthController Health { get; private set; }

        [Inject, ShowInInspector, ReadOnly, FoldoutGroup("Debug")]
        public ICharacterView View { get; private set; }


        public StateMachine<CharacterStateType> States;

        public override void InstallBindings()
        {
            Container.
                Bind<ICharacterController>().
                FromInstance(this).
                AsCached().
                NonLazy();
        }

        public override void Start()
        {
            States = new StateMachine<CharacterStateType>(this);
            States.ChangeState(CharacterStateType.Live);

            Health.Health.OnValueChanged += OnHealthChanged;
            Health.WasDead.OnValueChanged += OnDeadChanged;
        }

        protected virtual void OnHealthChanged(float old, float current)
        {
            if (current > 0)
            {
                if (current < old)
                    View.PlayClip("Hit");
            }
            else
                View.PlayClip("Dead");
        }

        protected virtual void Update ()
        {
            //I know, it's bad, but something wrong with State Machine library. State update is not called
            if (States != null && States.State == CharacterStateType.Live)
                Live_Update();
        }

        private void OnDeadChanged(bool old, bool now)
        {
            if (now)
                States.ChangeState(CharacterStateType.Dead);
            else
                States.ChangeState(CharacterStateType.Live);
        }

        #region States
        protected virtual void Live_Enter()
        {
            //for example - disable ragdoll or something else
        }

        protected virtual void Live_Update()
        {

        }

        protected virtual void Dead_Enter()
        {
            //for example - enable ragdoll or something else
        }

        protected virtual void Dead_Update()
        {

        }
        #endregion
    }

    public enum CharacterStateType
    {
        Live,
        Dead
    }
}
using MonsterLove.StateMachine;
using Sirenix.OdinInspector;
using UnityEngine;

namespace TestTask.Game.Characters
{
    public class AICharacterController : UltimateCharacterController
    {
        public StateMachine<AIBehaviourState> AIState;

        [ShowInInspector, ReadOnly, FoldoutGroup("Debug")]
        private IAIBehaviourStrategy behaviour;

        public override void InstallBindings()
        {
            base.InstallBindings();

            Container.
                Bind<AICharacterController>().
                FromInstance(this).
                AsCached().
                NonLazy();

            if(TryGetComponent(out behaviour))
            {
                Container.
                    Bind<IAIBehaviourStrategy>().
                    FromInstance(behaviour).
                    AsCached().
                    NonLazy();
            }
            else
            {
                Debug.LogError($"Need {nameof(IAIBehaviourStrategy)}");
            }
        }

        public override void Start()
        {
            base.Start();

            AIState = new StateMachine<AIBehaviourState>(this);
        }

        protected override void Live_Update()
        {
            base.Live_Update();

            behaviour?.LifeUpdate();
        }
    }

    public enum AIBehaviourState
    {
        Patrol,
        StayOnPatrolPoint,
        Pursuit,
        CooldownAfterLostTarget
    }
}
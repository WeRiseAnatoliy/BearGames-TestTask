using MonsterLove.StateMachine;
using UnityEngine;

namespace TestTask.Game.Characters
{
    public class AIStandardPatrolPursuitBehaviour : AIStrategyBase
    {
        public StateMachine<StandardAIBehaviourStates> State;

        private void Start()
        {
            State = new StateMachine<StandardAIBehaviourStates>(this);
        }

        public override void LifeUpdate()
        {

        }

        private void IdleStay_Enter ()
        {

        }

        public enum StandardAIBehaviourStates
        {
            IdleStay,
            Patrol,
            TargetVisibled,
            Pursuit,
            TargetIntoAttackRange,
            Attack
        }
    }
}
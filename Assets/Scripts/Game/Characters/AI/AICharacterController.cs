using MonsterLove.StateMachine;

namespace TestTask.Game.Characters
{
    public class AICharacterController : CharacterController
    {
        public StateMachine<AIBehaviourState> AIState;

        public override void Start()
        {
            base.Start();

            AIState = new StateMachine<AIBehaviourState>(this);
        }

        protected override void Live_Update()
        {

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
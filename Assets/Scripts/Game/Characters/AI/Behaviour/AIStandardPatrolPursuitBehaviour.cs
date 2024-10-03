using MonsterLove.StateMachine;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

namespace TestTask.Game.Characters
{
    public class AIStandardPatrolPursuitBehaviour : AIStrategyBase
    {
        public StateMachine<StandardAIBehaviourStates> State;

        public Vector2 IdleStayTimerRange = new Vector2(1, 3);
        public PatrolSettings Patrol;

        private Vector3 startPos;

        private void Start()
        {
            startPos = transform.position;
            State = new StateMachine<StandardAIBehaviourStates>(this);
            State.ChangeState(StandardAIBehaviourStates.IdleStay);
        }

        public override void LifeUpdate()
        {
            //The same problem into CharacterController, void Update
            switch(State.State)
            {
                case StandardAIBehaviourStates.IdleStay:
                    IdleStay_Update();
                    break;
            }
        }

        private IEnumerator IdleStay_Enter ()
        {
            Debug.Log($"Idle stay begin");
            owner.ClearNavigation();
            yield return new WaitForSeconds(Random.Range(IdleStayTimerRange.x, IdleStayTimerRange.y));
            State.ChangeState(StandardAIBehaviourStates.Patrol);
            Debug.Log($"Idle stay end");
        }

        private void IdleStay_Update ()
        {
            Debug.Log($"Strategy idle update");
        }

        private void Patrol_Enter ()
        {
            var point = Patrol.GetPointPos(owner, startPos);
            owner.MoveTo(new AICharacterController.NavigationData(point, OnPatrolPointAchieved));
        }

        private void OnPatrolPointAchieved (bool success)
        {
            State.ChangeState(StandardAIBehaviourStates.IdleStay);
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

        [System.Serializable]
        public class PatrolSettings
        {
            public PatrolPointType PathType;

            [ShowIf(nameof(useTransformPath))] public Transform[] TransformPath;
            [ShowIf(nameof(useVectorPath))] public Vector3[] VectorPath;
            [ShowIf(nameof(useRadius))] public float Radius;

            private bool useTransformPath =>
                PathType == PatrolPointType.TransformsPath;

            private bool useVectorPath =>
                PathType == PatrolPointType.VectorsPath;

            private bool useRadius =>
                PathType == PatrolPointType.RandomFromRadius_StartPos || PathType == PatrolPointType.RandomFromRadius_OriginPos;

            public Vector3 GetPointPos (AICharacterController ai, Vector3 aiStartPos)
            {
                switch(PathType)
                {
                    case PatrolPointType.RandomFromRadius_StartPos:
                        return aiStartPos + Vector3.right * Random.Range(-Radius, Radius);
                    case PatrolPointType.RandomFromRadius_OriginPos:
                        return ai.transform.position + Vector3.right * Random.Range(-Radius, Radius);

                    default:
                        return ai.transform.position;
                }
            }

            public enum PatrolPointType
            {
                TransformsPath,
                VectorsPath,
                RandomFromRadius_StartPos,
                RandomFromRadius_OriginPos
            }
        }
    }
}
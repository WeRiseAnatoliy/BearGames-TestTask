using MonsterLove.StateMachine;
using Sirenix.OdinInspector;
using System.Collections;
using TestTask.Game.Vitals;
using UnityEngine;

namespace TestTask.Game.Characters
{
    public class AIStandardPatrolPursuitBehaviour : AIStrategyBase
    {
        public StateMachine<StandardAIBehaviourStates> State;

#if UNITY_EDITOR
        [ShowInInspector, ReadOnly]
        public StandardAIBehaviourStates debugState =>
            State != null ? State.State : StandardAIBehaviourStates.IdleStay;
#endif

        [MinMaxSlider(0, 30, showFields: true)] public Vector2 IdleStayTimerRange = new Vector2(1, 3);
        [MinMaxSlider(0, 15, showFields: true)] public Vector2 TargetDetectedTimerRange = new Vector2(1, 3);
        public PatrolSettings Patrol;

        private Vector3 startPos;
        private GameObject target;

        private void Start()
        {
            startPos = transform.position;
            State = new StateMachine<StandardAIBehaviourStates>(this);
            State.ChangeState(StandardAIBehaviourStates.IdleStay);
        }

        public override void LifeUpdate()
        {
            //The same problem into CharacterController, void Update

            if (State.State != StandardAIBehaviourStates.Pursuit &&
                State.State != StandardAIBehaviourStates.TargetVisibled &&
                owner.Vision.VisibledCount > 0)
            {
                State.ChangeState(StandardAIBehaviourStates.TargetVisibled);
            }
        }

        #region Idle State
        private IEnumerator IdleStay_Enter()
        {
            Debug.Log($"Idle stay begin");
            owner.ClearNavigation();
            yield return new WaitForSeconds(Random.Range(IdleStayTimerRange.x, IdleStayTimerRange.y));
            State.ChangeState(StandardAIBehaviourStates.Patrol);
            Debug.Log($"Idle stay end");
        }

        private void IdleStay_Update()
        {
            Debug.Log($"Strategy idle update");
        }
        #endregion

        #region Patrol State
        private void Patrol_Enter()
        {
            var point = Patrol.GetPointPos(owner, startPos);
            owner.MoveTo(new AICharacterController.NavigationData(point, OnPatrolPointAchieved));
        }

        private void OnPatrolPointAchieved(bool success)
        {
            State.ChangeState(StandardAIBehaviourStates.IdleStay);
        }
        #endregion

        #region Target Visibled State
        private IEnumerator TargetVisibled_Enter()
        {
            owner.ClearNavigation();

            var timerForDetect = Random.Range(TargetDetectedTimerRange.x, TargetDetectedTimerRange.y);

            while (timerForDetect > 0)
            {
                if (owner.Vision.VisibledCount == 0)
                {
                    State.ChangeState(StandardAIBehaviourStates.IdleStay);
                    break;
                }

                yield return new WaitForFixedUpdate();
                timerForDetect -= Time.fixedDeltaTime;
            }

            if (timerForDetect <= 0 && owner.Vision.VisibledCount > 0)
            {
                target = owner.Vision.Visibled[0];
                State.ChangeState(StandardAIBehaviourStates.Pursuit);
            }
        }
        #endregion

        #region Pursuit State
        private void Pursuit_Enter()
        {
            owner.MoveTo(new AICharacterController.NavigationData(target.transform, OnPursuitResult, true));
        }

        private void OnPursuitResult(bool result)
        {
            if(target.TryGetComponent<IHealthController>(out var enemyHealth))
            {
                if (enemyHealth.WasDead.Value)
                {
                    target = null;
                    State.ChangeState(StandardAIBehaviourStates.IdleStay);
                    return;
                }
            }

            if (result)
            {
                State.ChangeState(StandardAIBehaviourStates.Attack);
            }
            else
            {
                Pursuit_Enter();
            }
        }
        #endregion

        #region Attack State
        private IEnumerator Attack_Enter ()
        {
            owner.Attack();
            while (owner.View.IsCurrentPlay("Attack"))
                yield return new WaitForFixedUpdate();
            State.ChangeState(StandardAIBehaviourStates.Pursuit);
        }
        #endregion

        public enum StandardAIBehaviourStates
        {
            IdleStay,
            Patrol,
            TargetVisibled,
            Pursuit, 
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

            private int lastUsedPathIndex;

            public Vector3 GetPointPos (AICharacterController ai, Vector3 aiStartPos)
            {
                switch(PathType)
                {
                    case PatrolPointType.RandomFromRadius_StartPos:
                        return aiStartPos + Vector3.right * Random.Range(-Radius, Radius);
                    case PatrolPointType.RandomFromRadius_OriginPos:
                        return ai.View.Root.position + Vector3.right * Random.Range(-Radius, Radius);
                    case PatrolPointType.TransformsPath:
                        lastUsedPathIndex++;
                        if (lastUsedPathIndex >= TransformPath.Length)
                            lastUsedPathIndex = 0;
                        return TransformPath[lastUsedPathIndex].position;
                    case PatrolPointType.VectorsPath:
                        lastUsedPathIndex++;
                        if (lastUsedPathIndex >= VectorPath.Length)
                            lastUsedPathIndex = 0;
                        return aiStartPos + VectorPath[lastUsedPathIndex];
                    default:
                        return ai.View.Root.position;
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
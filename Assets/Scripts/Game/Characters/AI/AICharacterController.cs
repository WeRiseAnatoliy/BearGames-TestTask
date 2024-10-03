using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace TestTask.Game.Characters
{
    public class AICharacterController : TwoDUltimateCharacterController
    {
        [ShowInInspector, ReadOnly, FoldoutGroup("Debug")]
        private IAIBehaviourStrategy behaviour;

        [ShowInInspector, FoldoutGroup("Navigation")]
        public float AchievedRadius = 1f;

        [ShowInInspector, ReadOnly, FoldoutGroup("Debug")]
        public NavigationData? Navigation { get; private set; }

        public override void InstallBindings()
        {
            base.InstallBindings();

            Container.
                Bind<AICharacterController>().
                FromInstance(this).
                AsCached().
                NonLazy();

            if (TryGetComponent(out behaviour))
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

        protected override void Live_Update()
        {
            base.Live_Update();

            behaviour?.LifeUpdate();

            HandleNavigation();
        }

        private void HandleNavigation()
        {
            if (Navigation == null)
                return;

            var distanceToTarget = Vector2.Distance(transform.position, Navigation.Value.ResultPosition);
            if (distanceToTarget < AchievedRadius)
            {
                View.SetMoving(0);
                Navigation.Value.Result?.Invoke(true);
                Navigation = null;
            }
            else
            {
                View.SetMoving(Navigation.Value.EnableRun ? 2 : 1);
                var navPoint = Navigation.Value.ResultPosition;
                if (navPoint.x > transform.position.x)
                    Move(Vector3.right);
                else
                    Move(Vector3.left);
            }
        }

        public void MoveTo(NavigationData data)
        {
            if (Navigation != null)
                Navigation.Value.Result?.Invoke(false);

            Navigation = data;
        }

        public void ClearNavigation()
        {
            View.SetMoving(0);
            Navigation = null;
        }

        public struct NavigationData
        {
            public Transform Target;
            public Vector3 Point;

            public Action<bool> Result;
            public bool EnableRun;

            public Vector3 ResultPosition =>
                Target ? Target.position : Point;

            public NavigationData(Transform target,
                Action<bool> result,
                bool enableRun = false)
            {
                Target = target;
                Point = Vector3.zero;
                Result = result;
                EnableRun = enableRun;
            }

            public NavigationData(Vector3 point,
                Action<bool> result,
                bool enableRun = false)
            {
                Target = null;
                Point = point;
                Result = result;
                EnableRun = enableRun;
            }
        }
    }
}
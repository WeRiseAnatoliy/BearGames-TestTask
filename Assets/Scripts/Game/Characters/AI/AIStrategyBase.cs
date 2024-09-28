using UnityEngine;
using Zenject;

namespace TestTask.Game.Characters
{
    public abstract class AIStrategyBase : MonoBehaviour, IAIBehaviourStrategy
    {
        [Inject] protected AICharacterController owner { get; private set; }

        public abstract void LifeUpdate();
    }
}
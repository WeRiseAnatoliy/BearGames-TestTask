using UnityEngine;

namespace TestTask.Game.Enviroment
{
    public interface IParallaxLayer
    {
        internal void Move(Vector3 targetMoveDelta);
    }
}
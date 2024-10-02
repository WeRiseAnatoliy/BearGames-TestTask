using UnityEngine;

namespace TestTask.Game.Characters
{
    public interface ICharacterView
    {
        Transform Root { get; }
        void SetMoving(float direction);
        void PlayClip(string nodeName, int layer = 0);
        bool IsCurrentPlay(string nodeName, int layer = 0); 
    }
}
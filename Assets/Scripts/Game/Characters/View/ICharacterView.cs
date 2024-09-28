namespace TestTask.Game.Characters
{
    public interface ICharacterView
    {
        void SetMoving(float direction);
        void PlayClip(string nodeName, int layer = 0);
    }
}
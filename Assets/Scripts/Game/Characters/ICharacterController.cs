using TestTask.Game.Vitals;

namespace TestTask.Game.Characters
{
    public interface ICharacterController
    {
        IHealthController Health { get; }
    }
}
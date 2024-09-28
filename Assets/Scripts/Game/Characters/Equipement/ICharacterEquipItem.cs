namespace TestTask.Game.Characters
{

    public interface ICharacterEquipItem
    {
        float AttackRange { get; }
        float AttackRate { get; }
        float Damage { get; }

        bool IsItemReadyToAttack { get; }
        void Attack();
    }
}
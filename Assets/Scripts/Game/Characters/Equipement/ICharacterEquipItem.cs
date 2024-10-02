namespace TestTask.Game.Characters
{

    public interface ICharacterEquipItem
    {
        ICharacterEquipEvents Events { get; }

        float AttackRange { get; }
        float AttackRate { get; }
        float Damage { get; }

        bool IsItemReadyToAttack { get; }
        void Attack();
    }
}
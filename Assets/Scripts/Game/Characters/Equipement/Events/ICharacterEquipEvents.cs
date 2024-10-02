using System;

namespace TestTask.Game.Characters
{
    public interface ICharacterEquipEvents
    {
        Action Attack { get; set; }
    }
}
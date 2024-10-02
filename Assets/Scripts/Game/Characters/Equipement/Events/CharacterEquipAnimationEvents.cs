using System;
using UnityEngine;

namespace TestTask.Game.Characters
{
    public class CharacterEquipAnimationEvents : MonoBehaviour, ICharacterEquipEvents
    {
        public Action Attack { get; set; }

        public void CallAttackEvent() =>
            Attack?.Invoke();
    }
}
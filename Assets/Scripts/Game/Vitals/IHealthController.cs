using TestTask.Common;
using UnityEngine;

namespace TestTask.Game.Vitals
{
    public interface IHealthController
    {
        ObservedValue<float> Health { get; set; }
        ObservedValue<bool> WasDead { get; }

        void ChangeHealth(ChangeHealthData changeData);
    }

    public struct ChangeHealthData
    {
        public GameObject Sender;
        public float Value;

        public ChangeHealthData(GameObject sender, float value)
        {
            Sender = sender;
            Value = value;
        }
    }
}
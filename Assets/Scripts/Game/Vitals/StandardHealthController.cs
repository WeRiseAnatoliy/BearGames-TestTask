using TestTask.Common;
using UnityEngine;
using Zenject;

namespace TestTask.Game.Vitals
{
    public class StandardHealthController : MonoInstaller, IHealthController
    {
        public const float DEFAULT_HP = 10;

        [SerializeField] ObservedValue<float> _health = new ObservedValue<float>(DEFAULT_HP);
        [SerializeField] ObservedValue<bool> _wasDead = new ObservedValue<bool>();

        public ObservedValue<float> Health { get => _health; set => _health = value; }
        public ObservedValue<bool> WasDead { get => _wasDead; }

        public override void InstallBindings()
        {
            Container.
                Bind<IHealthController>().
                FromInstance(this).
                AsCached().
                NonLazy();
        }

        public void ChangeHealth(ChangeHealthData changeData)
        {
            if (WasDead.Value)
                return;

            HandleDamageData(changeData);

            if (Health.Value <= 0)
                WasDead.Value = true;
        }

        protected virtual void HandleDamageData (ChangeHealthData changeData)
        {
            Health.Value += changeData.Value;
        }
    }
}
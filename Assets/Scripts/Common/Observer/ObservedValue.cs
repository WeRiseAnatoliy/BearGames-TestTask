using UnityEngine;

namespace TestTask.Common
{
    [System.Serializable]
    public class ObservedValue<T>
    {
        public T Value
        {
            get
            {
                return _value;
            }
            set
            {
                if(object.Equals(value, _value) == false)
                {
                    OnValueChanged?.Invoke(_value, value);
                    _value = value;
                }
            }
        }

        [SerializeField] T _value;

        public event System.Action<T, T> OnValueChanged;

        public ObservedValue(T value = default)
        {
            _value = value;
        }
    }
}
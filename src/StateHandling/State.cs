using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using src.StateHandling;

namespace src
{
    public class State<T>
    {
        public State(T initialValue)
        {
            _value = initialValue;
            _lastSet = DateTime.MinValue;
        }

        private T _value;
        private DateTime _lastSet;
        private object _lock = new();

        public T Value
        {
            get
            {
                lock (_lock)
                {
                    return _value;
                }
            }

            set
            {
                lock (_lock)
                {
                    _value = value;
                    _lastSet = DateTime.Now;
                }
            }
        }

        public DateTime LastSet
        {
            get
            {
                lock (_lock)
                {
                    return _lastSet;
                }
            }
        }

        public IsSet<T> UpdatedIn(TimeSpan time)
        {
            lock (_lock)
            {
                if (_lastSet.Add(time) >= DateTime.Now)
                {
                    return _value;
                }

                return IsSet<T>.Unset();
            }
        }

        public ReadonlyState<T> SafeCopy()
        {
            lock (_lock)
            {
                return new ReadonlyState<T>(_value, _lastSet);
            }
        }

        public static implicit operator State<T>(T value) => new State<T>(value);
        public static implicit operator T(State<T> state) => state.Value;

        public override string ToString()
        {
            return $"[{(_lastSet == DateTime.MinValue ? "never" : _lastSet)}] {Value?.ToString() ?? "NULL"}";
        }
    }
}
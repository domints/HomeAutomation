using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace src
{
    public class Locked<T>
    {
        public Locked(T initialValue)
        {
            _value = initialValue;
        }

        private T _value;
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
                }
            }
        }

        public override string ToString()
        {
            return Value?.ToString() ?? "NULL";
        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace src
{
    public class IsSet<T>
    {
        public bool Set { get; init; }
        public T? Value { get; init; }

        public static implicit operator T?(IsSet<T> value) => value.Value;
        public static implicit operator IsSet<T>(T value) => new IsSet<T> { Set = true, Value = value };
        public static IsSet<T> Unset() => new IsSet<T> { Set = false };
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace src.StateHandling
{
    public record ReadonlyState<T>(T Value, DateTime LastSet)
    {
        public static implicit operator T(ReadonlyState<T> state) => state.Value;
    };
}
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsolasEngine
{
    public class SequenceEqualityComparer<T> : IEqualityComparer<IEnumerable<T>>
    {
        public bool Equals(IEnumerable<T> x, IEnumerable<T> y)
        {
            return Object.ReferenceEquals(x, y) || (x != null && y != null && x.SequenceEqual(y));
        }

        public int GetHashCode(IEnumerable<T> obj)
        {
            if (obj == null)
                return 0;

            return unchecked(obj.Select(e => e.GetHashCode()).Aggregate(0, (a, b) => a + b));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlidingPuzzleEngine
{
    public class HeuristicComparer<T> : IComparer<T>
    {
        private Func<T, T, int> comparer;
        public HeuristicComparer(Func<T, T, int> comparer)
        {
            this.comparer = comparer;
        }
        public static IComparer<T> Create(Func<T, T, int> comparer)
        {
            return new HeuristicComparer<T>(comparer);
        }
        public int Compare(T x, T y)
        {
            return comparer(x, y);
        }
    }
}

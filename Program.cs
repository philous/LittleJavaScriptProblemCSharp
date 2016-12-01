using System;

namespace ConsoleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Func<int, int> square = x => x*x;
            Action<int> logger = x => { Console.WriteLine(x.ToString()); };

            Func<int, int, Pair> range = null;
            range = (x, y) => { return new Pair(x, () => x == y ? null : range(x + 1, y)); };

            Func<Pair, Func<int, int>, Pair> map = null;
            map = (list, action) => { return new Pair(action.Invoke(list.Current), () => list.GetNext() == null ? null : map(list.GetNext(), action)); };

            Func<Pair, Pair> reverse = list => {
                                           Func<Pair, Pair, Pair> internalReverse = null;
                                           internalReverse = (l, newList) => { return l == null ? newList : internalReverse(l.GetNext(), new Pair(l.Current, () => newList)); };

                                           return internalReverse.Invoke(list, null);
                                       };

            Action<Pair, Action<int>> freach = null;
            freach = (list, log) => {
                         log.Invoke(list.Current);
                         if (list.GetNext() != null)
                             freach.Invoke(list.GetNext(), log);
                     };


            var numbers = range.Invoke(1, 10);
            numbers = map.Invoke(numbers, square);
            numbers = reverse.Invoke(numbers);

            freach.Invoke(numbers, logger);
        }
    }

    public class Pair {
        public Pair(int current, Func<Pair> next) {
            Current = current;
            Next = next;
        }

        public int Current { get; set; }

        public Func<Pair> Next { get; set; }

        public Pair GetNext() {
            return Next.Invoke();
        }
    }
}
